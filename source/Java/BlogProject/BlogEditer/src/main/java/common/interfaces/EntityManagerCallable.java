package common.interfaces;

import javax.persistence.EntityManager;

public interface EntityManagerCallable<V> {
	V run(EntityManager em);
}
