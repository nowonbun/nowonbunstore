package Servlet;

import java.util.List;
import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import Common.Util;
import common.FactoryDao;
import dao.PingPongDao;
import entity.PingPong;

@WebServlet("/GetPingPong")
public class GetPingPong extends AjaxServlet {
	private static final long serialVersionUID = 1L;

	@Override
	protected String doAjax() {
		List<PingPong> list = FactoryDao.getDao(PingPongDao.class).select();
		for (PingPong node : list) {
			node.setLastupdatedStr(Util.ConvertToStringFromDate(node.getLastupdated()));
		}
		return getDataTableData(list);

	}
}
