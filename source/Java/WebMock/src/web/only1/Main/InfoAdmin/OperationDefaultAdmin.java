package web.only1.Main.InfoAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class OperationDefaultAdmin
 */
@WebServlet("/OperationDefaultAdmin")
public class OperationDefaultAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public OperationDefaultAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("심사 기준 정보");
	}
}
