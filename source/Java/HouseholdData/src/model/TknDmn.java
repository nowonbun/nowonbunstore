package model;

import java.io.Serializable;
import javax.persistence.*;
import java.util.Date;


/**
 * The persistent class for the tkn_dmn database table.
 * 
 */
@Entity
@Table(name="tkn_dmn")
@NamedQuery(name="TknDmn.findAll", query="SELECT t FROM TknDmn t")
public class TknDmn implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private int ndx;

	@Column(name="access_token")
	private String accessToken;

	@Temporal(TemporalType.DATE)
	@Column(name="apply_date")
	private Date applyDate;

	//bi-directional many-to-one association to UsrNf
	@ManyToOne
	@JoinColumn(name="id")
	private UsrNf usrNf;

	public TknDmn() {
	}

	public int getNdx() {
		return this.ndx;
	}

	public void setNdx(int ndx) {
		this.ndx = ndx;
	}

	public String getAccessToken() {
		return this.accessToken;
	}

	public void setAccessToken(String accessToken) {
		this.accessToken = accessToken;
	}

	public Date getApplyDate() {
		return this.applyDate;
	}

	public void setApplyDate(Date applyDate) {
		this.applyDate = applyDate;
	}

	public UsrNf getUsrNf() {
		return this.usrNf;
	}

	public void setUsrNf(UsrNf usrNf) {
		this.usrNf = usrNf;
	}

}