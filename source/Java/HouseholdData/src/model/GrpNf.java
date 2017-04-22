package model;

import java.io.Serializable;
import javax.persistence.*;


/**
 * The persistent class for the grp_nf database table.
 * 
 */
@Entity
@Table(name="grp_nf")
@NamedQuery(name="GrpNf.findAll", query="SELECT g FROM GrpNf g")
public class GrpNf implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private String grpd;

	private String cptn;

	private String grpnm;

	public GrpNf() {
	}

	public String getGrpd() {
		return this.grpd;
	}

	public void setGrpd(String grpd) {
		this.grpd = grpd;
	}

	public String getCptn() {
		return this.cptn;
	}

	public void setCptn(String cptn) {
		this.cptn = cptn;
	}

	public String getGrpnm() {
		return this.grpnm;
	}

	public void setGrpnm(String grpnm) {
		this.grpnm = grpnm;
	}

}