package Servlet;

import java.util.ArrayList;
import java.util.List;

import javax.servlet.annotation.WebServlet;
import org.apache.log4j.Logger;
import Common.AbstractServlet;
import Common.JsonConverter;
import Common.LoggerManager;
import Common.Util;
import Entity.Parameter;
import Websocket.WorkQueue;
import common.FactoryDao;
import dao.RequestDataDao;
import entity.KeyNode;
import dao.KeyDataDao;
import dao.KeyNodeDao;

@WebServlet("/ScrapStart")
public class ScrapStart extends AbstractServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(ScrapStart.class);

	public ScrapStart() {
		super();
	}

	@Override
	protected void doGet() {

		if (getParameter("MallCD") == null) {
			logger.error("The value of MallCD is null.");
			setStatus(401);
			return;
		}
		int mallkey;
		try {
			mallkey = Integer.parseInt(getParameter("MallCD"));
		} catch (NumberFormatException e) {
			logger.error(e);
			setStatus(401);
			return;
		}

		Parameter node = new Parameter(mallkey, Util.createUID());
		node.setApikey(getParameter("ApiKey"));
		node.setId1(getParameter("Id1"));
		node.setId2(getParameter("Id2"));
		node.setId3(getParameter("Id3"));
		node.setPw1(getParameter("Pw1"));
		node.setPw2(getParameter("Pw2"));
		node.setPw3(getParameter("Pw3"));
		node.setOption1(getParameter("Option1"));
		node.setOption2(getParameter("Option2"));
		node.setOption3(getParameter("Option3"));
		node.setScraptype(getParameter("ScrapType"));
		node.setSdate(getParameter("Sdate"));
		node.setEdate(getParameter("Edate"));
		node.setExec(getParameter("Exec"));

		logger.info("[WEB LOG] StartScrap");
		logger.info("[WEB LOG] PARAMETER - " + node.toJson());

		if (node.getApikey() == null) {
			setStatus(401);
			return;
		}

		if (!FactoryDao.getDao(KeyDataDao.class).hasApiKey(getParameter("ApiKey"))) {
			setStatus(401);
			return;
		} else if (node.getId1() == null) {
			setStatus(401);
			return;
		} else if (node.getPw1() == null) {
			setStatus(401);
			return;
		} else if (node.getExec() == null) {
			setStatus(401);
			return;
		} else if (node.getScraptype() == null) {
			setStatus(401);
			return;
		}

		try {
			if ("99".equals(node.getScraptype())) {
				List<String> list = new ArrayList<>();
				for (int i = 0; i < 6; i++) {
					Parameter cloneNode = node.clone(mallkey, Util.createUID());
					cloneNode.setScraptype(String.valueOf(i));
					WorkQueue.instance().insert(cloneNode);
					getPrinter().append(cloneNode.getKey() + "\r\n");
					KeyNode cnode = cloneNode.getKeyNode();
					list.add(cnode.getKey());
					FactoryDao.getDao(KeyNodeDao.class).merge(cnode);
					FactoryDao.getDao(RequestDataDao.class).merge(cloneNode);
				}
				String c = JsonConverter.create(list);
				KeyNode parent = node.getKeyNode();
				parent.setChildrunkey(c);
				FactoryDao.getDao(KeyNodeDao.class).merge(parent);
				getPrinter().append(node.getKey());
			} else {
				WorkQueue.instance().insert(node);
				getPrinter().append(node.getKey());
				FactoryDao.getDao(KeyNodeDao.class).merge(node.getKeyNode());
				FactoryDao.getDao(RequestDataDao.class).merge(node);
			}
		} catch (Throwable e) {
			logger.error(e);
		}
	}

	@Override
	protected void doPost() {
		doGet();
	}
}
