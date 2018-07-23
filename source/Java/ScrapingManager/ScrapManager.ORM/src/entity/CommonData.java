package entity;

import common.Column;
import common.Table;
import entity.common.EntityMallType;

@Table(name = "CommonData")
public class CommonData extends EntityMallType {

	@Column(name = "idx", key = true)
	private int idx;
	@Column(name = "data")
	private String data;

	public CommonData(int mallkey, String key, int idx) {
		super(mallkey, key);
		this.idx = idx;
	}

	public int getIdx() {
		return idx;
	}

	public String getData() {
		return data;
	}

	public void setData(String data) {
		this.data = data;
	}
}
