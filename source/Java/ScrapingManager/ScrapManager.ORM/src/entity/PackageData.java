package entity;

import common.Column;
import common.Table;
import entity.common.EntityMallType;

@Table(name = "PackageData")
public class PackageData extends EntityMallType {

	@Column(name = "idx", key = true)
	private int idx;
	@Column(name = "separation", key = true)
	private int separation;
	@Column(name = "data")
	private String data;

	public PackageData(int mallkey, String key, int idx, int separation) {
		super(mallkey, key);
		this.idx = idx;
		this.separation = separation;
	}

	public int getIdx() {
		return idx;
	}

	public int getSeparation() {
		return separation;
	}

	public String getData() {
		return data;
	}

	public void setData(String data) {
		this.data = data;
	}

}
