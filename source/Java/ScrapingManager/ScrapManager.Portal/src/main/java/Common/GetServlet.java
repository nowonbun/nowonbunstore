package Common;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Map;

import javax.net.ssl.HttpsURLConnection;

//usage
//String host = PropertyManager.getProperty("project", "ServletHost");
//Map<String, String> param = new HashMap<>();
//param.put("test", "1234");
//String data = GetServlet.getHttpConnection(host + "GetBroker", "GET", param);
public class GetServlet {
	public static String getHttpConnection(String target, String method, Map<String, String> param) {
		HttpURLConnection conn = null;
		try {
			URL url = new URL(target + getParameter(param));
			conn = (HttpURLConnection) url.openConnection();
			conn.setRequestMethod(method);
			return getData(conn.getInputStream());
		} catch (Throwable e) {
			return null;
		} finally {
			if (conn != null) {
				conn.disconnect();
			}
		}
	}

	public static String getHttpsConnection(String target, String method, Map<String, String> param) {
		HttpsURLConnection conn = null;
		try {
			URL url = new URL(target + getParameter(param));
			conn = (HttpsURLConnection) url.openConnection();
			conn.setRequestMethod(method);
			return getData(conn.getInputStream());
		} catch (Throwable e) {
			return null;
		} finally {
			if (conn != null) {
				conn.disconnect();
			}
		}
	}

	private static String getParameter(Map<String, String> param) {
		if (param == null) {
			return "";
		}
		StringBuffer sb = new StringBuffer();
		sb.append("?");
		for (String key : param.keySet()) {
			sb.append(key);
			sb.append("=");
			sb.append(param.get(key));
			sb.append("&");
		}
		sb.setLength(sb.length() - 1);
		return sb.toString();
	}

	private static String getData(InputStream is) {
		try (BufferedReader reader = new BufferedReader(new InputStreamReader(is))) {
			StringBuffer sb = new StringBuffer();
			String line;
			while ((line = reader.readLine()) != null) {
				sb.append(line);
				sb.append("\r\n");
			}
			return sb.toString();
		} catch (Throwable e) {
			e.printStackTrace();
			return null;
		}

	}
}
