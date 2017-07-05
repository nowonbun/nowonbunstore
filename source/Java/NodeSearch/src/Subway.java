import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Stack;

/**
 * 지하철 노선 탐색 알고리즘 
 */
public class Subway {
	// 역정보 리스트
	private final ArrayList<Station> stations;
	/**
	 * 생성자 리스트
	 */
	public Subway(){
		stations = new ArrayList<Station>();
	}
	/**
	 * 역추가(다형성)
	 * @param code 역 코드
	 * @param name 역 이름
	 */
	public void addStation(String code,String name) throws SubwayException{
		Station s = new Station();
		s.setCode(code);
		s.setName(name);
		addStation(s);
	}
	/**
	 * 역추가(다형성)
	 * @param station 역 클래스
	 */
	public void addStation(Station station) throws SubwayException{
		//역코드가 중복일시 에러를 내보낸다
		if(getStation(station.getCode()) != null){
			throw new SubwayException("same code");
		}
		stations.add(station);
	}
	/**
	 * 파일로 부터 역정보를 읽어드린다.(다형성)
	 * @param file 파일
	 */
	public void addStation(File file) throws SubwayException,IOException{
		//파일을 전부 읽어드린다.
		byte[] buffer = new byte[(int)file.length()];
		try(FileInputStream in = new FileInputStream(file)){
			in.read(buffer, 0, buffer.length);
		}
		String data = new String(buffer);
		// 구분자 \n로 역정보를 전부 읽어드린다.(라인 하나에 역하나)
		String[] lineBuffer = data.split("\n");
		// 데이터가 하나도 없으면 에러
		if(lineBuffer.length <= 0) {
			new IOException("document style wrong");
		}
		// 역을 클래스화
		for(String line : lineBuffer){
			// 구분자는 콤마[,]
			String[] buf = line.split(",");
			// 데이터 이상은 통과
			if(buf.length != 2){
				continue;
			}
			// 파일 형식은 코드,역이름
			addStation(buf[0],buf[1]);
		}
	}
	/**
	 * 코드로 역을 찾는다
	 * @param code
	 */
	public Station getStation(String code){
		for(Station s: stations){
			if(s.getCode().equals(code)){
				return s;
			}
		}
		return null;
	}
	/**
	 * 역 링크정보 읽기
	 * @param file 파일
	 */
	public void setLinkStation(File file) throws IOException{
		//파일을 전부 읽어드린다.
		byte[] buffer = new byte[(int)file.length()];
		try(FileInputStream in = new FileInputStream(file)){
			in.read(buffer, 0, buffer.length);
		}
		String data = new String(buffer);
		// 구분자 \n로 링크정보를 전부 읽어드린다.(라인 하나에 링크하나)
		String[] lineBuffer = data.split("\n");
		if(lineBuffer.length <= 0){
			new IOException("document style wrong");
		}
		// 링크 만들기
		for(String line : lineBuffer){
			// 구분자는 콤마[,]
			String[] buf = line.split(",");
			// 데이터 이상은 통과
			if(buf.length != 3){
				continue;
			}
			//buf[1]의 다음역은 buf[2]
			if("n".equals(buf[0])){
				setNextStation(buf[1],buf[2]);
			}
			//buf[1]의 전역은 buf[2]
			else if("p".equals(buf[0])){
				setPrevStation(buf[1],buf[2]);
			}
		}
	}
	/**
	 * 다음역 세팅
	 * @param point 기준 역 정보
	 * @param next 다음 역 정보
	 */
	public void setNextStation(Station point,Station next){
		point.addNext(next);
		next.addPrev(point);
	}
	/**
	 * 다음역 세팅(다형성)
	 * @param pointCode 역 코드
	 * @param nextCode 역 코드
	 */
	public void setNextStation(String pointCode,String nextCode){
		Station point = getStation(pointCode);
		Station next = getStation(nextCode);
		setNextStation(point,next);
	}
	/**
	 * 전역 세팅
	 * @param point 기준 역 정보
	 * @param prev 전역정보
	 */
	public void setPrevStation(Station point,Station prev){
		point.addPrev(prev);
		prev.addNext(point);
	}
	/**
	 * 전역 세팅(다형성)
	 * @param pointCode 전역 코드
	 * @param prevCode 전역 코드
	 */
	public void setPrevStation(String pointCode,String prevCode){
		Station point = getStation(pointCode);
		Station prev = getStation(prevCode); 
		setPrevStation(point,prev);
	}
	/**
	 * 역 탐색
	 * @param start 출발 역코드
	 * @param end 도착 역코드
	 * @return 노선 출력
	 */
	public String search(String start,String end) throws SubwayException{
		Station startStation = getStation(start);
		Station endStation = getStation(end);
		return search(startStation,endStation);
	}
	/**
	 * 역 탐색 (다형성)
	 * @param start 출발 역정보
	 * @param end 도착 역정보
	 * @return 노선 출력
	 */
	public String search(Station start,Station end) throws SubwayException{
		//모든 탐색 정보
		ArrayList<ArrayList<Station>> list = new ArrayList<ArrayList<Station>>();
		//탐색용 버퍼
		Stack<Station> buffer = new Stack<Station>();
		//역코드로 역을 탐색할때 없으면 에러처리
		if(getStation(start.getCode()) == null) {
			throw new SubwayException("Not Station");
		}
		//역코드로 역을 탐색할때 없으면 에러처리
		if(getStation(end.getCode()) == null) {
			throw new SubwayException("Not Station");
		}
		//경로 탐색
		nodeExplorer(start, end, buffer, list);
		
		//출역
		String ret = "";
		int index = 0;
		int size = 999999;
		//노드가 가장 적은 역이 어떤건지 찾음(최단 탐색)
		for(int i=0;i<list.size();i++){
			if(list.get(i).size() < size){
				size = list.get(i).size();
				index = i;
			}
		}
		//모든 경로를 출력한다.
		for(ArrayList<Station> item : list){
			ret += print(item);
		}
		ret += "\r\n\r\n";
		//최단 경로를 출력한다.
		ret += "Best root\r\n";
		ret += print(list.get(index));
		return ret;
	}
	private String print(ArrayList<Station> item){
		StringBuffer sb =new StringBuffer(); 
		sb.append("Size : "+item.size()+"**");
		for(Station s:item){
			if(sb.length() > 0){
				sb.append("->");
			}
			sb.append(s.toString());
		}
		sb.append("\r\n");
		return sb.toString();
	}
	/**
	 * 노드 탐색(재귀적으로 탐색한다.)
	 * @param point 현재 탐색 역
	 * @param end 종착역
	 * @param buffer 버퍼
	 * @param list 노드리스트
	 * 
	 * 재귀의 구조
	 * 도봉에서 석계을 간다고 가정할때 처음 도봉의 전역,다음역으로 재귀를 부름
	 * 전역에서는 망월사 -> 회룡 -> 의정부 북부까지 갔는데 역이 없고 종착역을 만나지 못하면
	 * pop으로 다시 도봉까지 돌아옴
	 * 도봉역에서 방학-> 창동-> 노원 -> 상계 -> 당고개를 가지만 또 역이 없고 종착을 못만나면 
	 * pop으로 돌아오지만 분기가 노원에서 되었기 때문에
	 * 노원에서 쌍문-> 수유 등의 모든 경로를 탐색하게 됨
	 * 결국엔 석계역을 만날때까지 모든 경로를 탐색함. 
	 */
	private boolean nodeExplorer(
			Station point,
			Station end,
			Stack<Station> buffer,
			ArrayList<ArrayList<Station>> list){
		//탐색역과 종착역이 같으면 도착함
		if(point == end){
			//탐색노드 선언
			ArrayList<Station> root = new ArrayList<Station>();
			//노드 담기
			for(Station s:buffer){
				root.add(s);
			}
			//마지막 역 담기
			root.add(point);
			//리스트에 추가
			list.add(root);
			//종료
			return true;
		}
		//현재역이 없으면 재탐색
		if(point == null){
			return false;
		}
		//버퍼에 현재역 담기
		buffer.push(point);
		//현재역의 전역 개수만큼
		for (int i = 0; i < point.getPrevCount(); i++) {
			// 버퍼에 현재역이 있으면 돌아가기(지나간 역을 다시 지나가면 의미없음)
			// 예)종각에서 시청을 갔는데 시청에서 다시 종각으로 돌아가면 의미 없음
			if(buffer.contains(point.getPrev(i))){
				continue;
			}
			//없으면 전역으로 이동
			if(!nodeExplorer(point.getPrev(i), end, buffer, list)){
				//재탐색이 되면 현재역은 경로가 아님
				if(buffer.size() > 0) {
					buffer.pop();
				}
			}
		}
		//현재역의 다음역 개수만큼
		for (int i = 0; i < point.getNextCount(); i++) {
			// 버퍼에 현재역이 있으면 돌아가기(지나간 역을 다시 지나가면 의미없음)
			// 예)종각에서 시청을 갔는데 시청에서 다시 종각으로 돌아가면 의미 없음
			if(buffer.contains(point.getNext(i))) {
				continue;
			}
			if (!nodeExplorer(point.getNext(i), end, buffer, list)) {
				//재탐색이 되면 현재역은 경로가 아님
				if(buffer.size() > 0) {
					buffer.pop();
				}
			}
		}
		//재탐색
		return false;
	}
	public String toString() {
		String ret = "";
		for (Station s : stations) {
			ret += s.toString() + "\r\n";
		}
		return ret;
	}
}
