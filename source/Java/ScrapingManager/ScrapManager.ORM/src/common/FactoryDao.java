package common;

import java.lang.reflect.Constructor;
import java.util.HashMap;
import java.util.Map;

public class FactoryDao {
	private static FactoryDao instance = null;
	private final Map<Class<?>, IDao<?>> flyweight = new HashMap<>();

	@SuppressWarnings("unchecked")
	public static <T> T getDao(Class<T> clz) {
		try {
			if (instance == null) {
				instance = new FactoryDao();
			}
			if (!instance.flyweight.containsKey(clz)) {
				Constructor<T> cons = clz.getDeclaredConstructor();
				cons.setAccessible(true);
				instance.flyweight.put(clz, (IDao<?>) cons.newInstance());
			}
			return (T) instance.flyweight.get(clz);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}
}
