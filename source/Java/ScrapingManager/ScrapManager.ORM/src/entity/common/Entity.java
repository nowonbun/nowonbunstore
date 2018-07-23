package entity.common;

import common.Column;

public abstract class Entity {
	@Column(name = "key", key = true)
	private String key;

	protected Entity(String key) {
		this.key = key;
	}

	public String getKey() {
		return key;
	}
}
