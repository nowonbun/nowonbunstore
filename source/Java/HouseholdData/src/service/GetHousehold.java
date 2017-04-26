package service;

import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import dao.HshldDao;
import dao.ManagerDao;
import dao.UsrNfDao;

@WebServlet("/GetHousehold")
public class GetHousehold extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldDao hshldDao;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute(Map<String, String[]> parameter) {
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("IDX")) {
			throw new HouseholdException(400);
		}
		String gid = parameter.get("GID")[0];
		String idx = parameter.get("IDX")[0];

		return ManagerDao.transaction(() -> {
			return hshldDao.findEntity(Integer.parseInt(idx), usrNfDao.findOne(gid));
		});
	}
}
