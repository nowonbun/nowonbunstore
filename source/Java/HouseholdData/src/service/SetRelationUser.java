package service;

import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
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

	public Object execute(Map<String, String[]> parameter) {
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("RID")) {
			throw new HouseholdException(400);
		}
		String gid = parameter.get("GID")[0];
		String rid = parameter.get("RID")[0];
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
