package service;

import java.io.IOException;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import common.AbstractHttpServlet;
import common.HouseholdException;
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

	public Object execute(Map<String, String[]> parameter) {
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		String gid = parameter.get("GID")[0];
		return ManagerDao.transaction(() -> {
			return hshldRelationDao.findByRid(usrNfDao.findOne(gid));
		});
	}
}
