package Common;

import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

public class PropertyManager {

	private static PropertyManager instance = null;
	private Map<String, Properties> flyweight = new HashMap<>();

	private PropertyManager() {
	}

	private static PropertyManager singleton() {
		if (instance == null) {
			instance = new PropertyManager();
		}
		return instance;
	}

	public static String getProperty(String property, String key) {
		Properties properties = singleton().getProperties(property);
		return properties.getProperty(key);
	}

	public static int getPropertyInt(String property, String key) {
		return Integer.parseInt(getProperty(property, key));
	}

	private Properties getProperties(String property) {
		if (!flyweight.containsKey(property)) {
			try (InputStream stream = Thread.currentThread().getContextClassLoader()
					.getResourceAsStream(property + ".properties")) {
				Properties properties = new Properties();
				properties.load(stream);
				flyweight.put(property, properties);
			} catch (Throwable e) {
				e.printStackTrace();
			}
		}
		return flyweight.get(property);
	}
}
