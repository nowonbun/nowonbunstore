package service;

import java.math.BigDecimal;
import java.util.Calendar;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.ResourceDao;
import common.Util;
import dao.CtgryDao;
import dao.HshldDao;
import dao.ManagerDao;
import dao.TpDao;
import dao.UsrNfDao;
import model.Hshld;

@WebServlet("/AddHousehold")
public class AddHousehold extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@ResourceDao
	private HshldDao hshldDao;
	
	@ResourceDao
	private UsrNfDao usrNfDao;
	
	@ResourceDao
	private CtgryDao ctgryDao;
	
	@ResourceDao
	private TpDao tpDao;

	public Object execute(){
		final String gid = super.getParameter("GID");
		final String cd = super.getParameter("CD");
		final String tp = super.getParameter("TP");
		final String dt = super.getParameter("DT");
		final String cntxt = super.getParameter("CNTXT");
		final String prc = super.getParameter("PRC");

		return ManagerDao.transaction(() -> {
			Hshld entity = new Hshld();
			entity.setUsrNf(usrNfDao.findOne(gid));
			entity.setCtgry(ctgryDao.findOne(cd));
			entity.setTpBean(tpDao.findOne(tp));
			entity.setDt(Util.convertStringtoDate(dt));
			entity.setCntxt(cntxt);
			entity.setPrc(new BigDecimal(prc));
			entity.setPdt(Calendar.getInstance().getTime());
			hshldDao.create(entity);
			return entity;
		});
	}
}