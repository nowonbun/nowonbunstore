package service;

import java.util.Calendar;
import java.util.Date;
import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import dao.HshldDao;
import dao.ManagerDao;
import dao.UsrNfDao;

@WebServlet("/GetHouseholdList")
public class GetHouseholdList extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldDao hshldDao;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute(Map<String, String[]> parameter) {
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("YEAR")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("MONTH")) {
			throw new HouseholdException(400);
		}
		final String gid = parameter.get("GID")[0];
		final int year = Integer.parseInt(parameter.get("YEAR")[0]);
		final int month = Integer.parseInt(parameter.get("MONTH")[0]);

		return ManagerDao.transaction(() -> {
			Date start = createDate(year, month).getTime();
			Calendar temp = createDate(year, month);
			temp.add(Calendar.MONTH, 1);
			Date end = temp.getTime();
			return hshldDao.findList(usrNfDao.findOne(gid), start, end);
		});
	}

	private Calendar createDate(int year, int month) {
		Calendar c = Calendar.getInstance();
		c.clear();
		c.set(Calendar.YEAR, year);
		c.set(Calendar.MONTH, month - 1);
		c.set(Calendar.DATE, 1);
		return c;
	}
}
