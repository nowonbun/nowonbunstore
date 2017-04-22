package common;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.math.BigInteger;
import java.util.List;
import java.util.Map;

import javax.json.Json;
import javax.json.JsonArrayBuilder;
import javax.json.JsonObjectBuilder;

public class JsonConverter {
	public static String create(Object obj) {
		try {
			return createJson(obj);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	private static String createJson(Object obj) {
		try {
			if (obj instanceof List) {
				JsonArrayBuilder list = Json.createArrayBuilder();
				List<?> temp = (List<?>) obj;
				for (Object val : temp) {
					if (check(val)) {
						transAdd(list, val);
					} else {
						list.add(build(val));
					}
				}
				return list.build().toString();
			} else if (obj instanceof Map) {
				JsonObjectBuilder map = Json.createObjectBuilder();
				Map<?, ?> temp = (Map<?, ?>) obj;
				for (Object key : temp.keySet()) {
					Object val = temp.get(key);
					if (check(val)) {
						transAdd(map, key.toString(), val);
					} else {
						map.add(key.toString(), build(temp.get(key)));
					}
				}
				return map.build().toString();
			} else if (obj.getClass().isArray()) {
				JsonArrayBuilder list = Json.createArrayBuilder();
				Object[] temp = (Object[]) obj;
				for (Object val : temp) {
					if (check(val)) {
						transAdd(list, val);
					} else {
						list.add(build(val));
					}
				}
				return list.build().toString();
			} else {
				return build(obj).toString();
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	private static void transAdd(JsonArrayBuilder list, Object val) {
		if (val instanceof BigDecimal) {
			list.add((BigDecimal) val);
			return;
		} else if (val instanceof BigInteger) {
			list.add((BigInteger) val);
			return;
		} else if (val instanceof Boolean) {
			list.add((Boolean) val);
			return;
		} else if (val instanceof Double) {
			list.add((Double) val);
			return;
		} else if (val instanceof Integer) {
			list.add((Integer) val);
			return;
		} else if (val instanceof Long) {
			list.add((Long) val);
			return;
		} else if (val instanceof String) {
			list.add((String) val);
			return;
		}
		throw new RuntimeException("not format");
	}

	private static void transAdd(JsonObjectBuilder map, String key, Object val) {
		if (val instanceof BigDecimal) {
			map.add(key, (BigDecimal) val);
			return;
		} else if (val instanceof BigInteger) {
			map.add(key, (BigInteger) val);
			return;
		} else if (val instanceof Boolean) {
			map.add(key, (Boolean) val);
			return;
		} else if (val instanceof Double) {
			map.add(key, (Double) val);
			return;
		} else if (val instanceof Integer) {
			map.add(key, (Integer) val);
			return;
		} else if (val instanceof Long) {
			map.add(key, (Long) val);
			return;
		} else if (val instanceof String) {
			map.add(key, (String) val);
			return;
		}
		throw new RuntimeException("not format");
	}

	private static boolean check(Object obj) {
		if (obj instanceof BigDecimal) {
			return true;
		} else if (obj instanceof BigInteger) {
			return true;
		} else if (obj instanceof Boolean) {
			return true;
		} else if (obj instanceof Double) {
			return true;
		} else if (obj instanceof Integer) {
			return true;
		} else if (obj instanceof Long) {
			return true;
		} else if (obj instanceof String) {
			return true;
		}
		return false;
	}

	private static JsonObjectBuilder build(Object obj) {
		try {
			JsonObjectBuilder builder = Json.createObjectBuilder();
			Class<?> clz = obj.getClass();
			for (Field field : clz.getDeclaredFields()) {
				field.setAccessible(true);
				if ((field.getModifiers() & Modifier.STATIC) != 0) {
					continue;
				}
				Object val = field.get(obj);
				if (val == null) {
					builder.addNull(field.getName());
				} else if (val instanceof List) {
					JsonArrayBuilder list = Json.createArrayBuilder();
					List<?> temp = (List<?>) val;
					for (Object v : temp) {
						list.add(build(v));
					}
					builder.add(field.getName(), list);
				} else if (val instanceof Map) {
					JsonObjectBuilder map = Json.createObjectBuilder();
					Map<?, ?> temp = (Map<?, ?>) val;
					for (Object key : temp.keySet()) {
						map.add(key.toString(), build(temp.get(key)));
					}
					builder.add(field.getName(), map);
				} else if (obj.getClass().isArray()) {
					JsonArrayBuilder list = Json.createArrayBuilder();
					Object[] temp = (Object[]) obj;
					for (Object v : temp) {
						list.add(build(v));
					}
					builder.add(field.getName(), list);
				} else if (val instanceof BigDecimal) {
					transAdd(builder, field.getName(), val);
				} else if (val instanceof BigInteger) {
					transAdd(builder, field.getName(), val);
				} else if (val instanceof Boolean) {
					transAdd(builder, field.getName(), val);
				} else if (val instanceof Double) {
					transAdd(builder, field.getName(), val);
				} else if (val instanceof Integer) {
					transAdd(builder, field.getName(), val);
				} else if (val instanceof Long) {
					transAdd(builder, field.getName(), val);
				} else if (val instanceof String) {
					transAdd(builder, field.getName(), val);
				} else {
					builder.add(field.getName(), build(val));
				}
			}
			return builder;
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}
}
