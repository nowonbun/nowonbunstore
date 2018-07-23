package Common;

import java.io.PrintWriter;
import Entity.ObjectBean;

public abstract class AjaxServlet extends AbstractServlet {
	private static final long serialVersionUID = 1L;

	@Override
	protected final void doGet() {
		doPost();
	}

	@Override
	protected final void doPost() {
		try {
			super.getRequest().setCharacterEncoding("UTF-8");
			super.getResponse().setContentType("text/html;charset=UTF-8");
		} catch (Exception e) {
			e.printStackTrace();
		}
		try (PrintWriter writer = super.getPrinter()) {
			writer.println(doAjax());
		}
	}

	protected final String getDataTableData(Object obj) {
		ObjectBean node = new ObjectBean();
		node.setData(obj);
		return getJsonData(node);
	}

	protected final String getJsonData(Object obj) {
		if (obj == null) {
			obj = "";
		}
		return JsonConverter.create(obj);
	}

	protected abstract String doAjax();
}
