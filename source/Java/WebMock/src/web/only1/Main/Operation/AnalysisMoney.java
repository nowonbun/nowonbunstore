package web.only1.Main.Operation;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class AnalysisMoney
 */
@WebServlet("/AnalysisMoney")
public class AnalysisMoney extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public AnalysisMoney() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("영업률 분석");
	}
}
