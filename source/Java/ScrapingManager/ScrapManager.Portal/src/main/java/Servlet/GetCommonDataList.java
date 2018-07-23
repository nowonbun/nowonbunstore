package Servlet;

import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import common.FactoryDao;
import dao.CommonDataDao;

@WebServlet("/GetCommonDataList")
public class GetCommonDataList extends AjaxServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		return getDataTableData(FactoryDao.getDao(CommonDataDao.class).select());
	}
}
