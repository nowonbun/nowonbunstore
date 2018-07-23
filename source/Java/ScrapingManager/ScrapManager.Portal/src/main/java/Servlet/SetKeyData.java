package Servlet;

import javax.json.JsonObject;
import javax.servlet.annotation.WebServlet;
import Common.AbstractServlet;
import Common.JsonConverter;
import Common.Util;
import common.FactoryDao;
import dao.KeyDataDao;
import entity.KeyData;

@WebServlet("/SetKeyData")
public class SetKeyData extends AbstractServlet {
	private static final long serialVersionUID = 1L;

	@Override
	protected void doGet() {

		String bizno = getParameter("bizno");
		String name = getParameter("name");
		String ip = getParameter("ip");
		String callback = getParameter("callback");
		run(bizno, name, ip, callback);
	}

	@Override
	protected void doPost() {
		JsonObject json = JsonConverter.convertJsonObject(getPostParameters());
		String bizno = json.containsKey("bizno") ? json.getString("bizno") : null;
		String name = json.containsKey("name") ? json.getString("name") : null;
		String ip = json.containsKey("ip") ? json.getString("ip") : null;
		String callback = json.containsKey("callback") ? json.getString("callback") : null;
		run(bizno, name, ip, callback);
	}

	private void run(String bizno, String name, String ip, String callback) {
		if (Util.StringIsNullEmpty(bizno)) {
			setStatus(401);
			return;
		}
		if (Util.StringIsNullEmpty(name)) {
			setStatus(401);
			return;
		}
		if (Util.StringIsNullEmpty(ip)) {
			setStatus(401);
			return;
		}

		KeyData node = new KeyData(Util.createUID());
		node.setBizno(bizno);
		node.setName(name);
		node.setIp(ip);
		node.setCallback(callback);

		FactoryDao.getDao(KeyDataDao.class).merge(node);
		getPrinter().println("OK");
	}

}
