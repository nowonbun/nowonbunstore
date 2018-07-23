package Servlet;

import javax.servlet.annotation.WebServlet;
import org.apache.log4j.Logger;
import Common.AjaxServlet;
import Common.LoggerManager;
import Common.Util;
import common.FactoryDao;
import dao.KeyNodeDao;
import dao.PackageDataDao;

@WebServlet("/GetPackageData")
public class GetPackageData extends AjaxServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(GetPackageData.class);

	@Override
	protected String doAjax() {
		String key = getParameter("KEY");
		logger.info("[WEB LOG] PARAMETER - " + key);
		if (Util.StringIsNullEmpty(key)) {
			setStatus(401);
			return "";
		}
		int mallkey = FactoryDao.getDao(KeyNodeDao.class).getMallkey(key);
		return getJsonData(FactoryDao.getDao(PackageDataDao.class).selectByKey(mallkey, key));
	}
}