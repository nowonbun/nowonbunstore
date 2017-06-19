package web.only1.Main.Member;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class MemberHistory
 */
@WebServlet("/MemberHistory")
public class MemberHistory extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public MemberHistory() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("회원 변경 히스토리");
	}
}
