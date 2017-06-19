package web.only1.Common;

public class Util {
	public static boolean CheckCode(int code, int flag){
		return (code & flag) != 0;
	}
}
