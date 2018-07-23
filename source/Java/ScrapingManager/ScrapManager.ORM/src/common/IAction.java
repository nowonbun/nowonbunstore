package common;

import com.datastax.driver.core.Session;

public interface IAction {
	void run(Session session);
}
