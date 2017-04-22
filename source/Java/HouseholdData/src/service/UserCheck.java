package service;

import java.util.Map;

import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;

@WebServlet("/UserCheck")
public class UserCheck extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	public Object execute(Map<String,String[]> parameter){
		String[] code = parameter.get("TEST");
		return code;
	}
}
