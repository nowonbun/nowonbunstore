package service;

import java.util.Calendar;
import java.util.Date;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.CtgryDao;
import dao.HshldDao;
import dao.ManagerDao;
import dao.UsrNfDao;

@WebServlet("/GetHouseholdList2")
public class GetHouseholdList2 extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldDao hshldDao;

	@ResourceDao
	private UsrNfDao usrNfDao;
	
	@ResourceDao
	private CtgryDao ctgryDao;

	public Object execute() {

		final String gid = super.getParameter("GID");
		final int year = Integer.parseInt(super.getParameter("YEAR"));
		final int month = Integer.parseInt(super.getParameter("MONTH"));
		final String category = super.getParameter("CATEGORY");

		return ManagerDao.transaction(() -> {
			Date start = createDate(year, month).getTime();
			Calendar temp = createDate(year, month);
			temp.add(Calendar.MONTH, 1);
			Date end = temp.getTime();
			return hshldDao.findList(usrNfDao.findOne(gid), start, end,ctgryDao.findOne(category));
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
