package common;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.List;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.springframework.ui.ModelMap;
import bean.CategoryBean;
import dao.CategoryDao;
import model.Category;

public abstract class IController extends HttpServlet {

	private static final long serialVersionUID = 1L;

	protected void setCookie(HttpServletResponse response, String key, String value) {
		Cookie cookie = new Cookie(key, value);
		cookie.setMaxAge(getCookieExpire());
		cookie.setPath(getCookiePath());
		response.addCookie(cookie);
	}

	protected void deleteCookie(HttpServletResponse response, String key) {
		Cookie cookie = new Cookie(key, "");
		cookie.setMaxAge(0);
		cookie.setPath(getCookiePath());
		response.addCookie(cookie);
	}

	protected Cookie[] getCookies(HttpServletRequest request) {
		return request.getCookies();
	}

	protected Cookie getCookie(HttpServletRequest request, String name) {
		return Util.searchArray(getCookies(request), (node) -> {
			return Util.StringEquals(name, node.getName());
		});
	}

	protected void redirect(HttpServletResponse response, String url) {
		try {
			response.sendRedirect(url);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected String getStreamData(HttpServletRequest request) {
		try (BufferedReader br = new BufferedReader(new InputStreamReader(request.getInputStream()))) {
			return br.readLine();
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected PrintWriter getPrinter(HttpServletResponse response) {
		try {
			return response.getWriter();
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected void setStatus(HttpServletResponse response, int code) {
		response.setStatus(code);
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

	protected void initMenu(ModelMap modelmap, HttpServletRequest request) {
		List<CategoryBean> menuitem = new ArrayList<>();
		java.util.List<Category> list = FactoryDao.getDao(CategoryDao.class).selectAll();
		list.sort((x, y) -> Integer.compare(x.getSequence(), y.getSequence()));
		for (Category m : list) {
			if (m.getIsdeleted()) {
				continue;
			}
			CategoryBean item = new CategoryBean();
			menuitem.add(item);
			item.setCategoryHref(request.getServletContext().getContextPath() + m.getUrl());
			item.setCategoryText(m.getCategoryName());
		}
		modelmap.addAttribute("menu", menuitem);
	}
}