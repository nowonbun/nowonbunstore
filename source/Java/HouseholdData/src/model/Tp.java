package model;

import java.io.Serializable;
import javax.persistence.*;
import java.util.List;


/**
 * The persistent class for the tp database table.
 * 
 */
@Entity
@NamedQuery(name="Tp.findAll", query="SELECT t FROM Tp t")
public class Tp implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private String tp;

	private String nm;

	//bi-directional many-to-one association to Hshld
	@OneToMany(mappedBy="tpBean")
	private List<Hshld> hshlds;

	//bi-directional many-to-one association to Ctgry
	@ManyToOne
	@JoinColumn(name="CD")
	private Ctgry ctgry;

	public Tp() {
	}

	public String getTp() {
		return this.tp;
	}

	public void setTp(String tp) {
		this.tp = tp;
	}

	public String getNm() {
		return this.nm;
	}

	public void setNm(String nm) {
		this.nm = nm;
	}

	public List<Hshld> getHshlds() {
		return this.hshlds;
	}

	public void setHshlds(List<Hshld> hshlds) {
		this.hshlds = hshlds;
	}

	public Hshld addHshld(Hshld hshld) {
		getHshlds().add(hshld);
		hshld.setTpBean(this);

		return hshld;
	}

	public Hshld removeHshld(Hshld hshld) {
		getHshlds().remove(hshld);
		hshld.setTpBean(null);

		return hshld;
	}

	public Ctgry getCtgry() {
		return this.ctgry;
	}

	public void setCtgry(Ctgry ctgry) {
		this.ctgry = ctgry;
	}

}