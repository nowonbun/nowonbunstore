package service;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.HshldRelationDao;
import dao.ManagerDao;
import dao.UsrNfDao;
import model.HshldRelation;

@WebServlet("/SetRelationUser")
public class SetRelationUser extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldRelationDao hshldRelationDao;

	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute() {
		String gid = super.getParameter("GID");
		String rid = super.getParameter("RID");
		return ManagerDao.transaction(() -> {
			HshldRelation entity = hshldRelationDao.findByEntity(usrNfDao.findOne(gid), usrNfDao.findOne(rid));
			if (entity == null) {
				entity = new HshldRelation();
				entity.setUsrNf1(usrNfDao.findOne(gid));
				entity.setUsrNf2(usrNfDao.findOne(rid));
				hshldRelationDao.create(entity);
			}
			return entity;
		});
	}
}
