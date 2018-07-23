package Websocket;

import javax.websocket.EndpointConfig;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;
import org.apache.log4j.Logger;
import Common.LoggerManager;
import common.FactoryDao;
import dao.KeyDataDao;
import entity.KeyData;
import java.io.IOException;
import java.io.StringReader;
import java.io.StringWriter;
import java.util.ArrayList;
import java.util.List;
import javax.json.Json;
import javax.json.JsonObject;
import javax.json.JsonObjectBuilder;
import javax.json.JsonReader;
import javax.json.JsonWriter;
import javax.json.stream.JsonParsingException;
import Common.Util;

@ServerEndpoint(value = "/index")
public class WorkWebSocket {

	private static List<Session> sessionlist = new ArrayList<>();
	private Logger logger = LoggerManager.getLogger(WorkWebSocket.class);

	public static void sendBroadcast(String key, String value) {
		try {
			for (Session sess : sessionlist) {
				sess.getBasicRemote().sendText(createJson(key, value));
			}
		} catch (IOException e) {
			LoggerManager.getLogger(WorkWebSocket.class).error(e);
		}
	}

	@OnOpen
	public void handleOpen(Session socketSession, EndpointConfig config) {
		// TODO: This need confirm.
		logger.info("[WEB LOG] connect - " + socketSession.getBasicRemote().toString());
		sessionlist.add(socketSession);
	}

	@OnClose
	public void handleClose(Session socketSession) {
		// TODO: This need confirm.
		logger.info("[WEB LOG] disconnect - " + socketSession.getBasicRemote().toString());
		sessionlist.remove(socketSession);
	}

	@OnMessage
	public void handleMessage(String message, Session socketSession) {
		try {
			String ret = "";
			logger.info("[WEB LOG] message - " + message);
			try (JsonReader jsonReader = Json.createReader(new StringReader(message))) {
				JsonObject object = jsonReader.readObject();
				ret = work(object.getString("key"), object.getString("value"));
			} catch (JsonParsingException e) {
				logger.error(e);
				ret = createJson("error", "An unexpected error has occurred.");
			}
			socketSession.getBasicRemote().sendText(ret);
			logger.info("[WEB LOG] return - " + ret);
		} catch (Throwable e) {
			logger.error(e);
			throw new RuntimeException(e);
		}
	}

	private String work(String key, String value) {
		if ("CREATEKEY".equals(key.toUpperCase())) {
			try (JsonReader jsonReader = Json.createReader(new StringReader(value))) {
				JsonObject object = jsonReader.readObject();
				KeyData node = new KeyData(Util.createUID());
				node.setBizno(object.getString("bizno"));
				node.setName(object.getString("name"));
				node.setIp(object.getString("ip"));
				node.setCallback(object.getString("callback"));
				FactoryDao.getDao(KeyDataDao.class).merge(node);
				return createJson("createkey", "");
			} catch (JsonParsingException e) {
				logger.error(e);
			}
		} else if ("APIKEYLIST-DEL".equals(key.toUpperCase())) {
			KeyData node = new KeyData(value);
			FactoryDao.getDao(KeyDataDao.class).delete(node);
			return createJson("apikeylist-del", "");
		}
		return createJson("error", "A undefined key has called.");
	}

	protected static String createJson(String key, String value) {
		JsonObjectBuilder builder = Json.createObjectBuilder();
		builder.add("key", key);
		builder.add("value", value);
		JsonObject jsonObject = builder.build();
		StringWriter stringwriter = new StringWriter();
		try (JsonWriter jsonWriter = Json.createWriter(stringwriter)) {
			jsonWriter.write(jsonObject);
		}
		return stringwriter.toString();
	}
}
