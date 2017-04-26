package service;

import java.math.BigDecimal;
import java.util.Map;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import common.HouseholdException;
import common.ResourceDao;
import common.Util;
import dao.CtgryDao;
import dao.HshldDao;
import dao.HshldLogDao;
import dao.ManagerDao;
import dao.TpDao;
import dao.UsrNfDao;
import model.Hshld;

@WebServlet("/ModifyHousehold")
public class ModifyHousehold extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@ResourceDao
	private HshldDao hshldDao;
	
	@ResourceDao
	private HshldLogDao hshldLogDao;
	
	@ResourceDao
	private UsrNfDao usrNfDao;
	
	@ResourceDao
	private CtgryDao ctgryDao;
	
	@ResourceDao
	private TpDao tpDao;
	
	public Object execute(Map<String,String[]> parameter){
		if (!parameter.containsKey("IDX")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("GID")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("CD")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("TP")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("DT")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("CNTXT")) {
			throw new HouseholdException(400);
		}
		if (!parameter.containsKey("PRC")) {
			throw new HouseholdException(400);
		}
		String idx = parameter.get("IDX")[0];
		final String gid = parameter.get("GID")[0];
		final String cd = parameter.get("CD")[0];
		final String tp = parameter.get("TP")[0];
		final String dt = parameter.get("DT")[0];
		final String cntxt = parameter.get("CNTXT")[0];
		final String prc = parameter.get("PRC")[0];

		return ManagerDao.transaction(() -> {
			Hshld entity = hshldDao.findEntity(Integer.parseInt(idx), usrNfDao.findOne(gid));
			if(entity != null){
				hshldLogDao.create(Util.trans(entity));
				entity.setUsrNf(usrNfDao.findOne(gid));
				entity.setCtgry(ctgryDao.findOne(cd));
				entity.setTpBean(tpDao.findOne(tp));
				entity.setDt(Util.convertStringtoDate(dt));
				entity.setCntxt(cntxt);
				entity.setPrc(new BigDecimal(prc));
				hshldDao.update(entity);
			}
			return entity;
		});
	}
}
