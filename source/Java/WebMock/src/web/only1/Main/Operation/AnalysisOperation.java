package web.only1.Main.Operation;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class AnalysisOperation
 */
@WebServlet("/AnalysisOperation")
public class AnalysisOperation extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public AnalysisOperation() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("수익률 분석");
	}
}
