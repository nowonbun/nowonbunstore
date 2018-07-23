package Servlet;

import java.util.List;

import javax.servlet.annotation.WebServlet;

import Common.AjaxServlet;
import Common.Util;
import common.FactoryDao;
import dao.KeyNodeDao;
import dao.RequestDataDao;
import dao.ResultDataDao;
import entity.RequestData;
import entity.ResultData;

@WebServlet("/GetScrapState")
public class GetScrapState extends AjaxServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		String key = getParameter("KEY");
		if (Util.StringIsNullEmpty(key)) {
			setStatus(401);
			return "";
		}
		int mallkey = FactoryDao.getDao(KeyNodeDao.class).getMallkey(key);
		
		List<RequestData> requests = FactoryDao.getDao(RequestDataDao.class).selectByKey(mallkey, key);
		List<ResultData> results = FactoryDao.getDao(ResultDataDao.class).selectByKey(mallkey, key);
		if (requests.size() > 0 && results.size() > 0) {
			return results.get(0).getResultcd();
		}
		return "";
	}

}
