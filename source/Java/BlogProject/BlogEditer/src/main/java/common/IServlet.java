package common;

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

public abstract class IServlet extends HttpServlet {

	private static final long serialVersionUID = 1L;
	private HttpServletRequest request;
	private HttpServletResponse response;

	protected void doWork(HttpServletRequest request, HttpServletResponse response) {
		this.request = request;
		this.response = response;
		getResponse().setContentType("text/html;charset=UTF-8");
		try {
			doMain();
		} catch (Throwable e) {
			error(e);
		}

	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doWork(request, response);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doWork(request, response);
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

	protected void setCookie(String key, String value) {
		Cookie cookie = new Cookie(key, value);
		cookie.setMaxAge(getCookieExpire());
		cookie.setPath(getCookiePath());
		getResponse().addCookie(cookie);
	}

	protected void deleteCookie(String key) {
		Cookie cookie = new Cookie(key, "");
		cookie.setMaxAge(0);
		cookie.setPath(getCookiePath());
		getResponse().addCookie(cookie);
	}

	protected Cookie[] getCookies() {
		return request.getCookies();
	}

	protected Cookie getCookie(String name) {
		return Util.searchArray(getCookies(), (node) -> {
			return Util.StringEquals(name, node.getName());
		});
	}

	protected void redirect(String url) {
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

	protected String getCookieKey() {
		return PropertyMap.getInstance().getProperty("config", "cookie_key");
	}

	protected int getCookieExpire() {
		return Integer.parseInt(PropertyMap.getInstance().getProperty("config", "cookie_expire"));
	}

	protected String getCookiePath() {
		return PropertyMap.getInstance().getProperty("config", "cookie_path");
	}

	protected String combineParameter(String url, String name, String paramter) {
		if (url.contains("?")) {
			url += "&" + name + "=" + paramter;
		} else {
			url += "?" + name + "=" + paramter;
		}
		return url;
	}

	protected abstract void doMain() throws Exception;

	protected abstract void error(Throwable e);

}