package web.only1.Main.Consult;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class ConsultHistory2
 */
@WebServlet("/ConsultHistory2")
public class ConsultHistory2 extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public ConsultHistory2() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("상담 실적 현황");
	}
}
