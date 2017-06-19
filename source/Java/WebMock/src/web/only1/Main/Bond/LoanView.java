package web.only1.Main.Bond;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanView
 */
@WebServlet("/LoanView")
public class LoanView extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanView() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출 계약");
	}
}
