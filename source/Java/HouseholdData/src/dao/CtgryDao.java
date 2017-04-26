package dao;

import java.util.List;

import model.Ctgry;

public class CtgryDao extends Dao<Ctgry>{
	public CtgryDao() {
		super(Ctgry.class);
	}
	@SuppressWarnings("unchecked")
	public List<Ctgry> findAll(){
		return ManagerDao.get().createNamedQuery("Ctgry.findAll").getResultList();
	}
}
