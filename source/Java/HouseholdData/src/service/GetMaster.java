package service;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.CtgryDao;
import dao.ManagerDao;
import dao.SysDtDao;
import dao.TpDao;

@WebServlet("/GetMaster")
public class GetMaster extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private SysDtDao sysDtDao;

	@ResourceDao
	private CtgryDao ctgryDao;

	@ResourceDao
	private TpDao tpDao;

	public Object execute() {
		final Map<String, Object> map = new HashMap<>();
		ManagerDao.transaction(() -> {
			map.put("CATEGORY", ctgryDao.findAll());
		});
		ManagerDao.transaction(() -> {
			map.put("TP", tpDao.findAll());
		});
		ManagerDao.transaction(() -> {
			map.put("SYSTEMDATA", sysDtDao.findAll());
		});
		return map;
	}
}
