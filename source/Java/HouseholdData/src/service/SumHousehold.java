package service;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.CtgryDao;
import dao.HshldDao;
import dao.ManagerDao;
import dao.TpDao;
import dao.UsrNfDao;

@WebServlet("/SumHousehold")
public class SumHousehold extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldDao hshldDao;

	@ResourceDao
	private UsrNfDao usrNfDao;

	@ResourceDao
	private CtgryDao ctgryDao;

	@ResourceDao
	private TpDao tpDao;

	public Object execute() {

		final String gid = super.getParameter("GID");
		final String cd = super.getParameter("CD");
		final String tp = super.getParameter("TP");
		return ManagerDao.transaction(() -> {
			return hshldDao.sum(usrNfDao.findOne(gid), ctgryDao.findOne(cd), tpDao.findOne(tp));
		});
	}
}
