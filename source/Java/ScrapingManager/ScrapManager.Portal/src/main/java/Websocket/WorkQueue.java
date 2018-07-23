package Websocket;

import java.io.IOException;
import java.util.LinkedHashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.Executors;
import org.apache.log4j.Logger;
import Broker.BrokerUnit;
import Common.LoggerManager;
import Entity.Parameter;
import common.FactoryDao;
import dao.BrokerDao;

public class WorkQueue {

	private List<Parameter> waitqueue;
	private Map<BrokerUnit, List<Parameter>> runningqueue;
	private static WorkQueue singleton = null;
	private Logger logger = LoggerManager.getLogger(WorkQueue.class);

	public static WorkQueue instance() {
		if (singleton == null) {
			singleton = new WorkQueue();
		}
		return singleton;
	}

	/**
	 * singleton pattern
	 */
	private WorkQueue() {
		logger.info("[SYSTEM LOG] This workqueue is started.");
		waitqueue = new LinkedList<>();
		runningqueue = new LinkedHashMap<>();

		Executors.newCachedThreadPool().execute(() -> {
			while (true) {
				try {
					if (waitqueue.isEmpty()) {
						waitThread(1);
						continue;
					}
					Parameter node = waitqueue.remove(0);
					BrokerUnit broker = getBroker();
					if (broker == null) {
						logger.error("There are not the applied broker.");
						synchronized (waitqueue) {
							waitqueue.add(node);
						}
						waitThread(1);
						continue;
					}

					if (!excuteScrap(broker, node)) {
						synchronized (waitqueue) {
							waitqueue.add(node);
						}
					}
				} catch (Throwable e) {
					logger.error(e);
				}
			}
		});
	}

	private BrokerUnit getBroker() {
		int size = 1000000;
		BrokerUnit broker = null;
		synchronized (runningqueue) {
			for (BrokerUnit b : runningqueue.keySet()) {
				int s = runningqueue.get(b).size();
				if (s < size) {
					broker = b;
					size = s;
				}
			}
		}
		return broker;
	}

	private void waitThread(int sec) {
		try {
			Thread.sleep(1000 * sec);
		} catch (Throwable e) {
			e.printStackTrace();
		}
	}

	public int getNodeCount(BrokerUnit broker) {
		if (!runningqueue.containsKey(broker)) {
			return 0;
		}
		return runningqueue.get(broker).size();
	}

	public void insert(Parameter node) {
		logger.info("[SYSTEM LOG] The node is applied - " + node.getKey());
		logger.info("[SYSTEM LOG] Id - " + node.getId1());
		synchronized (waitqueue) {
			waitqueue.add(node);
		}
		logger.info("[SYSTEM LOG] The Uid is generated- " + node.getKey());
	}

	private boolean excuteScrap(BrokerUnit broker, Parameter node) {
		try {
			String json = node.toJson();
			logger.info("[SYSTEM LOG] broker - " + broker.getIp());
			logger.info("[SYSTEM LOG] node pop - code" + node.getKey() + "uid" + node.getKey());
			logger.info("[SYSTEM LOG] excute json" + json);
			broker.send(json);
			WorkQueue.Async(() -> {
				broker.modifyCount(1);
			});
		} catch (IOException e) {
			logger.error(e);
			// return false;
		}
		synchronized (runningqueue) {
			node.setBroker(broker);
			runningqueue.get(broker).add(node);
		}
		return true;
	}

	public void remove(BrokerUnit broker, String uid) {
		if (!runningqueue.containsKey(broker)) {
			throw new RuntimeException("Not node");
		}
		List<Parameter> list = runningqueue.get(broker);
		Optional<Parameter> ret = list.stream().filter(x -> uid.equals(x.getKey())).findFirst();
		if (ret.isPresent()) {
			Parameter node = list.stream().filter(x -> uid.equals(x.getKey())).findFirst().get();
			synchronized (list) {
				list.remove(node);
			}
		}
		WorkQueue.Async(() -> {
			broker.modifyCount(-1);
		});
	}

	public void addBroker(BrokerUnit broker) {
		synchronized (runningqueue) {
			runningqueue.put(broker, new LinkedList<>());
		}
		try {
			broker.ping();
		} catch (Throwable e) {
			removeBroker(broker);
			return;
		}
		WorkQueue.Async(() -> {
			FactoryDao.getDao(BrokerDao.class).merge(broker);
			logger.info("[SYSTEM LOG] The broker is applied - " + broker.getIp());
		});
	}

	public void removeBroker(BrokerUnit broker) {
		synchronized (runningqueue) {
			List<Parameter> node = runningqueue.remove(broker);
			// The remained node is forwarded to waitlist.
			// instance.waitqueue.addAll(node);
		}
		WorkQueue.Async(() -> {
			broker.ChangeStatusToEnd();
			logger.info("[SYSTEM LOG] The broker is unapplied - " + broker.getIp());
		});
	}

	public static void Async(Runnable func) {
		Executors.newCachedThreadPool().execute(() -> {
			func.run();
		});
	}
}
