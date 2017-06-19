package web.only1.Main.Risk;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class RiskApply2
 */
@WebServlet("/RiskApply2")
public class RiskApply2 extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public RiskApply2() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대손 접수 및 관리");
	}
}
