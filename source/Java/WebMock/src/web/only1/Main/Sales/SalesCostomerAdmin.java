package web.only1.Main.Sales;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class SalesCostomerAdmin
 */
@WebServlet("/SalesCostomerAdmin")
public class SalesCostomerAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public SalesCostomerAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("영업 고객 관리");
	}
}
