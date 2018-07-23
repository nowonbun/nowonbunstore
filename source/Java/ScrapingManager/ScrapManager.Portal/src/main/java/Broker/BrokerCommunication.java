package Broker;

import java.io.IOException;
import java.net.ServerSocket;
import java.util.concurrent.Executors;
import org.apache.log4j.Logger;
import Common.LoggerManager;
import Common.PropertyManager;
import Websocket.WorkSocket;

public class BrokerCommunication extends ServerSocket {

	private BrokerMethod method = null;
	private static BrokerCommunication singleton = null;
	private Logger logger = LoggerManager.getLogger(BrokerCommunication.class);

	public static BrokerCommunication newInstance(BrokerMethod method) {
		if (singleton != null) {
			singleton.logger.error("[SCRAP LOG] Already scraper was excuted");
			throw new RuntimeException("Already scraper was excuted");
		}
		try {
			singleton = new BrokerCommunication();
			singleton.method = method;
			return singleton;
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	private BrokerCommunication() throws IOException {
		super(PropertyManager.getPropertyInt("project", "ListenPort"));
		Executors.newCachedThreadPool().execute(() -> {
			try {
				while (true) {
					BrokerUnit broker = new BrokerUnit(super.accept());
					WorkSocket.setBroker(broker);
					Executors.newCachedThreadPool().execute(() -> {
						try {
							while (true) {
								receive(broker, broker.receive());
							}
						} catch (Throwable e) {
							WorkSocket.removeBroker(broker);
							broker.close();
						}
					});
				}
			} catch (Throwable e) {
				logger.error(e);
			}
		});
	}

	private void receive(BrokerUnit broker, String data) {
		Executors.newCachedThreadPool().execute(() -> {
			method.run(broker, data);
		});
	}

	public static void send(BrokerUnit broker, String data) throws IOException {
		broker.send(data);
	}
}
