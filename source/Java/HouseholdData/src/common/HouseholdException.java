package common;

public class HouseholdException extends RuntimeException{
	private static final long serialVersionUID = 1L;
	private int errorcode;
	public HouseholdException(int errorcode){
		this.errorcode = errorcode;
	}
	public int getErrorCode(){
		return errorcode;
	}
}
