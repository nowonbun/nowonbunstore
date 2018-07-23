package entity;

import java.util.Date;
import common.Column;
import common.ORMUtil;
import common.Table;
import entity.common.EntityPType;

@Table(name = "PingPong")
public class PingPong extends EntityPType {

	@Column(name = "ip")
	private String ip;
	@Column(name = "lastupdated")
	private Date lastupdated;

	private String lastupdatedStr;

	public PingPong(String key) {
		super(key);
	}

	public PingPong(int pkey, String key) {
		super(pkey, key);
	}

	public void setIp(String ip) {
		this.ip = ip;
	}

	public String getIp() {
		return ip;
	}

	public Date getLastupdated() {
		ORMUtil.setTimeZone();
		return this.lastupdated;
	}

	public void setLastupdated(Date lastupdated) {
		this.lastupdated = lastupdated;
	}

	public String getLastupdatedStr() {
		return lastupdatedStr;
	}

	public void setLastupdatedStr(String lastupdatedStr) {
		this.lastupdatedStr = lastupdatedStr;
	}

}
