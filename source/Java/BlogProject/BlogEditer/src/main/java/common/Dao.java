package common;

import java.io.Serializable;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.EntityTransaction;
import javax.persistence.Persistence;

import common.interfaces.EntityManagerCallable;
import common.interfaces.EntityManagerRunable;

public abstract class Dao<T extends Serializable> {

	private static EntityManagerFactory emf = Persistence.createEntityManagerFactory("blog");
	private Class<T> clazz;

	protected Dao(Class<T> clazz) {
		this.clazz = clazz;
	}

	protected final void setClass(Class<T> clazz) {
		this.clazz = clazz;
	}

	protected final Class<T> getClazz() {
		return clazz;
	}

	public T findOne(Object id) {
		return transaction((em) -> {
			return em.find(clazz, id);
		});
	}

	public void create(T entity) {
		transaction((em) -> {
			em.persist(entity);
			// Manager.get().flush();
		});
	}

	public T update(T entity) {
		return transaction((em) -> {
			T ret = em.merge(entity);
			// Manager.get().flush();
			return ret;
		});
	}

	public void delete(T entity) {
		transaction((em) -> {
			em.remove(entity);
			// Manager.get().flush();
		});
	}

	protected <V> V transaction(EntityManagerCallable<V> callable) {
		return transaction(callable, false);
	}

	protected <V> V transaction(EntityManagerCallable<V> callable, boolean readonly) {
		EntityManager em = emf.createEntityManager();
		EntityTransaction transaction = em.getTransaction();
		transaction.begin();
		try {
			V ret = callable.run(em);
			if (readonly) {
				transaction.rollback();
			} else {
				transaction.commit();
			}
			return ret;
		} catch (Throwable e) {
			if (transaction.isActive()) {
				transaction.rollback();
			}
			throw new RuntimeException(e);
		} finally {
			em.clear();
			em.close();
			// emf.close();
		}
	}

	protected void transaction(EntityManagerRunable runnable) {
		transaction(runnable, false);
	}

	protected void transaction(EntityManagerRunable runnable, boolean readonly) {
		EntityManager em = emf.createEntityManager();
		EntityTransaction transaction = em.getTransaction();
		transaction.begin();
		try {
			runnable.run(em);
			if (readonly) {
				transaction.rollback();
			} else {
				transaction.commit();
			}
		} catch (Throwable e) {
			if (transaction.isActive()) {
				transaction.rollback();
			}
			throw new RuntimeException(e);
		} finally {
			em.clear();
			em.close();
		}
	}
}
