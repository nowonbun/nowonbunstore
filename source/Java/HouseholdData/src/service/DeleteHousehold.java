package service;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import common.Util;
import dao.HshldDao;
import dao.HshldLogDao;
import dao.ManagerDao;
import dao.UsrNfDao;
import model.Hshld;

@WebServlet("/DeleteHousehold")
public class DeleteHousehold extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@ResourceDao
	private HshldDao hshldDao;
	
	@ResourceDao
	private HshldLogDao hshldLogDao;
	
	@ResourceDao
	private UsrNfDao usrNfDao;

	public Object execute(){

		final String gid = super.getParameter("GID");
		final String idx = super.getParameter("IDX");

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
