package web.only1.Main.Bond;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanPaymentSchedule
 */
@WebServlet("/LoanPaymentSchedule")
public class LoanPaymentSchedule extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanPaymentSchedule() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("상환 스케줄 관리");
	}
}
