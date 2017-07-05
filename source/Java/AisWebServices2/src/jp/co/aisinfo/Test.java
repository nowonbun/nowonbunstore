package jp.co.aisinfo;

import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import jp.co.aisdata.ITest;
import jp.co.aisinfo.Common.AisCommonHttpServlet;
import jp.co.aisinfo.Common.Annotation.Allocation;



@WebServlet("/Test")
public class Test extends AisCommonHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@Allocation(ClassName = "jp.co.aisdata.ITestProxy")
	private ITest testdao; 

	@Override
	protected String execute(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		String key = testdao.doWork();
		return super.getDataByKey(key);
		//ResultTest result = getClassFromJson(ResultTest.class,data);
		//PrintWriter out = response.getWriter();
	}

}
