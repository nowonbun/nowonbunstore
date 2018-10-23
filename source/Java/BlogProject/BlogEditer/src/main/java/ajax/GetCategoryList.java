package ajax;

import java.util.List;
import javax.servlet.annotation.WebServlet;
import common.FactoryDao;
import common.IServlet;
import dao.CategoryDao;
import model.Category;

@WebServlet("/GetCategoryList")
public class GetCategoryList extends IServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected void doMain() throws Exception {
		List<Category> list = FactoryDao.getDao(CategoryDao.class).selectAll();
		StringBuffer sb = new StringBuffer();
		sb.append("<select class=\"category-select\">");
		sb.append("<option value=\"\" disabled selected>Choose category...</option>");
		for (Category item : list) {
			sb.append("<option value=\"");
			sb.append(item.getIdx());
			sb.append("\" > ");
			sb.append(item.getCategoryName());
			sb.append("</option>");
		}
		getPrinter().write(sb.toString());
	}

	@Override
	protected void error(Throwable e) {

	}

}
