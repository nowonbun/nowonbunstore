package web.only1.Main.Deadline;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class ViewAboutLoan
 */
@WebServlet("/ViewAboutLoan")
public class ViewAboutLoan extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public ViewAboutLoan() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("투자처별 원장 조회");
	}
}
