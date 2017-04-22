package model;

import java.io.Serializable;
import javax.persistence.*;
import java.util.Date;
import java.util.List;


/**
 * The persistent class for the usr_nf database table.
 * 
 */
@Entity
@Table(name="usr_nf")
@NamedQuery(name="UsrNf.findAll", query="SELECT u FROM UsrNf u")
public class UsrNf implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private String id;

	@Temporal(TemporalType.DATE)
	private Date createdate;

	private String email;

	private String name;

	//bi-directional many-to-one association to Hshld
	@OneToMany(mappedBy="usrNf")
	private List<Hshld> hshlds;

	//bi-directional many-to-one association to HshldRelation
	@OneToMany(mappedBy="usrNf1")
	private List<HshldRelation> hshldRelations1;

	//bi-directional many-to-one association to HshldRelation
	@OneToMany(mappedBy="usrNf2")
	private List<HshldRelation> hshldRelations2;

	//bi-directional many-to-one association to TknDmn
	@OneToMany(mappedBy="usrNf")
	private List<TknDmn> tknDmns;

	public UsrNf() {
	}

	public String getId() {
		return this.id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public Date getCreatedate() {
		return this.createdate;
	}

	public void setCreatedate(Date createdate) {
		this.createdate = createdate;
	}

	public String getEmail() {
		return this.email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getName() {
		return this.name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public List<Hshld> getHshlds() {
		return this.hshlds;
	}

	public void setHshlds(List<Hshld> hshlds) {
		this.hshlds = hshlds;
	}

	public Hshld addHshld(Hshld hshld) {
		getHshlds().add(hshld);
		hshld.setUsrNf(this);

		return hshld;
	}

	public Hshld removeHshld(Hshld hshld) {
		getHshlds().remove(hshld);
		hshld.setUsrNf(null);

		return hshld;
	}

	public List<HshldRelation> getHshldRelations1() {
		return this.hshldRelations1;
	}

	public void setHshldRelations1(List<HshldRelation> hshldRelations1) {
		this.hshldRelations1 = hshldRelations1;
	}

	public HshldRelation addHshldRelations1(HshldRelation hshldRelations1) {
		getHshldRelations1().add(hshldRelations1);
		hshldRelations1.setUsrNf1(this);

		return hshldRelations1;
	}

	public HshldRelation removeHshldRelations1(HshldRelation hshldRelations1) {
		getHshldRelations1().remove(hshldRelations1);
		hshldRelations1.setUsrNf1(null);

		return hshldRelations1;
	}

	public List<HshldRelation> getHshldRelations2() {
		return this.hshldRelations2;
	}

	public void setHshldRelations2(List<HshldRelation> hshldRelations2) {
		this.hshldRelations2 = hshldRelations2;
	}

	public HshldRelation addHshldRelations2(HshldRelation hshldRelations2) {
		getHshldRelations2().add(hshldRelations2);
		hshldRelations2.setUsrNf2(this);

		return hshldRelations2;
	}

	public HshldRelation removeHshldRelations2(HshldRelation hshldRelations2) {
		getHshldRelations2().remove(hshldRelations2);
		hshldRelations2.setUsrNf2(null);

		return hshldRelations2;
	}

	public List<TknDmn> getTknDmns() {
		return this.tknDmns;
	}

	public void setTknDmns(List<TknDmn> tknDmns) {
		this.tknDmns = tknDmns;
	}

	public TknDmn addTknDmn(TknDmn tknDmn) {
		getTknDmns().add(tknDmn);
		tknDmn.setUsrNf(this);

		return tknDmn;
	}

	public TknDmn removeTknDmn(TknDmn tknDmn) {
		getTknDmns().remove(tknDmn);
		tknDmn.setUsrNf(null);

		return tknDmn;
	}

}