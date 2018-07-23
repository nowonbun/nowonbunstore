import java.util.List;


import common.FactoryDao;
import common.Service;
import dao.CommonDataDao;
import entity.CommonData;

public class DaoTest {
	public static void main(String... args) {
		Service.newInstance("192.168.111.210","root","dhsfldnjs1","scraping_sy");
		List<CommonData> list =  FactoryDao.getDao(CommonDataDao.class).select();
		for(CommonData l : list) {
			FactoryDao.getDao(CommonDataDao.class).delete(l);	
		}
	}
}
