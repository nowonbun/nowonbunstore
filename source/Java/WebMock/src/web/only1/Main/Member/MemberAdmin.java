package web.only1.Main.Member;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class MemberAdmin
 */
@WebServlet("/MemberAdmin")
public class MemberAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public MemberAdmin() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("회원 정보 관리");
	}
}
