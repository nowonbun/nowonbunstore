package web.only1.Main.Consult;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanApply
 */
@WebServlet("/LoanApply")
public class LoanApply extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanApply() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출 접수");
	}
}
