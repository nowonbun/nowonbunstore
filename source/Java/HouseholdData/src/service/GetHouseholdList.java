package service;

import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import dao.HshldDao;

@WebServlet("/GetHouseholdList")
public class GetHouseholdList extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldDao hshldDao;
	
	public Object execute(Map<String,String[]> parameter){
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("YEAR")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("MONTH")) {
			throw new HouseholdException(400);
		}
		final String id = parameter.get("GID")[0];
		final int year = Integer.parseInt(parameter.get("YEAR")[0]);
		final int month = Integer.parseInt(parameter.get("MONTH")[0]);
		
		return hshldDao.transaction(()->{
			return hshldDao.findList(id, year, month);
		});
	}
}
