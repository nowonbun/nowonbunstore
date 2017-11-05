package kr.pe.nowonbun.household2.common;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;

public class HService{
	
	private static EntityManager entityManager = null;
	
	public static EntityManager get(){
		if(entityManager == null){
			EntityManagerFactory emf = Persistence.createEntityManagerFactory("household2");
			entityManager = emf.createEntityManager();
		}
		return entityManager;
	}
	public static void transactionBegin(){
		entityManager.getTransaction().begin();
	}
	public static void transactionCommit(){
		entityManager.getTransaction().commit();
	}
	public static void transactionRollback(){
		entityManager.getTransaction().rollback();
	}
}
