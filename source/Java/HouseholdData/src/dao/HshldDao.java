package dao;

import java.util.List;

import javax.persistence.Query;

import model.Hshld;

public class HshldDao extends Dao<Hshld> {
	public HshldDao() {
		super(Hshld.class);
	}

	@SuppressWarnings("unchecked")
	public List<Hshld> findAll() {
		return super.getEntityManager().createNamedQuery("Hshld.findAll").getResultList();
	}

	@SuppressWarnings("unchecked")
	public List<Hshld> findList(String id, int year, int month) {
		String start = year + "-" + month + "-01";
		month++;
		if (month > 12) {
			month -= 12;
			year++;
		}
		String end = year + "-" + month + "-01";
		StringBuilder sb = new StringBuilder();
		sb.append(" select h.* from Hshld h ");
		sb.append(" where (h.id = ?1 or h.id in (select id from hshld_relation r where r.rid = ?1)) ");
		sb.append(" and h.dt >= ?2 and h.dt < ?3 ");
		Query qy = super.getEntityManager().createNativeQuery(sb.toString(),Hshld.class);
		qy.setParameter(1, id);
		qy.setParameter(2, start);
		qy.setParameter(3, end);
		
		return qy.getResultList();
	}
}

