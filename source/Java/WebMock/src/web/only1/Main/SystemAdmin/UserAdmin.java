package web.only1.Main.SystemAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class UserAdmin
 */
@WebServlet("/UserAdmin")
public class UserAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public UserAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("사용자 관리");
	}
}
