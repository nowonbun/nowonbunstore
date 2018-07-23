package common;

import java.io.Closeable;
import java.io.IOException;

import com.datastax.driver.core.Cluster;
import com.datastax.driver.core.Session;
import com.datastax.driver.core.policies.RoundRobinPolicy;

public class Service implements Closeable {
	private static Service instance = null;

	private String CLUSTER_NAME = "192.168.111.210";
	private String ID = "root";
	private String PW = "dhsfldnjs1";
	private String KEYSPACE = "scraping";

	private static Cluster cluster = null;

	private Cluster getCluster() {
		if (cluster == null) {
			cluster = Cluster.builder().addContactPoint(CLUSTER_NAME).withCredentials(ID, PW)
					.withLoadBalancingPolicy(new RoundRobinPolicy()).build();
		}
		return cluster;
	}

	private Session getSession(Cluster cluster) {
		return cluster.connect(KEYSPACE);
	}

	public static void newInstance(String cn, String id, String pw, String ks) {
		instance = new Service();
		instance.CLUSTER_NAME = cn;
		instance.ID = id;
		instance.PW = pw;
		instance.KEYSPACE = ks;
	}

	public static String getKeyspace() {
		return instance.KEYSPACE;
	}

	public <T> T init(IFunc<T> func) {
		Session session = null;
		try {
			session = getSession(getCluster());
			return func.run(session);
		} finally {
			if (session != null) {
				session.close();
			}
		}
	}

	public void init(IAction func) {
		Session session = null;
		try {
			session = getSession(getCluster());
			func.run(session);
		} finally {
			if (session != null) {
				session.close();
			}
		}
	}

	public static <T> T Transaction(IFunc<T> func) {
		return instance.init(func);
	}

	public static void Transaction(IAction func) {
		instance.init(func);
	}

	@Override
	public void close() throws IOException {
		cluster.close();
		cluster = null;
	}
}
