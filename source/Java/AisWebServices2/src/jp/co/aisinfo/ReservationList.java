package jp.co.aisinfo;

import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import jp.co.aisdata.ITest;
import jp.co.aisinfo.Common.AisCommonHttpServlet;
import jp.co.aisinfo.Common.Annotation.Allocation;

@WebServlet("/ReservationList")
public class ReservationList extends AisCommonHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@Allocation(ClassName = "jp.co.aisdata.ITestProxy") // TODO C#チームが提供するクラスを指定
	private ITest testdao; // TODO C#チームが提供するクラスを指定

	@Override
	protected String execute(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		
		// TODO session 処理追加
		
		String meetingRoomCode = request.getParameter("meetingRoomCode");
		String date = request.getParameter("date");
		
		if (meetingRoomCode.isEmpty()) {
			String key = testdao.doWork(); // TODO meetingRoom(String　date)になる予定 date は　"2017-04-12"　形式の10桁文字列
			return new jp.co.aisinfo.Util.StubJson().getDataByKey(key, "meetingRoom1");
		} else {
			String key = testdao.doWork(); // TODO meetingRoom(String　date, int meetingRoomCode)になる予定 date は　"2017-04-12"　形式の10桁文字列
			return new jp.co.aisinfo.Util.StubJson().getDataByKey(key, "meetingRoom2");
		}
		//return super.getDataByKey(key);
		//ResultTest result = getClassFromJson(ResultTest.class,data);
		//PrintWriter out = response.getWriter();
	}
}
