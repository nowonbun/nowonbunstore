package web.only1.Main.InfoAdmin;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class MemberTypeAdmin
 */
@WebServlet("/MemberTypeAdmin")
public class MemberTypeAdmin extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public MemberTypeAdmin() {
		super();
	}
	@Override
	protected ViewResult execute() {
		return View.nativeCode("회원 구분 항목 관리");
	}
}
