package Servlet;

import javax.servlet.annotation.WebServlet;
import org.apache.log4j.Logger;
import Common.AjaxServlet;
import Common.LoggerManager;
import Common.Util;
import common.FactoryDao;
import dao.KeyNodeDao;
import dao.RequestDataDao;

@WebServlet("/GetRequestData")
public class GetRequestData extends AjaxServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(GetRequestData.class);

	@Override
	protected String doAjax() {
		String key = getParameter("KEY");
		logger.info("[WEB LOG] PARAMETER - " + key);
		if (Util.StringIsNullEmpty(key)) {
			setStatus(401);
			return "";
		}
		int mallkey = FactoryDao.getDao(KeyNodeDao.class).getMallkey(key);
		return getJsonData(FactoryDao.getDao(RequestDataDao.class).selectByKey(mallkey, key));

	}
}
