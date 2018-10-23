package common;

import java.io.IOException;
import java.net.URL;
import java.util.Hashtable;
import java.util.Map;
import java.util.Properties;

public class PropertyMap {
	private static PropertyMap singleton = null;

	public static PropertyMap getInstance() {
		if (singleton == null) {
			singleton = new PropertyMap();
		}
		return singleton;
	}

	private Map<String, Properties> map = new Hashtable<>();

	private PropertyMap() {
	}

	public String getProperty(String session, String key) {
		try {
			if (!map.containsKey(session)) {
				Properties pro = new Properties();
				map.put(session, pro);
				ClassLoader cl = Thread.currentThread().getContextClassLoader();
				URL url = cl.getResource(session + ".properties");
				pro.load(url.openStream());
			}
			Properties property = map.get(session);
			return property.getProperty(key);
		} catch (IOException e) {
			return null;
		}
	}
}
