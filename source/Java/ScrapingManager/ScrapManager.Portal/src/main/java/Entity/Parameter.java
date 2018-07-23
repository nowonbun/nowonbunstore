package Entity;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.List;

import javax.json.Json;
import javax.json.JsonObjectBuilder;
import Broker.BrokerUnit;
import entity.KeyNode;
import entity.RequestData;

public class Parameter extends RequestData implements Cloneable {

	public Parameter(int mallkey, String key) {
		super(mallkey, key);
		super.setCreatedDate(new Date());
	}

	private BrokerUnit broker;
	private Date ping;

	public BrokerUnit getBroker() {
		return broker;
	}

	public void setBroker(BrokerUnit broker) {
		this.broker = broker;
	}

	public Date getPing() {
		return ping;
	}

	public void setPing(Date ping) {
		this.ping = ping;
	}

	public String toJson() {
		JsonObjectBuilder builder = Json.createObjectBuilder();
		addJson(builder, "Key", getKey());
		addJson(builder, "ApiKey", getApikey());
		addJson(builder, "MallCD", getMallKey());
		addJson(builder, "Id1", getId1());
		addJson(builder, "Id2", getId2());
		addJson(builder, "Id3", getId3());
		addJson(builder, "Pw1", getPw1());
		addJson(builder, "Pw2", getPw2());
		addJson(builder, "Pw3", getPw3());
		addJson(builder, "Option1", getOption1());
		addJson(builder, "Option2", getOption2());
		addJson(builder, "Option3", getOption3());
		addJson(builder, "ScrapType", getScraptype());
		addJson(builder, "Sdate", getSdate());
		addJson(builder, "Edate", getEdate());
		addJson(builder, "Exec", getExec());
		return builder.build().toString();
	}

	private JsonObjectBuilder addJson(JsonObjectBuilder builder, String name, int value) {
		builder.add(name, value);
		return builder;
	}

	private JsonObjectBuilder addJson(JsonObjectBuilder builder, String name, String value) {
		if (value == null) {
			builder.addNull(name);
		} else {
			builder.add(name, value);
		}
		return builder;
	}

	public Parameter clone(int mallkey, String key) {
		try {
			Parameter clonedata = (Parameter) super.clone();
			setField(clonedata, "mallkey", mallkey);
			setField(clonedata, "key", key);
			return clonedata;
		} catch (CloneNotSupportedException e) {
			throw new RuntimeException(e);
		}
	}

	private void setField(Object obj, String name, Object value) {
		try {
			Field field = getAllFields(Parameter.class).stream().filter(x -> x.getName().equals(name)).findFirst().get();
			field.setAccessible(true);
			field.set(obj, value);
		} catch (IllegalAccessException e) {
			throw new RuntimeException(e);
		}
	}

	private List<Field> getAllFields(Class<?> type) {
		List<Field> fields = new ArrayList<>();
		getAllFields(fields, type);
		return fields;
	}

	private List<Field> getAllFields(List<Field> fields, Class<?> type) {
		if (type.getSuperclass() != null) {
			getAllFields(fields, type.getSuperclass());
		}
		fields.addAll(Arrays.asList(type.getDeclaredFields()));
		return fields;
	}

	public KeyNode getKeyNode() {
		KeyNode node = new KeyNode(getKey());
		node.setMallkey(getMallKey());
		return node;
	}
}
