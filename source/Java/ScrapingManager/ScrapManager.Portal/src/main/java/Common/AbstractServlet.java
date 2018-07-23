package Common;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

public abstract class AbstractServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private HttpServletRequest request;
	private HttpServletResponse response;

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		this.request = request;
		this.response = response;
		request.setCharacterEncoding("UTF-8");
		response.setContentType("text/html; charset=utf-8");
		doGet();
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		this.request = request;
		this.response = response;
		request.setCharacterEncoding("UTF-8");
		response.setContentType("text/html; charset=utf-8");
		doPost();
	}

	protected HttpServletRequest getRequest() {
		return this.request;
	}

	protected HttpServletResponse getResponse() {
		return this.response;
	}

	protected HttpSession getSession() {
		return request.getSession();
	}

	protected Cookie[] getCookies() {
		return request.getCookies();
	}

	protected void Redirect(String url) {
		try {
			getResponse().sendRedirect(url);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected String getStreamData() {
		try (BufferedReader br = new BufferedReader(new InputStreamReader(getRequest().getInputStream()))) {
			return br.readLine();
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected PrintWriter getPrinter() {
		try {
			return getResponse().getWriter();
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected void setStatus(int code) {
		getResponse().setStatus(code);
	}

	protected String getParameter(String key) {
		return getRequest().getParameter(key);
	}

	protected String getPostParameters() {
		StringBuilder sb = new StringBuilder();
		BufferedReader br;
		try {
			br = request.getReader();
			String str = null;
			while ((str = br.readLine()) != null) {
				sb.append(str);
			}
			return sb.toString();
		} catch (IOException e) {
			return null;
		}
	}

	protected abstract void doGet();

	protected abstract void doPost();
}
