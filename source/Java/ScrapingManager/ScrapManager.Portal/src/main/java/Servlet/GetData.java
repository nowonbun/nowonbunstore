package Servlet;

import java.util.ArrayList;
import java.util.List;
import javax.servlet.annotation.WebServlet;
import org.apache.log4j.Logger;
import Common.AjaxServlet;
import Common.JsonConverter;
import Common.LoggerManager;
import Common.Util;
import common.FactoryDao;
import dao.KeyNodeDao;
import entity.KeyNode;

@WebServlet("/GetData")
public class GetData extends AjaxServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(GetCommonData.class);

	@Override
	protected String doAjax() {
		String key = getParameter("KEY");
		logger.info("[WEB LOG] PARAMETER - " + key);
		if (Util.StringIsNullEmpty(key)) {
			setStatus(401);
			return "";
		}
		KeyNode node = FactoryDao.getDao(KeyNodeDao.class).getKeyNode(key);
		if (node.getChildrunkey() != null) {
			List<String> list = JsonConverter.parseArray(node.getChildrunkey(), (data) -> {
				List<String> ret = new ArrayList<>();
				for (int i = 0; i < data.size(); i++) {
					ret.add(data.getString(i));
				}
				return ret;
			});
			StringBuffer sb = new StringBuffer();
			for (String n : list) {
				sb.append(n);
				sb.append("\r\n");
			}
			return sb.toString();
		} else {
			return node.getKey();
		}
	}

}
