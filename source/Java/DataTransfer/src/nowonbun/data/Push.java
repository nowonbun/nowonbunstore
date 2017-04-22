package nowonbun.data;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.UUID;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.log4j.Logger;

@WebServlet("/Push")
public class Push extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(this.getClass());

	public Push() {
		super();
	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		logger.error(request.getRemoteHost());
		logger.error("response error code" + 406);
		response.sendError(406);
		//doPost(request, response);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		try {
			response.setHeader("Content-Type", "text/html;charset=UTF-8");
			String code = new SimpleDateFormat("yyyyMMddHHmmssSSS").format(Calendar.getInstance().getTime())
					+ UUID.randomUUID().toString();
			String data = request.getParameter("DATA");
			FileCenter.getInstance().crateFile(code, data);
			response.getWriter().println(code);
		} catch (Throwable e) {
			logger.error(request.getRemoteHost());
			logger.error("error");
			logger.error(e);
			throw e;
		}
	}

}
