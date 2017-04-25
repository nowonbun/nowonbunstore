package common;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.lang.reflect.Field;
import java.lang.reflect.Type;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Map;
import java.util.Properties;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.log4j.Logger;

import dao.FactoryDao;

public abstract class AbstractHttpServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private HttpServletRequest request;
	private HttpServletResponse response;
	private Properties properties = null;
	protected Logger logger = LoggerManager.getLogger(this.getClass());

	public AbstractHttpServlet() {
		super();
		logger.info(this.getClass());
		logger.info("request web page");
		Class<?> clazz = this.getClass();
		Field[] fields = clazz.getDeclaredFields();
		for (Field field : fields) {
			ResourceDao resource = field.getAnnotation(ResourceDao.class);
			if (resource == null) {
				continue;
			}
			field.setAccessible(true);
			Type type = field.getType();
			try {
				Class<?> daoclz = Class.forName(type.getTypeName());
				field.set(this, FactoryDao.getDao(daoclz));
			} catch (Throwable e) {
				logger.error(e);
			}
		}
	}

	protected HttpServletRequest getRequest() {
		return this.request;
	}

	protected HttpServletResponse getResponse() {
		return this.response;
	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		logger.info(request.getRemoteHost());
		logger.error("response error code : 406" );
		response.sendError(406);
//		doPost(request, response);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		logger.info(request.getRemoteHost());
		request.setCharacterEncoding("UTF-8");
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

	private String sendData(String json) {
		String serveruri = getProperty("DataServer");
		try {
			URL url = new URL(serveruri);
			HttpURLConnection conn = (HttpURLConnection) url.openConnection();
			conn.setRequestMethod("POST");
			conn.setDoInput(true);
			conn.setDoOutput(true);
			try (OutputStream out = conn.getOutputStream()) {
				out.write("DATA=".getBytes("UTF-8"));
				out.write(json.getBytes("UTF-8"));
				out.flush();
			}
			int length = conn.getContentLength();
			byte[] recv = new byte[length];
			try (InputStream in = conn.getInputStream()) {
				in.read(recv, 0, recv.length);
			}
			return new String(recv, "UTF-8");
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	private String getProperty(String key) {
		try {
			if (properties == null) {
				properties = new Properties();
				InputStream stream = Thread.currentThread().getContextClassLoader()
						.getResourceAsStream("project.properties");
				properties.load(stream);
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
		return properties.getProperty(key);
	}

	protected abstract Object execute(Map<String, String[]> parameter);
}