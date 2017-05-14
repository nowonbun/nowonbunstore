package common;

import java.text.SimpleDateFormat;
import java.util.Date;
import model.Hshld;
import model.HshldLog;

public class Util {
	private static SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");

	public static boolean StringEquals(String val1, String val2) {
		if (val1 == null) {
			return false;
		}
		if (val2 == null) {
			return false;
		}
		return val1.equals(val2);
	}

	public static Date convertStringtoDate(String data) {
		try {
			return sdf.parse(data);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}
	
	public static HshldLog trans(Hshld entity){
		HshldLog ret = new HshldLog();
		ret.setId(entity.getUsrNf().getId());
		ret.setCd(entity.getCtgry().getCd());
		ret.setTp(entity.getTpBean().getTp());
		ret.setDt(entity.getDt());
		ret.setCntxt(entity.getCntxt());
		ret.setPrc(entity.getPrc());
		return ret;
	}
}
