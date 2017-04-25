package service;

import java.util.Calendar;
import java.util.Map;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import common.Util;
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
		if (!parameter.containsKey("NAME")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("EMAIL")) {
			throw new HouseholdException(400);
		}
		final String id = parameter.get("GID")[0];
		final String name = parameter.get("NAME")[0];
		final String email = parameter.get("EMAIL")[0];
		UsrNf ret = usrNfDao.transaction(() -> {
			UsrNf entity = (UsrNf) usrNfDao.findOne(id);
			if (entity == null) {
				entity = new UsrNf();
				entity.setId(id);
				entity.setName(name);
				entity.setEmail(email);
				usrNfDao.create(entity);
			}
			boolean refresh = false;
			if (entity.getName() == null || !Util.StringEquals(name, entity.getName())) {
				refresh = true;
				entity.setName(name);
			}
			if (entity.getEmail() == null || !Util.StringEquals(email, entity.getEmail())) {
				refresh = true;
				entity.setEmail(email);
			}
			if (refresh) {
				entity.setCreatedate(Calendar.getInstance().getTime());
				usrNfDao.update(entity);
			}
			return entity;
		});
		return ret;
	}
}
