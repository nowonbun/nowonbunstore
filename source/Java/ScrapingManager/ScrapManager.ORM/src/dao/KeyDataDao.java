package dao;

import common.IDao;
import common.Service;
import entity.KeyData;
import entity.common.EntityPType;

public class KeyDataDao extends IDao<KeyData> {

	protected KeyDataDao() {
		super();
	}

	@Override
	protected Class<KeyData> setClassType() {
		return KeyData.class;
	}

	public boolean hasApiKey(String key) {
		return query((result) -> {
			return result.one() != null;
		}, " SELECT * FROM " + Service.getKeyspace() + ".KeyData WHERE PKEY=? AND KEY=? ", EntityPType.DEFAULT_P_KEY,
				key);
	}
	
	public String getCallback(String apikey) {
		return query((result) -> {
			return result.one().get(0, String.class);
		}, " SELECT CALLBACK FROM " + Service.getKeyspace() + ".KeyData WHERE PKEY=? AND KEY=? ",
				EntityPType.DEFAULT_P_KEY, apikey);
	}
}
