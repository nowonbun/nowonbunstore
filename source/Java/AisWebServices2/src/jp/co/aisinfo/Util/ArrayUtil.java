package jp.co.aisinfo.Util;

public class ArrayUtil {
	public static byte[] reverse(byte[] data) {
		byte[] ret = new byte[data.length];
		for (int i = 0; i < ret.length; i++) {
			ret[i] = data[data.length - i - 1];
		}
		return ret;
	}
}
