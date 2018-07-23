package Common;

import java.text.SimpleDateFormat;
import java.util.Date;

public class Util {

	private static SimpleDateFormat dayTime = new SimpleDateFormat("yyyyMMddHHmmssSSS");
	private static SimpleDateFormat CSharp_datetime = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");

	public static boolean StringEquals(String val1, String val2) {
		if (val1 == null) {
			return false;
		}
		if (val2 == null) {
			return false;
		}
		return val1.equals(val2);
	}

	public static boolean StringIsNullEmpty(String val) {
		if (val == null) {
			return true;
		}
		if (val.trim().length() < 1) {
			return true;
		}
		return false;
	}

	public static String createUID() {
		try {
			Thread.sleep(1);
		} catch (Throwable e) {

		}
		return dayTime.format(new Date(System.currentTimeMillis()));
	}

	public static Date ConvertToDateFromString(String val) {
		try {
			return CSharp_datetime.parse(val);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public static String ConvertToStringFromDate(Date val) {
		try {
			return CSharp_datetime.format(val);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}
}
