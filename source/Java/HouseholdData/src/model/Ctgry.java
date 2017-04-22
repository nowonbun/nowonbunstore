package model;

import java.io.Serializable;
import javax.persistence.*;
import java.util.List;


/**
 * The persistent class for the ctgry database table.
 * 
 */
@Entity
@NamedQuery(name="Ctgry.findAll", query="SELECT c FROM Ctgry c")
public class Ctgry implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private String cd;

	private String nm;

	//bi-directional many-to-one association to Hshld
	@OneToMany(mappedBy="ctgry")
	private List<Hshld> hshlds;

	//bi-directional many-to-one association to Tp
	@OneToMany(mappedBy="ctgry")
	private List<Tp> tps;

	public Ctgry() {
	}

	public String getCd() {
		return this.cd;
	}

	public void setCd(String cd) {
		this.cd = cd;
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
		hshld.setCtgry(this);

		return hshld;
	}

	public Hshld removeHshld(Hshld hshld) {
		getHshlds().remove(hshld);
		hshld.setCtgry(null);

		return hshld;
	}

	public List<Tp> getTps() {
		return this.tps;
	}

	public void setTps(List<Tp> tps) {
		this.tps = tps;
	}

	public Tp addTp(Tp tp) {
		getTps().add(tp);
		tp.setCtgry(this);

		return tp;
	}

	public Tp removeTp(Tp tp) {
		getTps().remove(tp);
		tp.setCtgry(null);

		return tp;
	}

}