package Servlet;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;
import javax.servlet.annotation.WebServlet;
import Common.AjaxServlet;
import Entity.BrokerBean;
import common.FactoryDao;
import dao.BrokerDao;

@WebServlet("/GetBroker")
public class GetBroker extends AjaxServlet {

	private static final long serialVersionUID = 1L;
	private final static SimpleDateFormat format = new SimpleDateFormat("yyyy/MM/dd HH:mm.ss");

	@Override
	protected String doAjax() {
		List<BrokerBean> list = new ArrayList<>();
		FactoryDao.getDao(BrokerDao.class).select().forEach(x -> {
			BrokerBean b = new BrokerBean();
			list.add(b);
			b.setKey(x.getKey());
			b.setIp(x.getIp());
			b.setCount(x.getCount());
			b.setActive(x.getActive());
			if (x.getConnected() != null) {
				b.setConnected(format.format(x.getConnected()));
			}
			if (x.getDisconnected() != null) {
				b.setDisconnected(format.format(x.getDisconnected()));
			}
			if (x.getLastpingupdated() != null) {
				b.setLastpingupdated(format.format(x.getLastpingupdated()));
			}
		});
		return getDataTableData(list);
	}

}
