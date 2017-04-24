package service;

import java.util.Map;

import javax.annotation.Resource;
import javax.servlet.annotation.WebServlet;
import common.AbstractHttpServlet;
import dao.UsrNfDao;

@WebServlet("/UserCheck")
public class UserCheck extends AbstractHttpServlet {

	private static final long serialVersionUID = 1L;

	@Resource
	private UsrNfDao usrnFDao;

	public Object execute(Map<String, String[]> parameter) {
		
		String[] code = parameter.get("TEST");
		return code;
	}
}
