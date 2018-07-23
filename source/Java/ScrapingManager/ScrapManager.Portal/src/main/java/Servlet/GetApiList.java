package Servlet;

import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import common.FactoryDao;
import dao.KeyDataDao;

@WebServlet("/GetApiList")
public class GetApiList extends AjaxServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		return getDataTableData(FactoryDao.getDao(KeyDataDao.class).select());
	}

}
