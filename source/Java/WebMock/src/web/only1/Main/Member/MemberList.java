package web.only1.Main.Member;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class MemberList
 */
@WebServlet("/MemberList")
public class MemberList extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public MemberList() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("회원 리스트");
	}
}
