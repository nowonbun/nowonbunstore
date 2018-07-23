package common;

import com.datastax.driver.core.Session;

public interface IFunc<T> {
	T run(Session session);
}
