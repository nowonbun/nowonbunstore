package dao;

import java.util.List;

import common.IDao;
import entity.Broker;
import entity.PingPong;

public class PingPongDao extends IDao<PingPong> {

	protected PingPongDao() {
		super();
	}

	@Override
	protected Class<PingPong> setClassType() {
		return PingPong.class;
	}

	public PingPong selectByBroker(Broker broker) {
		PingPong where = new PingPong(broker.getKey());
		List<PingPong> ret = super.select(where);
		if (ret.size() > 0) {
			return ret.get(0);
		}
		return null;
	}
}