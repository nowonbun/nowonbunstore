package dao;

import java.util.List;

import common.IDao;
import common.Service;
import entity.KeyNode;
import entity.common.EntityPType;

public class KeyNodeDao extends IDao<KeyNode> {

	protected KeyNodeDao() {
		super();
	}

	@Override
	protected Class<KeyNode> setClassType() {
		return KeyNode.class;
	}

	public int getMallkey(String key) {
		return query((result) -> {
			return result.one().get(0, Integer.class);
		}, " SELECT mallkey FROM " + Service.getKeyspace() + ".KeyNode WHERE PKEY=? AND KEY=? ", EntityPType.DEFAULT_P_KEY, key);
	}

	public KeyNode getKeyNode(String key) {
		KeyNode where = new KeyNode(key);
		List<KeyNode> list = select(where);
		if (list != null && list.size() == 1) {
			return list.get(0);
		}
		return null;
	}
}
