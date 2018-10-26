package dao;

import java.util.List;
import java.util.NoSuchElementException;
import java.util.stream.Collectors;

import javax.persistence.NoResultException;
import javax.persistence.Query;
import common.Dao;
import model.Category;

public class CategoryDao extends Dao<Category> {

	private static List<Category> flyweight = null;

	protected CategoryDao() {
		super(Category.class);
	}

	@SuppressWarnings("unchecked")
	public void reset() {
		flyweight = transaction((em) -> {
			try {
				Query query = em.createNamedQuery("Category.findAll", Category.class);
				return (List<Category>) query.getResultList();
			} catch (NoResultException e) {
				return null;
			}
		});
	}

	public List<Category> selectAll() {
		if (flyweight == null) {
			reset();
		}
		return flyweight;
	}

	public boolean hasCategory(String name) {
		return selectAll().stream().filter(x -> x.getCategoryName().equals(name)).count() > 0;
	}

	public List<Category> selectDefaultMenu() {
		return selectAll().stream().filter(x -> !x.getIshome() && !x.getIsdeleted()).sorted((x, y) -> Integer.compare(x.getSequence(), y.getSequence())).collect(Collectors.toList());
	}

	public Category getCategory(String categoryCode) {
		try {
			return selectAll().stream().filter(x -> x.getCategoryCode() == categoryCode).findFirst().get();
		} catch (NoSuchElementException e) {
			return null;
		}
	}
}
