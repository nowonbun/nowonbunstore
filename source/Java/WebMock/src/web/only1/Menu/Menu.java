package web.only1.Menu;

import javax.servlet.annotation.WebServlet;

import web.only1.Common.AbstractLoadServlect;
import web.only1.Common.View;
import web.only1.Common.ViewResult;

@WebServlet("/Menu")
public class Menu extends AbstractLoadServlect {

	private static final long serialVersionUID = 1L;
	//private final List<MenuItem> itemList = new ArrayList<>();

	public Menu() {
		super();
		// DEBUG
		//Menu_Test test = new Menu_Test();
		//itemList.addAll(test.getList());
	}

	@Override
	protected ViewResult execute() {
		return View.resource("Menu.html");
		/*StringBuffer sb = new StringBuffer();
		sb.append("<script src='./js/main/menu.js'> </script>");
		sb.append("<ul>");
		for (MenuItem item : itemList) {
			if (StringUtil.isEmpty(item.getWorkKey()) || StringUtil.isEmpty(item.getName())) {
				continue;
			}
			sb.append("<li>");
			if (!StringUtil.isEmpty(item.getUrl())) {
				sb.append("<a onclick=\"common.mainNavigate('").append(item.getName()).append("','")
						.append(item.getUrl()).append("')\">");
			}
			sb.append(item.getName());
			if (!StringUtil.isEmpty(item.getUrl())) {
				sb.append("</a>");
			}
			if (item.countChild() > 0) {
				Object[] childlist = item.getChild().stream().filter((i) -> {
					return !StringUtil.isEmpty(i.getWorkKey()) && !StringUtil.isEmpty(i.getName())
							&& !StringUtil.isEmpty(i.getUrl()) && Util.CheckCode(2097151, i.getAuth());
				}).map((i) -> {
					return "<li><a onclick=\"common.mainNavigate('"+i.getName()+"','"+i.getUrl()+"')\">"+i.getName()+"</a></li>";
				}).toArray();
				sb.append("<ul class='disp-off'>");
				for (Object i : childlist) {
					sb.append(i);
				}
				sb.append("</ul>");
			}
			sb.append("</li>");
		}
		sb.append("</ul>");
		return View.nativeCode(sb.toString());*/
	}
}
