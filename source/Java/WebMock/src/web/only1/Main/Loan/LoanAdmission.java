package web.only1.Main.Loan;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanAdmission
 */
@WebServlet("/LoanAdmission")
public class LoanAdmission extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanAdmission() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출 승인");
	}
}
