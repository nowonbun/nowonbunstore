package entity;

import java.util.Date;

import common.Column;
import common.Table;
import entity.common.EntityPType;

@Table(name = "broker")
public class Broker extends EntityPType {

	@Column(name = "ip")
	private String ip;
	@Column(name = "count")
	private int count;
	@Column(name = "active")
	private int active;
	@Column(name = "connected")
	private Date connected;
	@Column(name = "disconnected")
	private Date disconnected;
	@Column(name = "lastpingupdated")
	private Date lastpingupdated;

	public Broker(String key) {
		super(key);
	}

	public Broker(int pkey, String key) {
		super(pkey, key);
	}

	public void setIp(String ip) {
		this.ip = ip;
	}

	public String getIp() {
		return ip;
	}

	public int getCount() {
		return count;
	}

	public void setCount(int count) {
		this.count = count;
	}

	public boolean getActive() {
		return active == 1;
	}

	public void setActive(boolean active) {
		this.active = active ? 1 : 0;
	}

	public Date getConnected() {
		return connected;
	}

	public void setConnected(Date connected) {
		this.connected = connected;
	}

	public Date getDisconnected() {
		return disconnected;
	}

	public void setDisconnected(Date disconnected) {
		this.disconnected = disconnected;
	}

	public Date getLastpingupdated() {
		return lastpingupdated;
	}

	public void setLastpingupdated(Date lastpingupdated) {
		this.lastpingupdated = lastpingupdated;
	}

}
