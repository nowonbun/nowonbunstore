package web.only1.Main.SystemAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class SystemAdmin
 */
@WebServlet("/SystemAdmin")
public class SystemAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public SystemAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("시스템 설정");
	}
}
