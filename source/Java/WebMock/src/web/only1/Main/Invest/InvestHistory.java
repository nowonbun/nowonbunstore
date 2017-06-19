package web.only1.Main.Invest;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class InvestHistory
 */
@WebServlet("/InvestHistory")
public class InvestHistory extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public InvestHistory() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("투자 고객 원장 조회");
	}
}
