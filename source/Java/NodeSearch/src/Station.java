import java.util.ArrayList;

public class Station {
	//전역(복수가 될수 있다.환승역 예:동대문운동장)
	private final ArrayList<Station> prev;
	//다음역(복수가 될수 있다.환승역 예:동대문운동장)
	private final ArrayList<Station> next;
	//역코드(임의)
	private String code = null;
	//역이름
	private String name = null;
	//생성자
	public Station(){
		prev = new ArrayList<Station>();
		next = new ArrayList<Station>();
	}
	//이하 프로퍼티
	public void setCode(String code){
		this.code = code;
	}
	public String getCode(){
		return this.code;
	}
	public void setName(String name){
		this.name = name;
	}
	public String getName(){
		return this.name;
	}
	public Station getPrev(int index){
		return this.prev.get(index);
	}
	public void addPrev(Station value){
		if(!this.prev.contains(value)){
			this.prev.add(value);
		}
	}
	public Station getNext(int index){
		return this.next.get(index);
	}
	public void addNext(Station value){
		if(!this.next.contains(value)){
			this.next.add(value);
		}
	}
	public String toString(){
		return "[CODE]" + code + "[NAME]"+name;
	}
	public int getPrevCount(){
		return this.prev.size();
	}
	public int getNextCount(){
		return this.next.size();
	}
}
