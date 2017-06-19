package web.only1.Main.SystemAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LanguegeAdmin
 */
@WebServlet("/LanguegeAdmin")
public class LanguegeAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LanguegeAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("Localization 설정");
	}
}
