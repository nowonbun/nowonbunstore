package common;

import java.io.File;
import java.io.UnsupportedEncodingException;
import java.net.URLDecoder;
import java.util.HashMap;
import java.util.Map;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import common.annotations.JspName;

public class FactoryJspServlet {
	private static FactoryJspServlet singleton = null;
	private Map<String, JspServlet> flyweight = null;

	private FactoryJspServlet() {
		flyweight = new HashMap<>();
		String path = getClass().getProtectionDomain().getCodeSource().getLocation().getPath();
		try {
			File file = new File(URLDecoder.decode(path, "UTF-8") + "jsp/");
			File[] clazzFiles = file.listFiles();
			for (File clzFile : clazzFiles) {
				try {
					Class<?> clz = Class.forName("jsp." + clzFile.getName().replace(".class", ""));
					JspName anno = clz.getAnnotation(JspName.class);
					if (anno == null) {
						continue;
					}
					if (!flyweight.containsKey(anno.value())) {
						flyweight.put(anno.value(), (JspServlet) clz.newInstance());
					}

				} catch (ClassNotFoundException | InstantiationException | IllegalAccessException e) {
				}
			}
		} catch (UnsupportedEncodingException ex) {

		}
	}

	public static FactoryJspServlet instance() {
		if (singleton == null) {
			singleton = new FactoryJspServlet();
		}
		return singleton;
	}

	public static Object action(String name, HttpServletRequest request, HttpServletResponse response) {
		if (instance().flyweight.containsKey(name)) {
			return instance().flyweight.get(name).doJspWork(request, response);
		}
		System.exit(0);
		return null;
	}
}
