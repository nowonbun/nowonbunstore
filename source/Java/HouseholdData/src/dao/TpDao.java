package dao;

import java.util.List;

import model.Tp;

public class TpDao extends Dao<Tp>{
	public TpDao(){
		super(Tp.class);
	}
	@SuppressWarnings("unchecked")
	public List<Tp> findAll(){
		return super.getEntityManager().createNamedQuery("Tp.findAll").getResultList();
	}
}
