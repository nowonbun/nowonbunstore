package jp.co.aisinfo.Common;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.StringReader;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Field;
import java.lang.reflect.ParameterizedType;
import java.net.Socket;
import java.util.Arrays;
import java.util.Map;

import javax.json.Json;
import javax.json.JsonObject;
import javax.json.JsonReader;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import jp.co.aisdata.ITest;
import jp.co.aisdata.ITestProxy;
import jp.co.aisinfo.Common.Annotation.Allocation;
import jp.co.aisinfo.Util.ArrayUtil;
import jp.co.aisinfo.Util.BitConverter;

public abstract class AisCommonHttpServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;

	public AisCommonHttpServlet() {
		Field[] fileds = this.getClass().getDeclaredFields();
		Arrays.asList(fileds).stream().filter(f->{
			return f.getAnnotation(Allocation.class) != null;
		}).forEach(f->{
			try{
				Allocation anno = f.getAnnotation(Allocation.class);
				String classname = anno.ClassName();
				f.setAccessible(true);
				f.set(this, Class.forName(classname).newInstance());
			}catch(Throwable e){
				throw new RuntimeException(e);
			}
		});
	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		// response.sendRedirect("/");
		doPost(request,response);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		String json = execute(request, response);
		response.getWriter().write(json);
	}

	protected String getDataByKey(String key) {
		try {
			byte[] data = key.getBytes(Define.DEFAULT_ENCODING);
			byte[] length = ArrayUtil.reverse(BitConverter.getBytes(data.length));
			try (Socket sock = new Socket(Define.SOCKET_IP_ADDRESS, Define.SOCKET_PORT)) {
				OutputStream out = sock.getOutputStream();
				InputStream in = sock.getInputStream();
				out.write(length);
				out.write(data);
				in.read(length);
				int len = BitConverter.toInt32(ArrayUtil.reverse(length), 0);
				data = new byte[len];
				in.read(data, 0, len);
				return new String(data);
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected <T> T getClassFromJson(Class<T> classtype, String json) {
		try {
			JsonReader reader = Json.createReader(new StringReader(json));
			JsonObject object = reader.readObject();
			T obj = (T) classtype.newInstance();
			for (String key : object.keySet()) {
				Field field = classtype.getField(key);
				if(field == null){
					continue;
				}
				field.setAccessible(true);
				if(String.class.equals(field.getType())){
					field.set(obj, object.getString(key));
				}
			}
			return obj;
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	protected abstract String execute(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException;
}
