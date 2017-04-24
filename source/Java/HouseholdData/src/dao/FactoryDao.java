package dao;

import java.util.HashMap;
import java.util.Map;

@SuppressWarnings("rawtypes")
public class FactoryDao {
	private static FactoryDao instance = null;
	
	public static Dao getDao(Class<?> clz) {
		if (instance == null) {
			instance = new FactoryDao();
		}
		return instance.get(clz);
	}

	private Map<Class<?>,Dao> flyweight = new HashMap<>();
	private FactoryDao() { }

	private Dao get(Class<?> clz) {
		if(!flyweight.containsKey(clz)){
			try{
				flyweight.put(clz, (Dao)clz.newInstance());
			}catch(Throwable e){
				throw new RuntimeException(e);
			}
		}
		return flyweight.get(clz);
	}
}
