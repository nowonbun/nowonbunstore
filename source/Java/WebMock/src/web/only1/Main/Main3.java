package web.only1.Main;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

@WebServlet("/Main3")
public class Main3 extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public Main3() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("Main3");
	}
}
