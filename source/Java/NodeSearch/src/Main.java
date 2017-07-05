import java.io.File;

public class Main {
	public static void main(String[] args){
		try{
			String path = System.getProperty("user.dir");
			Subway subway = new Subway();
			//프로퍼티 역정보(역정보)취득
			subway.addStation(new File(path+"/station.txt"));
			//프로퍼티 링크정보(지하철 노선)취득
			subway.setLinkStation(new File(path+"/link.txt"));
			//의정부 북부에서 교대의 최단 노선을 구하여라
			String ret = subway.search("1","91");
			//결과출력
			System.out.println(ret);
		}catch(Throwable e){
			e.printStackTrace();
		}
	}
}
