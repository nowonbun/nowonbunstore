package Websocket;

import java.util.ArrayList;
import java.util.List;
import Broker.BrokerUnit;

public class WorkSocket {
	private List<BrokerUnit> brokerlist = new ArrayList<>();
	private static WorkSocket singleton = null;
	//private Logger logger = LoggerManager.getLogger(WorkSocket.class);

	private WorkSocket() {
	}

	private static WorkSocket instance() {
		if (singleton == null) {
			singleton = new WorkSocket();
		}
		return singleton;
	}

	public static void setBroker(BrokerUnit broker) {
		instance().brokerlist.add(broker);
		WorkQueue.instance().addBroker(broker);
	}

	public static void removeBroker(BrokerUnit broker) {
		instance().brokerlist.remove(broker);
		WorkQueue.instance().removeBroker(broker);
	}

	public static List<BrokerUnit> getSocketList() {
		return instance().brokerlist;
	}
}
