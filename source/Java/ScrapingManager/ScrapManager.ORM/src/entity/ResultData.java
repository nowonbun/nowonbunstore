package entity;

import java.util.Date;

import common.Column;
import common.Table;
import entity.common.EntityMallType;

@Table(name = "ResultData")
public class ResultData extends EntityMallType {

	@Column(name = "resultcd")
	private String resultcd;
	@Column(name = "resultmsg")
	private String resultmsg;
	@Column(name = "starttime")
	private Date starttime;
	@Column(name = "endtime")
	private Date endtime;

	private String starttimeStr;
	private String endtimeStr;

	public ResultData(int mallkey, String key) {
		super(mallkey, key);
	}

	public String getResultcd() {
		return resultcd;
	}

	public void setResultcd(String resultcd) {
		this.resultcd = resultcd;
	}

	public String getResultmsg() {
		return resultmsg;
	}

	public void setResultmsg(String resultmsg) {
		this.resultmsg = resultmsg;
	}

	public Date getStarttime() {
		return starttime;
	}

	public void setStarttime(Date starttime) {
		this.starttime = starttime;
	}

	public Date getEndtime() {
		return endtime;
	}

	public void setEndtime(Date endtime) {
		this.endtime = endtime;
	}

	public String getStarttimeStr() {
		return starttimeStr;
	}

	public void setStarttimeStr(String starttimeStr) {
		this.starttimeStr = starttimeStr;
	}

	public String getEndtimeStr() {
		return endtimeStr;
	}

	public void setEndtimeStr(String endtimeStr) {
		this.endtimeStr = endtimeStr;
	}

}
