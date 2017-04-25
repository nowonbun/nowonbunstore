package nowonbun.data;

import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.log4j.Logger;

@WebServlet("/Pop")
public class Pop extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private Logger logger = LoggerManager.getLogger(this.getClass());

	public Pop() {
		super();
	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		logger.info(request.getRemoteHost());
		logger.error("response error code : 406" );
		response.sendError(406);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		logger.info(request.getRemoteHost());
		try {
			request.setCharacterEncoding("UTF-8");
			response.setHeader("Content-Type", "text/html;charset=UTF-8");
			String code = request.getParameter("CODE");
			String data = FileCenter.getInstance().readFile(code);
			FileCenter.getInstance().moveFile(code);
			response.getWriter().print(data);
		} catch (Throwable e) {
			logger.error(request.getRemoteHost());
			logger.error("error");
			logger.error(e);
			throw e;
		}
	}
}
