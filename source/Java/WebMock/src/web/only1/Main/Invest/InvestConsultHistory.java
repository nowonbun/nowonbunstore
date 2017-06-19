package web.only1.Main.Invest;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class InvestConsultHistory
 */
@WebServlet("/InvestConsultHistory")
public class InvestConsultHistory extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public InvestConsultHistory() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("투자 상담 내역 조회");
	}
}
