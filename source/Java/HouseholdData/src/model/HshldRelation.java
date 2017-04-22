package model;

import java.io.Serializable;
import javax.persistence.*;


/**
 * The persistent class for the hshld_relation database table.
 * 
 */
@Entity
@Table(name="hshld_relation")
@NamedQuery(name="HshldRelation.findAll", query="SELECT h FROM HshldRelation h")
public class HshldRelation implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private int ndx;

	//bi-directional many-to-one association to UsrNf
	@ManyToOne
	@JoinColumn(name="id")
	private UsrNf usrNf1;

	//bi-directional many-to-one association to UsrNf
	@ManyToOne
	@JoinColumn(name="rid")
	private UsrNf usrNf2;

	public HshldRelation() {
	}

	public int getNdx() {
		return this.ndx;
	}

	public void setNdx(int ndx) {
		this.ndx = ndx;
	}

	public UsrNf getUsrNf1() {
		return this.usrNf1;
	}

	public void setUsrNf1(UsrNf usrNf1) {
		this.usrNf1 = usrNf1;
	}

	public UsrNf getUsrNf2() {
		return this.usrNf2;
	}

	public void setUsrNf2(UsrNf usrNf2) {
		this.usrNf2 = usrNf2;
	}

}