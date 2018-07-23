package dao;

import java.util.ArrayList;
import java.util.List;
import com.datastax.driver.core.Row;
import common.IDao;
import common.Service;
import entity.PackageData;

public class PackageDataDao extends IDao<PackageData> {

	protected PackageDataDao() {
		super();
	}

	@Override
	protected Class<PackageData> setClassType() {
		return PackageData.class;
	}

	public List<PackageData> selectByKey(int mallkey, String key) {
		return query((result) -> {
			try {
				List<Row> rowlist = result.all();
				List<PackageData> list = new ArrayList<>();
				for (Row row : rowlist) {
					PackageData entity = new PackageData(mallkey, key, row.getInt("idx"), row.getInt("separation"));
					entity.setData(row.getString("data"));
					list.add(entity);
				}
				return list;
			} catch (Throwable e) {
				throw new RuntimeException(e);
			}
		}, " SELECT * FROM " + Service.getKeyspace() + ".Packagedata WHERE MALLKEY=? AND KEY=? ", mallkey, key);
	}
}
