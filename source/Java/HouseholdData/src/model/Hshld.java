package model;

import java.io.Serializable;
import javax.persistence.*;
import java.math.BigDecimal;
import java.util.Date;


/**
 * The persistent class for the hshld database table.
 * 
 */
@Entity
@NamedQuery(name="Hshld.findAll", query="SELECT h FROM Hshld h")
public class Hshld implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private int ndx;

	private String cntxt;

	@Temporal(TemporalType.DATE)
	private Date dt;

	@Temporal(TemporalType.TIMESTAMP)
	private Date pdt;

	private BigDecimal prc;

	//bi-directional many-to-one association to UsrNf
	@ManyToOne
	@JoinColumn(name="id")
	private UsrNf usrNf;

	//bi-directional many-to-one association to Tp
	@ManyToOne
	@JoinColumn(name="TP")
	private Tp tpBean;

	//bi-directional many-to-one association to Ctgry
	@ManyToOne
	@JoinColumn(name="CD")
	private Ctgry ctgry;

	public Hshld() {
	}

	public int getNdx() {
		return this.ndx;
	}

	public void setNdx(int ndx) {
		this.ndx = ndx;
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

	public UsrNf getUsrNf() {
		return this.usrNf;
	}

	public void setUsrNf(UsrNf usrNf) {
		this.usrNf = usrNf;
	}

	public Tp getTpBean() {
		return this.tpBean;
	}

	public void setTpBean(Tp tpBean) {
		this.tpBean = tpBean;
	}

	public Ctgry getCtgry() {
		return this.ctgry;
	}

	public void setCtgry(Ctgry ctgry) {
		this.ctgry = ctgry;
	}

}