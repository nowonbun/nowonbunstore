package web.only1.Common;

public class StringUtil {
	public static boolean equals(String val1,String val2){
		if(val1 == null){
			return false;
		}
		if(val2 == null){
			return false;
		}
		return val1.equals(val2);
	}
	public static boolean isEmpty(String val){
		if(val == null){
			return true;
		}
		return val.isEmpty();
	}
}
