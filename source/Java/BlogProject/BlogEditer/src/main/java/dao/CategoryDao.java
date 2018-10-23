package dao;

import java.util.List;
import java.util.NoSuchElementException;

import javax.persistence.NoResultException;
import javax.persistence.Query;
import common.Dao;
import model.Category;

public class CategoryDao extends Dao<Category> {

	protected CategoryDao() {
		super(Category.class);
	}

	@SuppressWarnings("unchecked")
	public List<Category> selectAll() {
		return transaction((em) -> {
			try {
				Query query = em.createNamedQuery("Category.findAll", Category.class);
				return (List<Category>) query.getResultList();
			} catch (NoResultException e) {
				return null;
			}
		});
	}

	public boolean hasCategory(String name) {
		return selectAll().stream().filter(x -> x.getCategoryName().equals(name)).count() > 0;
	}

	public Category getCategory(int idx) {
		try {
			return selectAll().stream().filter(x -> x.getIdx() == idx).findFirst().get();
		} catch (NoSuchElementException e) {
			return null;
		}
	}
}
