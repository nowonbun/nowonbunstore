package ajax;

import javax.servlet.annotation.WebServlet;
import bean.CategoryBean;
import bean.ObjectBean;
import common.FactoryDao;
import common.IServlet;
import common.JsonConverter;
import common.Util;
import dao.CategoryDao;
import model.Category;

@WebServlet("/SetCategory")
public class SetCategory extends IServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected void doMain() throws Exception {
		String json = getStreamData();
		if (Util.StringIsEmptyOrNull(json)) {
			setStatus(403);
		}
		CategoryBean categoryBean = null;
		try {
			categoryBean = JsonConverter.parseObject(json, (obj) -> {
				CategoryBean ret = new CategoryBean();
				ret.setCategoryName(JsonConverter.JsonString(obj, "categoryName"));
				try {
					ret.setParent(Integer.parseInt(JsonConverter.JsonString(obj, "parent")));
				} catch (NumberFormatException e) {
					ret.setParent(0);
				}
				return ret;
			});
		} catch (Throwable e) {
			setStatus(403);
			return;
		}
		if (Util.StringIsEmptyOrNull(categoryBean.getCategoryName())) {
			setStatus(403);
			return;
		}

		ObjectBean bean = new ObjectBean();
		if (FactoryDao.getDao(CategoryDao.class).hasCategory(categoryBean.getCategoryName())) {
			bean.setData("exists");
			getPrinter().println(JsonConverter.create(bean));
			return;
		}
		Category category = new Category();
		category.setCategoryName(categoryBean.getCategoryName());
		if (categoryBean.getParent() != 0) {
			category.setParentCategory(FactoryDao.getDao(CategoryDao.class).getCategory(categoryBean.getParent()));
		}
		FactoryDao.getDao(CategoryDao.class).create(category);
		bean.setData("ok");
		getPrinter().println(JsonConverter.create(bean));
	}

	@Override
	protected void error(Throwable e) {

	}

}
