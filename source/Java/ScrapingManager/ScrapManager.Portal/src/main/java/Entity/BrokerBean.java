package Entity;

public class BrokerBean extends ObjectBean {
	private String key;
	private String ip;
	private int count;
	private boolean active;
	private String connected;
	private String disconnected;
	private String lastpingupdated;

	public String getKey() {
		return key;
	}

	public void setKey(String key) {
		this.key = key;
	}

	public String getIp() {
		return ip;
	}

	public void setIp(String ip) {
		this.ip = ip;
	}

	public int getCount() {
		return count;
	}

	public void setCount(int count) {
		this.count = count;
	}

	public boolean isActive() {
		return active;
	}

	public void setActive(boolean active) {
		this.active = active;
	}

	public String getConnected() {
		return connected;
	}

	public void setConnected(String connected) {
		this.connected = connected;
	}

	public String getDisconnected() {
		return disconnected;
	}

	public void setDisconnected(String disconnected) {
		this.disconnected = disconnected;
	}

	public String getLastpingupdated() {
		return lastpingupdated;
	}

	public void setLastpingupdated(String lastpingupdated) {
		this.lastpingupdated = lastpingupdated;
	}

}
