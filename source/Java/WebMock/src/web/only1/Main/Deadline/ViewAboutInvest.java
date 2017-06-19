package web.only1.Main.Deadline;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class ViewAboutInvest
 */
@WebServlet("/ViewAboutInvest")
public class ViewAboutInvest extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public ViewAboutInvest() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출처별 원장 조회");
	}
}
