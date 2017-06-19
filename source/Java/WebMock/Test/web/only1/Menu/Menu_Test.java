package web.only1.Menu;

import java.util.ArrayList;
import java.util.List;

import web.only1.Entity.MenuItem;

public class Menu_Test {
	
	public List<MenuItem> getList(){
		List<MenuItem> ret = new ArrayList<>();
		MenuItem a = createEntity("GSM00001","Menu","",1); 
		ret.add(a);
		a.addChild(createEntity("GSM00002","Menu1","./Main1",1));
		a.addChild(createEntity("GSM00003","Menu2","./Main2",1));
		a.addChild(createEntity("GSM00004","Menu3","./Main3",1));
		return ret;
	}
	private MenuItem createEntity(String workKey,String name,String url,int auth){
		MenuItem ret = new MenuItem();
		ret.setWorkKey(workKey);
		ret.setName(name);
		ret.setUrl(url);
		ret.setAuth(auth);
		return ret;
	}
}
