package Broker;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Date;
import java.util.List;
import javax.json.Json;
import javax.json.JsonArrayBuilder;
import javax.json.JsonObjectBuilder;
import org.apache.log4j.Logger;
import Common.LoggerManager;
import Common.Util;
import Websocket.WorkQueue;
import common.FactoryDao;
import common.ORMUtil;
import dao.BrokerDao;
import dao.PingPongDao;
import entity.Broker;
import entity.PingPong;

public class BrokerUnit extends Broker {
	private Socket _socket;
	private Logger logger = LoggerManager.getLogger(BrokerUnit.class);
	private PingPong pingpong;

	public BrokerUnit(Socket _socket) {
		super(Util.createUID());
		this._socket = _socket;
		super.setIp(_socket.getRemoteSocketAddress().toString());
		super.setConnected(new Date());
		super.setActive(true);
		super.setCount(0);
		pingpong = new PingPong(super.getKey());
		pingpong.setIp(super.getIp());
	}

	public void ChangeStatusToEnd() {
		super.setDisconnected(new Date());
		super.setActive(false);
		FactoryDao.getDao(BrokerDao.class).merge(this);
	}

	public String receive() throws Exception {
		InputStream stream = _socket.getInputStream();
		byte[] buffer = new byte[4];
		stream.read(buffer);
		buffer = BitConverter.reverse(buffer);
		int size = BitConverter.toInt32(buffer, 0);
		buffer = new byte[size];
		stream.read(buffer);
		return new String(buffer, "UTF-8");
	}

	public void send(String data) throws IOException {
		byte[] buffer = data.getBytes("UTF-8");
		byte[] size = BitConverter.getBytes(buffer.length);
		size = BitConverter.reverse(size);
		OutputStream output = this._socket.getOutputStream();
		output.write(size);
		output.write(buffer);
	}

	public void ping() {
		try {
			this.setLastpingupdated(new Date());
			pingpong.setLastupdated(new Date());
			send("PING");
			WorkQueue.Async(() -> {
				FactoryDao.getDao(PingPongDao.class).merge(pingpong);
				FactoryDao.getDao(BrokerDao.class).merge(this);
			});
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public PingPong getPingpong() {
		return pingpong;
	}

	public String getPingpongTime() {
		return ORMUtil.DEFAULT_DAYTIME.format(pingpong.getLastupdated());
	}

	public String GetPingpoongTimeSimple() {
		return ORMUtil.DAYTIME_SIMPLE.format(pingpong.getLastupdated());
	}

	public void close() {
		try {
			this._socket.close();
		} catch (Throwable e) {
			logger.error(e);
		}
	}

	public String toJson() {
		return toJsonObject(this).build().toString();
	}

	public void modifyCount(int count) {
		synchronized (this) {
			count = super.getCount() + count;
			super.setCount(count);
			FactoryDao.getDao(BrokerDao.class).merge(this);
		}
	}

	private static JsonObjectBuilder toJsonObject(BrokerUnit unit) {
		JsonObjectBuilder builder = Json.createObjectBuilder();
		builder.add("key", unit.getKey());
		builder.add("ip", unit.getIp().substring(0, unit.getIp().indexOf(":")));
		builder.add("count", unit.getCount());
		builder.add("pingpong", unit.GetPingpoongTimeSimple());
		return builder;
	}

	public static String toJson(List<BrokerUnit> list) {
		JsonArrayBuilder builder = Json.createArrayBuilder();
		for (BrokerUnit node : list) {
			builder.add(toJsonObject(node));
		}
		return builder.build().toString();
	}
}
