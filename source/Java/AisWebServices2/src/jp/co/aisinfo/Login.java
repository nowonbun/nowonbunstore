package jp.co.aisinfo;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import jp.co.aisdata.ITest;
import jp.co.aisinfo.Common.AisCommonHttpServlet;
import jp.co.aisinfo.Common.Define;
import jp.co.aisinfo.Common.Annotation.Allocation;
import jp.co.aisinfo.Util.ArrayUtil;
import jp.co.aisinfo.Util.BitConverter;
import jp.co.aisinfo.Util.StubJson;

@WebServlet("/Login")
public class Login extends AisCommonHttpServlet {

	private static final long serialVersionUID = 1L;
	
	@Allocation(ClassName = "jp.co.aisdata.ITestProxy") // TODO C#チームが提供するクラスを指定
	private ITest testdao; // TODO C#チームが提供するクラスを指定

	@Override
	protected String execute(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		String email = request.getParameter("email");
		String token = request.getParameter("token");
		String name = request.getParameter("name");
		
		System.out.println("email : " + email +", token : "+ token +", name: " + name);
		
		String key = testdao.doWork(); // TODO メソッドは　setID(String　email, String token, String name)になる予定 
		return new jp.co.aisinfo.Util.StubJson().getDataByKey(key, "setID");
		
		//return super.getDataByKey(key);
		//ResultTest result = getClassFromJson(ResultTest.class,data);
		//PrintWriter out = response.getWriter();
	}
	
	
}
