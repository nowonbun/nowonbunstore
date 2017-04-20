package test;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.stream.Stream;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.tempuri.*;

import com.sun.xml.internal.bind.v2.runtime.unmarshaller.XsiNilLoader.Array;

/**
 * Servlet implementation class Test
 */
@WebServlet("/Test")
public class Test extends HttpServlet {
	private static final long serialVersionUID = 1L;

	/**
	 * @see HttpServlet#HttpServlet()
	 */
	public Test() {
		super();
		// TODO Auto-generated constructor stub
	}

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse
	 *      response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		ITest test = new ITestProxy();
		String key = test.doWork();
		System.out.println(key);
		byte[] data = key.getBytes("UTF-8");
		byte[] length = reverse(BitConverter.getBytes(data.length));
		
		try (Socket sock = new Socket("127.0.0.1", 15000)) {
			OutputStream out = sock.getOutputStream();
			InputStream in = sock.getInputStream();
			out.write(length);
			out.write(data);
			in.read(length);
			int len = BitConverter.toInt32(reverse(length), 0);
			data = new byte[len];
			in.read(data, 0, len);
			System.out.println(new String(data));
		} catch (Throwable e) {
			e.printStackTrace();
		}

	}

	private byte[] reverse(byte[] data) {
		byte[] ret = new byte[data.length];
		for (int i = 0; i < ret.length; i++) {
			ret[i] = data[data.length - i - 1];
		}
		return ret;
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse
	 *      response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
