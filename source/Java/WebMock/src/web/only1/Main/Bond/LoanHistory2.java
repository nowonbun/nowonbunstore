package web.only1.Main.Bond;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanHistory2
 */
@WebServlet("/LoanHistory2")
public class LoanHistory2 extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanHistory2() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출 고객 원장 조회");
	}
}
