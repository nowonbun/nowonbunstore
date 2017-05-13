package service;

import java.util.Calendar;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import common.Util;
import dao.ManagerDao;
import dao.UsrNfDao;
import model.UsrNf;

@WebServlet("/ApplyUser")
public class ApplyUser extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute() {
		final String id = super.getParameter("GID");
		final String name = super.getParameter("NAME");
		return ManagerDao.transaction(() -> {
			UsrNf entity = (UsrNf) usrNfDao.findOne(id);
			if (entity == null) {
				entity = new UsrNf();
				entity.setId(id);
				entity.setName(name);
				entity.setCreatedate(Calendar.getInstance().getTime());
				usrNfDao.create(entity);
			}
			boolean refresh = false;
			if (entity.getName() == null || !Util.StringEquals(name, entity.getName())) {
				refresh = true;
				entity.setName(name);
			}
			if (refresh) {
				//TODO:updateはいらない
				entity.setCreatedate(Calendar.getInstance().getTime());
				usrNfDao.update(entity);
			}
			return entity;
		});
	}
}
