package common;

import java.io.InputStream;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;

public class LoggerManager {

	private static LoggerManager instance = null;

	public static Logger getLogger(Class<?> clazz) {
		if (instance == null) {
			instance = new LoggerManager();
		}
		return instance.get(clazz);
	}

	private LoggerManager() {
		try (InputStream stream = Thread.currentThread().getContextClassLoader().getResourceAsStream("log4j.xml")) {
			PropertyConfigurator.configure(stream);
		}catch(Throwable e){
			throw new RuntimeException(e);
		}
	}

	private Logger get(Class<?> clazz) {
		return Logger.getLogger(clazz);
	}
}
