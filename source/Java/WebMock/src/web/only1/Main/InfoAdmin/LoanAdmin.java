package web.only1.Main.InfoAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class LoanAdmin
 */
@WebServlet("/LoanAdmin")
public class LoanAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public LoanAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("대출 상품 관리");
	}
}
