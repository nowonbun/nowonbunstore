package dao;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import com.datastax.driver.core.Row;
import common.IDao;
import common.Service;
import entity.RequestData;

public class RequestDataDao extends IDao<RequestData> {

	protected RequestDataDao() {
		super();
	}

	@Override
	protected Class<RequestData> setClassType() {
		return RequestData.class;
	}

	public List<RequestData> selectByKey(int mallkey, String key) {
		return query((result) -> {
			try {
				List<Row> rowlist = result.all();
				List<RequestData> list = new ArrayList<>();
				for (Row row : rowlist) {
					RequestData entity = new RequestData(mallkey, key);
					entity.setSdate(row.getString("sdate"));
					entity.setEdate(row.getString("edate"));
					entity.setScraptype(row.getString("scraptype"));
					entity.setExec(row.getString("exec"));
					entity.setId1(row.getString("id1"));
					entity.setId2(row.getString("id2"));
					entity.setId3(row.getString("id3"));
					entity.setOption1(row.getString("option1"));
					entity.setOption2(row.getString("option2"));
					entity.setOption3(row.getString("option3"));
					entity.setCreatedDate(row.get("createdDate", Date.class));
					list.add(entity);
				}
				return list;
			} catch (Throwable e) {
				throw new RuntimeException(e);
			}
		}, " SELECT * FROM " + Service.getKeyspace() + ".RequestData WHERE MALLKEY=? AND KEY=? ", mallkey, key);
	}
	
	public String getApikey(int mallkey, String key) {
		return query((result) -> {
			return result.one().get(0, String.class);
		}, " SELECT APIKEY FROM " + Service.getKeyspace() + ".RequestData WHERE MALLKEY=? AND KEY=? ",
				mallkey, key);
	}
}
