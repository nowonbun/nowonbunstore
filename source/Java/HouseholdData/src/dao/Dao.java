package dao;

import java.io.Serializable;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.EntityTransaction;
import javax.persistence.Persistence;
import javax.persistence.PersistenceContext;

public abstract class Dao<T extends Serializable> {
	private Class<T> clazz;

	@PersistenceContext
	private EntityManager entityManager;

	protected Dao(Class<T> clazz) {
		EntityManagerFactory emf = Persistence.createEntityManagerFactory("HouseholdData");
		entityManager = emf.createEntityManager();
		this.clazz = clazz;
	}

	protected final void setClass(Class<T> clazz) {
		this.clazz = clazz;
	}

	protected final Class<T> getClazz() {
		return clazz;
	}

	public T findOne(Object id) {
		return entityManager.find(clazz, id);
	}

	public void create(T entity) {
		entityManager.persist(entity);
	}

	public T update(T entity) {
		return entityManager.merge(entity);
	}
	
	public void delete(T entity){
		entityManager.remove(entity);
	}
	
	protected EntityManager getEntityManager(){
		return this.entityManager;
	}
	
	public void Transaction(Runnable runnable){
		EntityTransaction transaction = this.entityManager.getTransaction();
		transaction.begin();
		try{
			runnable.run();
			transaction.commit();
		}catch(Throwable e){
			transaction.rollback();
			throw new RuntimeException(e);
		}
	}
	public void Transaction(Runnable runnable,boolean readonly){
		EntityTransaction transaction = this.entityManager.getTransaction();
		transaction.begin();
		try{
			runnable.run();
			if(readonly){
				transaction.rollback();
			}else{
				transaction.commit();	
			}
		}catch(Throwable e){
			transaction.rollback();
			throw new RuntimeException(e);
		}
	}
}
