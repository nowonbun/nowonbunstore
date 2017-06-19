package web.only1.Main.Sales;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class Fees
 */
@WebServlet("/Fees")
public class Fees extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public Fees() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("수수료 조회");
	}
}
