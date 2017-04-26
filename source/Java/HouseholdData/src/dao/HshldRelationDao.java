package dao;

import java.util.List;

import javax.persistence.Query;

import model.Hshld;
import model.HshldRelation;
import model.UsrNf;

public class HshldRelationDao extends Dao<HshldRelation> {
	public HshldRelationDao() {
		super(HshldRelation.class);
	}

	@SuppressWarnings("unchecked")
	public List<HshldRelation> findAll() {
		return ManagerDao.get().createNamedQuery("HshldRelation.findAll").getResultList();
	}

	@SuppressWarnings("unchecked")
	public List<HshldRelation> findById(UsrNf id) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select hr from HshldRelation hr ");
		sb.append(" where hr.usrNf1 = ?1 ");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, id);

		return (List<HshldRelation>) qy.getResultList();
	}

	@SuppressWarnings("unchecked")
	public List<HshldRelation> findByRid(UsrNf rid) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select hr from HshldRelation hr ");
		sb.append(" where hr.usrNf2 = ?1 ");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, rid);

		return (List<HshldRelation>) qy.getResultList();
	}

	public HshldRelation findByEntity(UsrNf gid, UsrNf rid) {
		StringBuilder sb = new StringBuilder();
		sb.append(" select hr from HshldRelation hr ");
		sb.append(" where hr.usrNf1 = ?1 and hr.usrNf2 = ?2");
		Query qy = ManagerDao.get().createQuery(sb.toString(), Hshld.class);
		qy.setParameter(1, gid);
		qy.setParameter(2, rid);

		return (HshldRelation) qy.getResultList();
	}
}
