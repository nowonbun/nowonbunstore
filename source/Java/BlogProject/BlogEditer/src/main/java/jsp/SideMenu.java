package jsp;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import bean.CategoryBean;
import bean.SideMenuBean;
import common.FactoryDao;
import common.JspServlet;
import common.annotations.JspName;
import dao.CategoryDao;
import model.Category;

@JspName("sidemnue.jsp")
public class SideMenu extends JspServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected Object doJspMain() throws Exception {
		SideMenuBean bean = new SideMenuBean();
		List<Category> list = FactoryDao.getDao(CategoryDao.class).selectAll();
		List<CategoryBean> mainList = new ArrayList<>();
		for (Category m : list.stream().filter(x -> x.getParentCategory() == null).collect(Collectors.toList())) {
			CategoryBean sublist = new CategoryBean();
			mainList.add(sublist);
			sublist.setCategoryIdx(m.getIdx());
			sublist.setCategoryName(m.getCategoryName());
			sublist.setChild(new ArrayList<>());
			for (Category s : list.stream().filter(y -> y.getParentCategory() != null && y.getParentCategory().getIdx() == m.getIdx()).collect(Collectors.toList())) {
				CategoryBean subbean = new CategoryBean();
				subbean.setCategoryIdx(s.getIdx());
				subbean.setCategoryName(s.getCategoryName());
				sublist.getChild().add(subbean);
			}
		}
		bean.setSidemenu(mainList);
		return bean;
	}

	@Override
	protected void error(Throwable e) {

	}

}
