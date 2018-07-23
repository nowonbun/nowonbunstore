package Servlet;

import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import common.FactoryDao;
import dao.PackageDataDao;

@WebServlet("/GetPackageDataList")
public class GetPackageDataList extends AjaxServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		return getDataTableData(FactoryDao.getDao(PackageDataDao.class).select());

	}
}
