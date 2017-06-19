package web.only1.Main.SystemAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class ConnectionAdmin
 */
@WebServlet("/ConnectionAdmin")
public class ConnectionAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public ConnectionAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("연동 관리");
	}
}
