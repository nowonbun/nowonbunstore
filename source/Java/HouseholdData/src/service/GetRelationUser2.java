package service;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.HshldRelationDao;
import dao.ManagerDao;
import dao.UsrNfDao;

@WebServlet("/GetRelationUser2")
public class GetRelationUser2 extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldRelationDao hshldRelationDao;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute() {
		String gid = super.getParameter("GID");
		return ManagerDao.transaction(() -> {
			return hshldRelationDao.findByRid(usrNfDao.findOne(gid));
		});
	}
}
