
package kr.pe.nowonbun.household2.common;
import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.NoResultException;
import javax.persistence.Query;
import javax.persistence.TypedQuery;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public abstract class HDao<T,K> {
	private static Logger logger = null;
	protected EntityManager em;
	public HDao(){	
		this.em = HService.get();
		logger = LoggerFactory.getLogger(setLogclass());
	}
	protected Logger logger(){
		return logger;
	}
	protected abstract Class<?> setLogclass();
	protected abstract Class<T> getEntityClass();
	public EntityManager getEntityManager() {
		return this.em;
	}
	public void TransactionBegin(){
		em.getTransaction().begin();
	}
	public void TransactionCommit(){
		em.getTransaction().commit();
	}
	public void TransactionRollBack(){
		em.getTransaction().rollback();
	}
	public String getEntityClassName(){
		String temp = getEntityClass().toString();
		String[] buffer = temp.split("[.]");
		return buffer[buffer.length-1];
	}
	
	//Basic
	@SuppressWarnings("unchecked")
	public T findByPk(K pk) {
		if (getFindActiveQueryName() == null) {
			return em.find(getEntityClass(), pk);
		}
		try {
			Query query = em.createNamedQuery(getFindActiveQueryName());
			query.setParameter("pk", pk);
			return (T) query.getSingleResult();
		} catch (NoResultException e) {
			return null;
		}
	}
	public List<T> findAll() {
		Query query = em.createNamedQuery(getFindAllQueryName());
		@SuppressWarnings("unchecked")
		List<T> resultList = query.getResultList();
		return resultList;
	}
	protected String getFindAllQueryName(){
		return getEntityClassName()+".findAll";
	}
	protected String getFindActiveQueryName(){
		return getEntityClassName()+".findActive";
	}
	public void update(T entity) {
		em.merge(entity);
		em.flush();
	}
	public void create(T entity) {
		em.persist(entity);
		em.flush();
	}
	public void delete(T entity) {
		em.remove(entity);
		em.flush();
	}
	public void deleteByPk(K pk) {
		T entity = em.getReference(getEntityClass(), pk);
		em.remove(entity);
		em.flush();
	}
	
	//custom
	public List<T> findList(Object[] params) throws Exception{
		try{
			TypedQuery<T> query = em.createQuery(getQueryString(params), getEntityClass());
			setQueryParams(query, params);
			
			return query.getResultList();
		}catch(Exception e){
			throw e;
		}
	}
	public T findOne(Object[] params) throws Exception{
		try{
			TypedQuery<T> query = em.createQuery(getQueryString(params), getEntityClass());
			setQueryParams(query, params);
			try {
				return query.getSingleResult();
			} catch (NoResultException e) {
				return null;
			}
		}catch(Exception e){
			throw e;
		}
	}
	protected abstract String getQueryString(Object[] params) throws Exception;
	protected abstract void setQueryParams(TypedQuery<T> query, Object[] params) throws Exception;
	
	public List<T> nativeFindList(Object[] params){
		Query query = em.createNativeQuery(getNativeQueryString(params), getEntityClass());
		setNativeQueryParams(query, params);
		@SuppressWarnings("unchecked")
		List<T> resultList = query.getResultList();
		return resultList;
	}
	public T nativeFindOne(Object[] params){
		Query query = em.createNativeQuery(getNativeQueryString(params), getEntityClass());
		setNativeQueryParams(query, params);
		try {
			@SuppressWarnings("unchecked")
			T result = (T) query.getSingleResult();
			return result;
		} catch (NoResultException e) {
			return null;
		}
	}
	protected abstract String getNativeQueryString(Object[] params);
	protected abstract void setNativeQueryParams(Query query, Object[] params);
	
	public int update(Object[] params){
		TypedQuery<T> query = em.createQuery(getUpdateQueryString(params), getEntityClass());
		setUpdateQueryParams(query, params);
		return query.executeUpdate();
	}
	protected abstract String getUpdateQueryString(Object[] params);
	protected void setUpdateQueryParams(TypedQuery<T> query, Object[] params) {
		return;
	}
	public int nativeUpdate(Object[] params){
		Query query = em.createNativeQuery(getNativeUpdateQueryString(params));
		setNativeUpdateQueryParams(query, params);
		return query.executeUpdate();
	}
	protected abstract String getNativeUpdateQueryString(Object[] params);
	protected abstract void setNativeUpdateQueryParams(Query query, Object[] params);
	public int delete(Object[] params){
		TypedQuery<T> query = em.createQuery(getDeleteQueryString(params), getEntityClass());
		setDeleteQueryParams(query, params);
		return query.executeUpdate();
	}
	protected abstract String getDeleteQueryString(Object[] params);
	protected abstract void setDeleteQueryParams(TypedQuery<T> query, Object[] params);
	public int nativeDelete(Object[] params){
		Query query = em.createNativeQuery(getNativeDeleteQueryString(params));
		setNativeDeleteQueryParams(query, params);
		return query.executeUpdate();
	}
	protected abstract String getNativeDeleteQueryString(Object[] params);
	protected abstract void setNativeDeleteQueryParams(Query query, Object[] params);
	
	public void insert(Object[] params){
		TypedQuery<T> query = em.createQuery(getInsertQueryString(params), getEntityClass());
		setInsertQueryParams(query, params);
		query.executeUpdate();
	}
	protected abstract String getInsertQueryString(Object[] params);
	protected abstract void setInsertQueryParams(TypedQuery<T> query, Object[] params);
	public void nativeInsert(Object[] params){
		Query query = em.createNativeQuery(getNativeInsertQueryString(params));
		setNativeInsertQueryParams(query, params);
		query.executeUpdate();
	}
	protected abstract String getNativeInsertQueryString(Object[] params);
	protected abstract void setNativeInsertQueryParams(Query query, Object[] params);
}
