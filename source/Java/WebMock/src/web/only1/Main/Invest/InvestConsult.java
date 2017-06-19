package web.only1.Main.Invest;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class InvestConsult
 */
@WebServlet("/InvestConsult")
public class InvestConsult extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public InvestConsult() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("투자 상담 관리");
	}
}
