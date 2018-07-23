package Servlet;

import java.util.List;
import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import Common.Util;
import common.FactoryDao;
import dao.RequestDataDao;
import entity.RequestData;

@WebServlet("/GetRequestDataList")
public class GetRequestDataList extends AjaxServlet {
	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		List<RequestData> list = FactoryDao.getDao(RequestDataDao.class).select();
		for (RequestData data : list) {
			data.setCreateDateStr(Util.ConvertToStringFromDate(data.getCreatedDate()));
		}
		return getDataTableData(list);
	}
}
