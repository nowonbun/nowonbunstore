package service;

import java.math.BigDecimal;
import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import common.Util;
import dao.HshldDao;
import dao.HshldLogDao;
import dao.ManagerDao;
import dao.UsrNfDao;
import model.Hshld;
import model.HshldLog;

@WebServlet("/DeleteHousehold")
public class DeleteHousehold extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@ResourceDao
	private HshldDao hshldDao;
	
	@ResourceDao
	private HshldLogDao hshldLogDao;
	
	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute(Map<String,String[]> parameter){
		if (!parameter.containsKey("IDX")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}

		final String gid = parameter.get("GID")[0];
		final String idx = parameter.get("IDX")[0];

		return ManagerDao.transaction(() -> {
			Hshld entity = hshldDao.findEntity(Integer.parseInt(idx), usrNfDao.findOne(gid));
			if(entity != null){
				hshldLogDao.create(Util.trans(entity));
				hshldDao.delete(entity);
			}
			return true;
		});
	}
}
