package jsp;

import java.util.ArrayList;

import bean.CategoryBean;
import bean.SideMenuBean;
import common.FactoryDao;
import common.JspServlet;
import common.PropertyMap;
import common.annotations.JspName;
import dao.CategoryDao;
import model.Category;

@JspName("menu.jsp")
public class Menu extends JspServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected Object doJspMain() throws Exception {
		SideMenuBean bean = new SideMenuBean();
		bean.setSidemenu(new ArrayList<>());
		java.util.List<Category> list = FactoryDao.getDao(CategoryDao.class).selectAll();
		list.sort((x, y) -> Integer.compare(x.getSequence(), y.getSequence()));
		for (Category m : list) {
			if (m.getIsdeleted()) {
				continue;
			}
			CategoryBean item = new CategoryBean();
			bean.getSidemenu().add(item);
			item.setCategoryHref(PropertyMap.getInstance().getProperty("config", "server_root") + m.getUrl().replace("html", "jsp"));
			item.setCategoryText(m.getCategoryName());
		}
		return bean;
	}

	@Override
	protected void error(Throwable e) {

	}

}
