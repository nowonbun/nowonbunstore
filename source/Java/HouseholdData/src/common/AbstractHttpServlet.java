package common;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Map;
import java.util.Properties;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public abstract class AbstractHttpServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private HttpServletRequest request;
	private HttpServletResponse response;
	private Properties properties = null;

	public AbstractHttpServlet() {
		super();
	}

	protected HttpServletRequest getRequest() {
		return this.request;
	}

	protected HttpServletResponse getResponse() {
		return this.response;
	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		response.setHeader("Content-Type", "text/html;charset=UTF-8");
		this.request = request;
		this.response = response;
		try {
			Object ret = execute(request.getParameterMap());
			String json = JsonConverter.create(ret);
			String code = sendData(json);
			response.getWriter().print(code);
		} catch (HouseholdException e) {
			response.sendError(e.getErrorCode());
		}
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		doGet(request, response);
	}

	private String sendData(String json) {
		String serveruri = getProperty("DataServer");
		try {
			URL url = new URL(serveruri);
			HttpURLConnection conn = (HttpURLConnection) url.openConnection();
			conn.setRequestMethod("POST");
			conn.setDoInput(true);
			conn.setDoOutput(true);
			try(OutputStream out = conn.getOutputStream()){
				out.write("DATA=".getBytes("UTF-8"));
				out.write(json.getBytes("UTF-8"));
				out.flush();
			}
			int length = conn.getContentLength();
			byte[] recv = new byte[length];
			try(InputStream in = conn.getInputStream()){
				in.read(recv,0,recv.length);
			}
			return new String(recv,"UTF-8");
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	private String getProperty(String key) {
		try {
			if (properties == null) {
				properties = new Properties();
				properties
						.load(Thread.currentThread().getContextClassLoader().getResourceAsStream("project.properties"));
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
		return properties.getProperty(key);
	}

	protected abstract Object execute(Map<String,String[]> parameter);
}