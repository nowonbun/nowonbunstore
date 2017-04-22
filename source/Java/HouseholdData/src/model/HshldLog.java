package model;

import java.io.Serializable;
import javax.persistence.*;
import java.math.BigDecimal;
import java.util.Date;


/**
 * The persistent class for the hshld_log database table.
 * 
 */
@Entity
@Table(name="hshld_log")
@NamedQuery(name="HshldLog.findAll", query="SELECT h FROM HshldLog h")
public class HshldLog implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private int ndx;

	private String cd;

	private String cntxt;

	@Temporal(TemporalType.DATE)
	private Date dt;

	private String id;

	private int ndx2;

	@Temporal(TemporalType.TIMESTAMP)
	private Date pdt;

	private BigDecimal prc;

	private String tp;

	public HshldLog() {
	}

	public int getNdx() {
		return this.ndx;
	}

	public void setNdx(int ndx) {
		this.ndx = ndx;
	}

	public String getCd() {
		return this.cd;
	}

	public void setCd(String cd) {
		this.cd = cd;
	}

	public String getCntxt() {
		return this.cntxt;
	}

	public void setCntxt(String cntxt) {
		this.cntxt = cntxt;
	}

	public Date getDt() {
		return this.dt;
	}

	public void setDt(Date dt) {
		this.dt = dt;
	}

	public String getId() {
		return this.id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public int getNdx2() {
		return this.ndx2;
	}

	public void setNdx2(int ndx2) {
		this.ndx2 = ndx2;
	}

	public Date getPdt() {
		return this.pdt;
	}

	public void setPdt(Date pdt) {
		this.pdt = pdt;
	}

	public BigDecimal getPrc() {
		return this.prc;
	}

	public void setPrc(BigDecimal prc) {
		this.prc = prc;
	}

	public String getTp() {
		return this.tp;
	}

	public void setTp(String tp) {
		this.tp = tp;
	}

}