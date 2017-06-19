package web.only1.Main.Bond;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanAgreement
 */
@WebServlet("/LoanAgreement")
public class LoanAgreement extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanAgreement() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("실행 대출 관리");
	}
}
