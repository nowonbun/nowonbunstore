package dao;

import java.util.List;

import model.SysDt;

public class SysDtDao extends Dao<SysDt>{
	public SysDtDao(){
		super(SysDt.class);
	}
	@SuppressWarnings("unchecked")
	public List<SysDt> findAll(){
		return ManagerDao.get().createNamedQuery("SysDt.findAll").getResultList();
	}
}
