import common.FactoryDao;
import common.Service;
import dao.KeyDataDao;

public class ApiTest {
	public static void main(String... args) {
		Service.newInstance("192.168.111.210", "root", "dhsfldnjs1", "scraping_sy");
		boolean ret = FactoryDao.getDao(KeyDataDao.class)
				.hasApiKey("fd5bc313a13344feb48474de4ac7535020180608002016312");
		System.out.println(ret);
		ret = FactoryDao.getDao(KeyDataDao.class).hasApiKey("fd5bc313a13344feb48474de4ac7535020180608002016312-");
		System.out.println(ret);
	}
}
