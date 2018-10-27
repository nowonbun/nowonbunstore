package test.exam;

import java.util.ArrayList;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;
import test.entity.Test;

@Controller
public class Index {
	@RequestMapping(value = "/index.html")
	public String index(ModelMap modelmap, HttpSession session, HttpServletRequest req, HttpServletResponse res) {
		Test test = new Test();
		test.setData("Hello world");
		test.setCheck(true);
		test.setIterdata(new ArrayList<>());
		test.getIterdata().add("a");
		test.getIterdata().add("b");
		test.getIterdata().add("c");
		test.getIterdata().add("d");
		modelmap.addAttribute("test", test);
		return "index";
		// return "redirect:main.html";
	}
}
