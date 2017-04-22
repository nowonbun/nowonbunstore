package model;

import java.io.Serializable;
import javax.persistence.*;


/**
 * The persistent class for the sys_dt database table.
 * 
 */
@Entity
@Table(name="sys_dt")
@NamedQuery(name="SysDt.findAll", query="SELECT s FROM SysDt s")
public class SysDt implements Serializable {
	private static final long serialVersionUID = 1L;

	@Id
	private String kycd;

	private String dt;

	public SysDt() {
	}

	public String getKycd() {
		return this.kycd;
	}

	public void setKycd(String kycd) {
		this.kycd = kycd;
	}

	public String getDt() {
		return this.dt;
	}

	public void setDt(String dt) {
		this.dt = dt;
	}

}