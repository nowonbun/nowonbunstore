import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.UUID;

public class uid {
	public static void main(String[] args) {
		SimpleDateFormat dayTime = new SimpleDateFormat("yyyyMMddHHmmssSSS");
		String strDT = dayTime.format(new Date(System.currentTimeMillis()));
		System.out.println(UUID.randomUUID().toString().replace("-", "") + strDT);
	}
}
