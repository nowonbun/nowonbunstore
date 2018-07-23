package entity;

import common.Column;
import common.Table;
import entity.common.EntityPType;

@Table(name = "keynode")
public class KeyNode extends EntityPType {

	@Column(name = "mallkey")
	private int mallkey;

	@Column(name = "childrunkey")
	private String childrunkey;

	public KeyNode(String key) {
		super(key);
	}

	public KeyNode(int pkey, String key) {
		super(pkey, key);
	}

	public int getMallkey() {
		return mallkey;
	}

	public void setMallkey(int mallkey) {
		this.mallkey = mallkey;
	}

	public String getChildrunkey() {
		return childrunkey;
	}

	public void setChildrunkey(String childrunkey) {
		this.childrunkey = childrunkey;
	}

}
