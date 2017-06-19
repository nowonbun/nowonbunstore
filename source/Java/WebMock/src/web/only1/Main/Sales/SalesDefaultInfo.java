package web.only1.Main.Sales;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class SalesDefaultInfo
 */
@WebServlet("/SalesDefaultInfo")
public class SalesDefaultInfo extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public SalesDefaultInfo() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("영업 기준 정보");
	}
}
