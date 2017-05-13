package service;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
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

	public Object execute() {

		String gid = super.getParameter("GID");
		String idx = super.getParameter("IDX");

		return ManagerDao.transaction(() -> {
			return hshldDao.findEntity(Integer.parseInt(idx), usrNfDao.findOne(gid));
		});
	}
}
