package entity;

import java.util.Date;
import common.Column;
import common.ORMUtil;
import common.Table;
import entity.common.EntityMallType;

@Table(name = "RequestData")
public class RequestData extends EntityMallType {

	@Column(name = "apikey")
	private String apikey;
	@Column(name = "sdate")
	private String sdate;
	@Column(name = "edate")
	private String edate;
	@Column(name = "scraptype")
	private String scraptype;
	@Column(name = "exec")
	private String exec;
	@Column(name = "id1")
	private String id1;
	@Column(name = "id2")
	private String id2;
	@Column(name = "id3")
	private String id3;
	@Column(name = "pw1")
	private String pw1;
	@Column(name = "pw2")
	private String pw2;
	@Column(name = "pw3")
	private String pw3;
	@Column(name = "option1")
	private String option1;
	@Column(name = "option2")
	private String option2;
	@Column(name = "option3")
	private String option3;
	@Column(name = "createdDate")
	private Date createdDate;

	private String createDateStr;

	public RequestData(int mallkey, String key) {
		super(mallkey, key);
	}

	public String getApikey() {
		return apikey;
	}

	public void setApikey(String apikey) {
		this.apikey = apikey;
	}

	public String getSdate() {
		return sdate;
	}

	public void setSdate(String sdate) {
		this.sdate = sdate;
	}

	public String getEdate() {
		return edate;
	}

	public void setEdate(String edate) {
		this.edate = edate;
	}

	public String getScraptype() {
		return scraptype;
	}

	public void setScraptype(String scraptype) {
		this.scraptype = scraptype;
	}

	public String getExec() {
		return exec;
	}

	public void setExec(String exec) {
		this.exec = exec;
	}

	public String getId1() {
		return id1;
	}

	public void setId1(String id1) {
		this.id1 = id1;
	}

	public String getId2() {
		return id2;
	}

	public void setId2(String id2) {
		this.id2 = id2;
	}

	public String getId3() {
		return id3;
	}

	public void setId3(String id3) {
		this.id3 = id3;
	}

	public String getPw1() {
		return pw1;
	}

	public void setPw1(String pw1) {
		this.pw1 = pw1;
	}

	public String getPw2() {
		return pw2;
	}

	public void setPw2(String pw2) {
		this.pw2 = pw2;
	}

	public String getPw3() {
		return pw3;
	}

	public void setPw3(String pw3) {
		this.pw3 = pw3;
	}

	public String getOption1() {
		return option1;
	}

	public void setOption1(String option1) {
		this.option1 = option1;
	}

	public String getOption2() {
		return option2;
	}

	public void setOption2(String option2) {
		this.option2 = option2;
	}

	public String getOption3() {
		return option3;
	}

	public void setOption3(String option3) {
		this.option3 = option3;
	}

	public Date getCreatedDate() {
		ORMUtil.setTimeZone();
		return createdDate;
	}

	public void setCreatedDate(Date createdDate) {
		this.createdDate = createdDate;
	}

	public String getCreateDateStr() {
		return createDateStr;
	}

	public void setCreateDateStr(String createDateStr) {
		this.createDateStr = createDateStr;
	}

}
