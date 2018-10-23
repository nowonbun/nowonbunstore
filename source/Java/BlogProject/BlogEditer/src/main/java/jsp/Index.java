package jsp;

import common.JspServlet;
import common.annotations.JspName;

@JspName("index.jsp")
public class Index extends JspServlet {

	private static final long serialVersionUID = 1L;

	@Override
	protected Object doJspMain() throws Exception {
		return null;
	}

	@Override
	protected void error(Throwable e) {

	}
}
