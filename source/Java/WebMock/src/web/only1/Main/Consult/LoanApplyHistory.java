package web.only1.Main.Consult;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanApplyHistory
 */
@WebServlet("/LoanApplyHistory")
public class LoanApplyHistory extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanApplyHistory() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출 접수 현황");
	}
}
