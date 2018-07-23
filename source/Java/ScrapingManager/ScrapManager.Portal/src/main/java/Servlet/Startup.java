package Servlet;

import java.io.StringReader;
import javax.json.Json;
import javax.json.JsonObject;
import javax.json.JsonReader;
import javax.json.stream.JsonParsingException;
import org.apache.log4j.Logger;
import Broker.BrokerCommunication;
import Common.AbstractServlet;
import Common.LoggerManager;
import Common.PropertyManager;
import Entity.ResultType;
import Entity.ScrapNode;
import Websocket.WorkQueue;
import common.FactoryDao;
import common.Service;
import dao.CommonDataDao;
import dao.KeyNodeDao;
import dao.KeyDataDao;
import dao.PackageDataDao;
import dao.RequestDataDao;
import dao.ResultDataDao;
import java.io.*;
import java.net.*;

public class Startup extends AbstractServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(Startup.class);

	public Startup() {
		super();
	}

	public void init() {
		// TODO:: BrokerCommunication
		logger.info("ScrapManager is starting.");
		String cn = PropertyManager.getProperty("project", "CASSANDRA_CLUSTER_NAME");
		String id = PropertyManager.getProperty("project", "CASSANDRA_ID");
		String pw = PropertyManager.getProperty("project", "CASSANDRA_PW");
		String ks = PropertyManager.getProperty("project", "CASSANDRA_KEYSPACE");
		Service.newInstance(cn, id, pw, ks);

		//FactoryDao.getDao(BrokerDao.class).truncate();

		BrokerCommunication.newInstance((broker, msg) -> {
			if (msg.length() == 4 && "PONG".equals(msg.toUpperCase())) {
				try {
					Thread.sleep(10000);
					broker.ping();
					logger.info("[SCRAP LOG] PONG - " + broker.getIp());
				} catch (Throwable e) {
					logger.error(e);
				}
				return;
			}
			ScrapNode node = null;
			try {
				node = getNode(msg);
				logger.debug("[SCRAP LOG] Receive message - " + msg);
			} catch (Throwable e) {
				logger.error("[SCRAP LOG] error " + e);
				logger.error("[SCRAP LOG] Receive message - " + msg);
				return;
			}
			try {
				if (node.getResultType() == ResultType.Common) {
					FactoryDao.getDao(CommonDataDao.class).merge(node.getCommonData());
				} else if (node.getResultType() == ResultType.Pacakage) {
					FactoryDao.getDao(PackageDataDao.class).merge(node.getPackageData());
				} else if (node.getResultType() == ResultType.Exit || node.getResultType() == ResultType.Error) {
					// TODO:: Exit와 Error구분을 주는 이유가 있나요?? 안에 node안에 resultcd를 참조하여 실행되면 되지 않을까요?
					// TODO:: 현재 Error의 처리는 없습니다. 스크래핑상의 try~catch를 위해 만들어 놓은 것이라.
					//        여러가지 패턴을 생각하고 있습니다. Error로 올경우는 스크래핑을 다시 시작해야 한다던가.
					//        현재는 에러를 거의 고려하지 않고 작성중이지만 나중에는 꼭 필요한 파라미터입니다.          
					FactoryDao.getDao(ResultDataDao.class).merge(node.getResultData());
					WorkQueue.instance().remove(broker, node.getKey());
					int mallkey = FactoryDao.getDao(KeyNodeDao.class).getMallkey(node.getKey());
					String apikey = FactoryDao.getDao(RequestDataDao.class).getApikey(mallkey, node.getKey());
					String callbackurl = FactoryDao.getDao(KeyDataDao.class).getCallback(apikey);
					getUrl(callbackurl, node.getResultJson());
				} else {
					throw new RuntimeException("[SYSTEM LOG] It's wrong type");
				}
			} catch (Throwable e) {
				logger.error("[SCRAP LOG] Startup Error - " + e);
				throw new RuntimeException(e);
			}
		});
	}

	private ScrapNode getNode(String msg) {
		try (JsonReader jsonReader = Json.createReader(new StringReader(msg))) {
			JsonObject object = jsonReader.readObject();
			String key = object.getString("Key");
			int mallkey = FactoryDao.getDao(KeyNodeDao.class).getMallkey(key);
			ScrapNode node = new ScrapNode(mallkey, key);
			node.setResultType(object.getInt("Resulttype"));
			if (node.getResultType() == ResultType.Common) {
				node.setCommonData(object);
			} else if (node.getResultType() == ResultType.Pacakage) {
				node.setPackageData(object);
			} else if (node.getResultType() == ResultType.Exit) {
				node.setResultData(object);
			} else if (node.getResultType() == ResultType.Error) {
				node.setResultData(object);
			}
			return node;
		} catch (JsonParsingException e) {
			logger.error("[SCRAP LOG] getNode Error - " + e);
			logger.error("[SCRAP LOG] getNode Msg - " + msg);
			throw new RuntimeException(e);
		}
	}
	
	/**
	 * http request
	 * @param callbackurl
	 * @param param
	 */
	private void getUrl(String callbackurl, String param) 
	{
		URL url = null;
		URLConnection urlConnection = null;
		try {
			url = new URL(callbackurl + "?DATA=" + URLEncoder.encode(param,"UTF-8"));
            urlConnection = url.openConnection();
            printByInputStream(urlConnection.getInputStream());
		}catch(Exception e) {
			e.printStackTrace();
		}
	}
	
	public static void printByInputStream(InputStream is) {
        byte[] buf = new byte[1024];
        int len = -1;
        try {
            while((len = is.read(buf, 0, buf.length)) != -1) {
                System.out.write(buf, 0, len);
            }
        } catch(IOException e) {
            e.printStackTrace();
        }
    }

	@Override
	protected void doGet() {
		super.setStatus(500);
	}

	@Override
	protected void doPost() {
		super.setStatus(500);
	}
}
