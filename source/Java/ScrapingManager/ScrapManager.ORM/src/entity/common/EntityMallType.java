package entity.common;

import common.Column;

public abstract class EntityMallType extends Entity {
	@Column(name = "mallkey", partitionkey = true)
	private int mallkey;

	protected EntityMallType(int mallkey, String key) {
		super(key);
		this.mallkey = mallkey;
	}

	public int getMallKey() {
		return mallkey;
	}
}
