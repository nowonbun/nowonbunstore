package web.only1.Main.Invest;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class InvestApply
 */
@WebServlet("/InvestApply")
public class InvestApply extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public InvestApply() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("투자 접수 현황");
	}
}
