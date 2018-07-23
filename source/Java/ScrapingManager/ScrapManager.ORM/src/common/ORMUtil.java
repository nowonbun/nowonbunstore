package common;

import java.text.SimpleDateFormat;
import java.util.TimeZone;

public class ORMUtil {
	public static SimpleDateFormat DEFAULT_DAYTIME = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");
	public static SimpleDateFormat DAYTIME_SIMPLE = new SimpleDateFormat("yy/MM/dd HH:mm:ss");
	
	public static void setTimeZone() {
		ORMUtil.DEFAULT_DAYTIME.setTimeZone(TimeZone.getTimeZone("GMT+6"));
	}
}
