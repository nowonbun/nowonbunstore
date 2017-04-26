package service;

import java.util.Map;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import dao.ManagerDao;
import dao.UsrNfDao;
import model.UsrNf;

@WebServlet("/CheckUser")
public class CheckUser extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute(Map<String, String[]> parameter) {

		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		final String id = parameter.get("GID")[0];
		return ManagerDao.transaction(() -> {
			UsrNf entity = (UsrNf) usrNfDao.findOne(id);
			if (entity == null) {
				return false;
			}
			return entity;
		});
	}
}
