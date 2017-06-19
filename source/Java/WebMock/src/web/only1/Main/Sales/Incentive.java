package web.only1.Main.Sales;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class Incentive
 */
@WebServlet("/Incentive")
public class Incentive extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public Incentive() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("인센티브 조회");
	}
}
