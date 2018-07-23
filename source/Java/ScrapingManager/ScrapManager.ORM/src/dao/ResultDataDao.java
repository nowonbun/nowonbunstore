package dao;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import com.datastax.driver.core.Row;
import common.IDao;
import common.Service;
import entity.ResultData;

public class ResultDataDao extends IDao<ResultData> {

	protected ResultDataDao() {
		super();
	}

	@Override
	protected Class<ResultData> setClassType() {
		return ResultData.class;
	}

	public List<ResultData> selectByKey(int mallkey, String key) {
		return query((result) -> {
			try {
				List<Row> rowlist = result.all();
				List<ResultData> list = new ArrayList<>();
				for (Row row : rowlist) {
					ResultData entity = new ResultData(mallkey, key);
					entity.setResultcd(row.get("resultcd", String.class));
					entity.setResultmsg(row.get("resultmsg", String.class));
					entity.setStarttime(row.get("starttime", Date.class));
					entity.setEndtime(row.get("endtime", Date.class));
					list.add(entity);
				}
				return list;
			} catch (Throwable e) {
				throw new RuntimeException(e);
			}
		}, " SELECT * FROM " + Service.getKeyspace() + ".ResultData WHERE MALLKEY=? AND KEY=? ", mallkey, key);
	}
}
