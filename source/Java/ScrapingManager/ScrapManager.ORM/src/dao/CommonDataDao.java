package dao;

import java.util.ArrayList;
import java.util.List;
import com.datastax.driver.core.Row;
import common.IDao;
import common.Service;
import entity.CommonData;

public class CommonDataDao extends IDao<CommonData> {

	protected CommonDataDao() {
		super();
	}

	@Override
	protected Class<CommonData> setClassType() {
		return CommonData.class;
	}

	public List<CommonData> selectByKey(int mallkey, String key) {
		return query((result) -> {
			try {
				List<Row> rowlist = result.all();
				List<CommonData> list = new ArrayList<>();
				for (Row row : rowlist) {
					CommonData entity = new CommonData(mallkey, key, row.getInt("idx"));
					entity.setData(row.getString("data"));
					list.add(entity);
				}
				return list;
			} catch (Throwable e) {
				throw new RuntimeException(e);
			}
		}, " SELECT * FROM " + Service.getKeyspace() + ".CommonData WHERE MALLKEY=? AND KEY=? ", mallkey, key);
	}
}
