import javax.json.Json;
import javax.json.JsonObjectBuilder;

public class Test {
	public static void main(String... args) {
		try {
			JsonObjectBuilder builder = Json.createObjectBuilder();
			builder.add("no", 1);
			builder.add("count", 0);
			String json = builder.build().toString();
			System.out.println(json);
			//FactoryDao.getDao(BrokerDao.class).insert("127.0.0.1");
			//FactoryDao.getDao(CommonDataDao.class).insert("TEST",1,"TEST");
		} catch (Throwable e) {
			e.printStackTrace();
		}
	}
}
