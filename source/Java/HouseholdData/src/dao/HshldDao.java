package dao;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

import javax.persistence.Query;

import model.Ctgry;
import model.Hshld;
import model.Tp;
import model.UsrNf;

public class HshldDao extends Dao<Hshld> {
	public HshldDao() {
		super(Hshld.class);
	}

	@SuppressWarnings("unchecked")
	public List<Hshld> findAll() {
		return ManagerDao.get().createNamedQuery("Hshld.findAll").getResultList();
	}

	@SuppressWarnings("unchecked")
	public List<Hshld> findList(UsrNf id, Date start, Date end) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select h from Hshld h ");
		sb.append(" where (h.usrNf = ?1 or h.usrNf in (select hr.usrNf1 from HshldRelation hr where hr.usrNf2 = ?1)) ");
		sb.append(" and h.dt >= ?2 and h.dt < ?3 ");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, id);
		qy.setParameter(2, start);
		qy.setParameter(3, end);

		return qy.getResultList();
	}
	
	@SuppressWarnings("unchecked")
	public List<Hshld> findList(UsrNf id, Date start, Date end,Ctgry ctgry) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select h from Hshld h ");
		sb.append(" where (h.usrNf = ?1 or h.usrNf in (select hr.usrNf1 from HshldRelation hr where hr.usrNf2 = ?1)) ");
		sb.append(" and h.dt >= ?2 and h.dt < ?3 ");
		sb.append(" and h.ctgry = ?4 ");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, id);
		qy.setParameter(2, start);
		qy.setParameter(3, end);
		qy.setParameter(4, ctgry);

		return qy.getResultList();
	}

	public Hshld findEntity(int idx, UsrNf id) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select h from Hshld h ");
		sb.append(
				" where h.ndx = ?1 and (h.usrNf = ?2 or h.usrNf in (select hr.usrNf1 from HshldRelation hr where hr.usrNf2 = ?2)) ");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, idx);
		qy.setParameter(2, id);

		return (Hshld) qy.getSingleResult();
	}

	public BigDecimal sum(UsrNf id, Ctgry ctgry, Tp tp) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select sum(h.prc) from Hshld h ");
		sb.append(" where (h.usrNf = ?1 or h.usrNf in (select hr.usrNf1 from HshldRelation hr where hr.usrNf2 = ?1)) ");
		sb.append(" and h.ctgry = ?2");
		sb.append(" and h.tpBean = ?3");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, id);
		qy.setParameter(2, ctgry);
		qy.setParameter(3, tp);
		
		return (BigDecimal)qy.getSingleResult();
	}
}
