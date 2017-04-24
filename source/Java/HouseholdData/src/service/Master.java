package service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import dao.CtgryDao;
import dao.SysDtDao;
import dao.TpDao;
import model.Ctgry;

@WebServlet("/Master")
public class Master extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@ResourceDao
	private SysDtDao sysDtDao;
	
	@ResourceDao
	private CtgryDao ctgryDao;
	
	@ResourceDao
	private TpDao tpDao;

	public Object execute(Map<String,String[]> parameter){
		Map<String,Object> map = new HashMap<>();
		System.out.println("DEBUG");
		ctgryDao.Transaction(()->{
			List<Ctgry> a = ctgryDao.findAll();
			map.put("CATEGORY", ctgryDao.findAll());
		});
		tpDao.Transaction(()->{
			map.put("TP", tpDao.findAll());
		});
		sysDtDao.Transaction(()->{
			map.put("SYSTEMDATA", sysDtDao.findAll());
		});
		return map;
	}
}
