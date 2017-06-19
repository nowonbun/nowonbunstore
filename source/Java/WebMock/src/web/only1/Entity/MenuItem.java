package web.only1.Entity;

import java.util.ArrayList;
import java.util.List;

public class MenuItem {
	private String workKey;
	private String url;
	private String name;
	private int auth;
	private final List<MenuItem> child = new ArrayList<>();
	
	public String getWorkKey() {
		return workKey;
	}
	public void setWorkKey(String workKey) {
		this.workKey = workKey;
	}
	public String getUrl() {
		return url;
	}
	public void setUrl(String url) {
		this.url = url;
	}
	public int getAuth() {
		return auth;
	}
	public void setAuth(int auth) {
		this.auth = auth;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public boolean addChild(MenuItem item){
		return child.add(item);
	}
	public boolean removeChild(MenuItem item){
		return child.remove(item);
	}
	public MenuItem removeChild(int index){
		return child.remove(index);
	}
	public int countChild(){
		return child.size();
	}
	public List<MenuItem> getChild(){
		return child;
	}
	
}
