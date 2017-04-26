package service;

import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
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

	public Object execute(Map<String, String[]> parameter) {
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("CD")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("TP")) {
			throw new HouseholdException(400);
		}
		final String gid = parameter.get("GID")[0];
		final String cd = parameter.get("CD")[0];
		final String tp = parameter.get("TP")[0];
		return ManagerDao.transaction(() -> {
			return hshldDao.sum(usrNfDao.findOne(gid), ctgryDao.findOne(cd), tpDao.findOne(tp));
		});
	}
}
