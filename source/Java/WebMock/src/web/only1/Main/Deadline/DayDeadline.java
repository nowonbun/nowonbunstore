package web.only1.Main.Deadline;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

/**
 * Servlet implementation class DayDeadline
 */
@WebServlet("/DayDeadline")
public class DayDeadline extends AbstractLoadServlect {
	private static final long serialVersionUID = 1L;

	public DayDeadline() {
		super();
	}

	@Override
	protected ViewResult execute() {
		return View.nativeCode("일 마감");
	}
}
