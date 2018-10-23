package common;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public abstract class JspServlet extends IServlet {

	private static final long serialVersionUID = 1L;

	protected Object doJspWork(HttpServletRequest request, HttpServletResponse response) {
		super.doWork(request, response);

		try {
			Object ret = doJspMain();
			if (getResponse().getStatus() != 200) {
				JspException();
				return null;
			}
			return ret;
		} catch (Throwable e) {
			error(e);
			JspException(500);
			return null;
		}
	}

	private void JspException() {
		try {
			getResponse().getWriter().close();
		} catch (Throwable e) {
			e.printStackTrace();
		}
	}

	private void JspException(int code) {
		try {
			getResponse().setStatus(code);
			getResponse().getWriter().close();
		} catch (Throwable e) {
			e.printStackTrace();
		}
	}

	protected abstract Object doJspMain() throws Exception;

	protected final void doMain() throws Exception {
	}

	protected abstract void error(Throwable e);

}
