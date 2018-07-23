package dao;

import common.IDao;
import entity.Broker;

public class BrokerDao extends IDao<Broker> {

	protected BrokerDao() {
		super();
	}

	@Override
	protected Class<Broker> setClassType() {
		return Broker.class;
	}
}
