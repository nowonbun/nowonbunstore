package dao;

import java.io.Serializable;

public abstract class Dao<T extends Serializable> {
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
		return ManagerDao.get().find(clazz, id);
	}

	public void create(T entity) {
		ManagerDao.get().persist(entity);
	}

	public T update(T entity) {
		return ManagerDao.get().merge(entity);
	}

	public void delete(T entity) {
		ManagerDao.get().remove(entity);
	}
}
