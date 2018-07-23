package Servlet;

import java.util.List;
import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import Common.Util;
import common.FactoryDao;
import dao.ResultDataDao;
import entity.ResultData;

@WebServlet("/GetResultDataList")
public class GetResultDataList extends AjaxServlet {
	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		List<ResultData> list = FactoryDao.getDao(ResultDataDao.class).select();
		for (ResultData data : list) {
			data.setStarttimeStr(Util.ConvertToStringFromDate(data.getStarttime()));
			data.setEndtimeStr(Util.ConvertToStringFromDate(data.getEndtime()));
		}
		return getDataTableData(list);
	}
}
