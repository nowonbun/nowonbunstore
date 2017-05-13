package service;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.ManagerDao;
import dao.UsrNfDao;
import model.UsrNf;

@WebServlet("/CheckUser")
public class CheckUser extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute() {

		final String id = super.getParameter("GID");
		return ManagerDao.transaction(() -> {
			UsrNf entity = (UsrNf) usrNfDao.findOne(id);
			if (entity == null) {
				return false;
			}
			return entity;
		});
	}
}
