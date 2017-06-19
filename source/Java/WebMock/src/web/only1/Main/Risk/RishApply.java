package web.only1.Main.Risk;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class RishApply
 */
@WebServlet("/RishApply")
public class RishApply extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public RishApply() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("법수 접수 및 관리");
	}
}
