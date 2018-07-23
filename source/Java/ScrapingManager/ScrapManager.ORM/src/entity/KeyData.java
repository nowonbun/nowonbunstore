package entity;

import common.Column;
import common.Table;
import entity.common.EntityPType;

@Table(name = "keydata")
public class KeyData extends EntityPType {

	@Column(name = "bizno")
	private String bizno;
	@Column(name = "name")
	private String name;
	@Column(name = "ip")
	private String ip;
	@Column(name = "callback")
	private String callback;

	public KeyData(String key) {
		super(key);
	}

	public KeyData(int pkey, String key) {
		super(pkey, key);
	}

	public String getBizno() {
		return bizno;
	}

	public void setBizno(String bizno) {
		this.bizno = bizno;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getIp() {
		return ip;
	}

	public void setIp(String ip) {
		this.ip = ip;
	}

	public String getCallback() {
		return callback;
	}

	public void setCallback(String callback) {
		this.callback = callback;
	}

}
