package web.only1.Main.Operation;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class AnalysisRevenue
 */
@WebServlet("/AnalysisRevenue")
public class AnalysisRevenue extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public AnalysisRevenue() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("자금 회전률 분석");
	}
}
