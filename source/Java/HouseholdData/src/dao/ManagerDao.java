package dao;

import java.util.concurrent.Callable;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.EntityTransaction;
import javax.persistence.Persistence;
import javax.persistence.PersistenceContext;

public class ManagerDao {

	private static ManagerDao instance = null;
	@PersistenceContext
	private EntityManager entityManager;

	private ManagerDao() {
		EntityManagerFactory emf = Persistence.createEntityManagerFactory("HouseholdData");
		entityManager = emf.createEntityManager();
	}
	private static ManagerDao manager(){
		if (instance == null) {
			instance = new ManagerDao();
		}
		return instance;
	}
	public static EntityManager get() {
		return manager().entityManager;
	}

	public static <V> V  transaction(Callable<V> callable) {
		return transaction(callable, false);
	}

	public static <V> V  transaction(Callable<V> callable, boolean readonly) {
		EntityTransaction transaction = manager().entityManager.getTransaction();
		transaction.begin();
		try {
			V ret = callable.call();
			if (readonly) {
				transaction.rollback();
			} else {
				transaction.commit();
			}
			return ret;
		} catch (Throwable e) {
			transaction.rollback();
			throw new RuntimeException(e);
		}
	}

	public static void transaction(Runnable runnable) {
		transaction(runnable, false);
	}

	public static void transaction(Runnable runnable, boolean readonly) {
		EntityTransaction transaction = manager().entityManager.getTransaction();
		transaction.begin();
		try {
			runnable.run();
			if (readonly) {
				transaction.rollback();
			} else {
				transaction.commit();
			}
		} catch (Throwable e) {
			transaction.rollback();
			throw new RuntimeException(e);
		}
	}
}
