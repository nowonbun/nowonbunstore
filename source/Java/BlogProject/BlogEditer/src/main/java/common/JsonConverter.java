package common;

import java.io.StringReader;
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.math.BigInteger;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.Vector;
import javax.json.Json;
import javax.json.JsonArray;
import javax.json.JsonArrayBuilder;
import javax.json.JsonObject;
import javax.json.JsonObjectBuilder;
import javax.json.JsonReader;
import javax.json.stream.JsonParsingException;
import common.interfaces.ActionExpression;
import common.interfaces.LambdaExpression;

public class JsonConverter {
	public static String create(Object obj) {
		try {
			if (instance == null) {
				instance = new JsonConverter();
			}
			return instance.createJson(obj);
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public static void parseArray(String json, ActionExpression<JsonArray> func) {
		try (JsonReader jsonReader = Json.createReader(new StringReader(json))) {
			func.run(jsonReader.readArray());
		} catch (JsonParsingException e) {
			throw new RuntimeException(e);
		}
	}

	public static <T> T parseArray(String json, LambdaExpression<JsonArray, T> func) {
		try (JsonReader jsonReader = Json.createReader(new StringReader(json))) {
			return func.run(jsonReader.readArray());
		} catch (JsonParsingException e) {
			throw new RuntimeException(e);
		}
	}

	public static void parseObject(String json, ActionExpression<JsonObject> func) {
		try (JsonReader jsonReader = Json.createReader(new StringReader(json))) {
			func.run(jsonReader.readObject());
		} catch (JsonParsingException e) {
			throw new RuntimeException(e);
		}
	}

	public static <T> T parseObject(String json, LambdaExpression<JsonObject, T> func) {
		try (JsonReader jsonReader = Json.createReader(new StringReader(json))) {
			return func.run(jsonReader.readObject());
		} catch (JsonParsingException e) {
			throw new RuntimeException(e);
		}
	}

	private static JsonConverter instance = null;

	private JsonConverter() {

	}

	private enum TYPE {
		NULL, OBJECT, LIST, MAP, STATIC
	}

	private String createJson(Object obj) {
		try {
			TYPE type = check(obj);
			if (type == TYPE.NULL) {
				return null;
			} else if (type == TYPE.LIST) {
				return ((JsonArrayBuilder) trigger(obj)).build().toString();
			} else if (type == TYPE.OBJECT) {
				return ((JsonObjectBuilder) trigger(obj)).build().toString();
			} else if (type == TYPE.MAP) {
				return ((JsonObjectBuilder) trigger(obj)).build().toString();
			} else {
				return obj.toString();
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	private TYPE check(Object obj) {
		if (obj == null) {
			return TYPE.NULL;
		} else if (obj instanceof BigDecimal) {
			return TYPE.STATIC;
		} else if (obj instanceof BigInteger) {
			return TYPE.STATIC;
		} else if (obj instanceof Boolean) {
			return TYPE.STATIC;
		} else if (obj instanceof Double) {
			return TYPE.STATIC;
		} else if (obj instanceof Integer) {
			return TYPE.STATIC;
		} else if (obj instanceof Long) {
			return TYPE.STATIC;
		} else if (obj instanceof String) {
			return TYPE.STATIC;
		} else if (obj instanceof Map) {
			return TYPE.MAP;
		} else if (obj instanceof List) {
			return TYPE.LIST;
		} else if (obj instanceof Vector) {
			return TYPE.LIST;
		} else if (obj.getClass().isArray()) {
			return TYPE.LIST;
		}
		return TYPE.OBJECT;
	}

	private Object trigger(Object obj) {
		TYPE type = check(obj);
		if (type == TYPE.NULL) {
			return null;
		} else if (type == TYPE.OBJECT) {
			return buildObject(obj);
		} else if (type == TYPE.STATIC) {
			return obj;
		} else if (type == TYPE.MAP) {
			return buildMap(obj);
		} else if (type == TYPE.LIST) {
			return buildList(obj);
		}
		throw new RuntimeException();
	}

	private void add(JsonArrayBuilder builder, Object v) {
		TYPE type = check(v);
		if (type == TYPE.NULL) {
			builder.addNull();
		} else if (check(v) == TYPE.MAP) {
			builder.add((JsonObjectBuilder) trigger(v));
		} else if (check(v) == TYPE.STATIC) {
			addStatic(builder, v);
		} else if (check(v) == TYPE.LIST) {
			builder.add((JsonArrayBuilder) trigger(v));
		} else {
			builder.add((JsonObjectBuilder) trigger(v));
		}
	}

	private void add(JsonObjectBuilder builder, String name, Object v) {
		TYPE type = check(v);
		if (type == TYPE.NULL) {
			builder.addNull(name);
		} else if (check(v) == TYPE.MAP) {
			builder.add(name, (JsonObjectBuilder) trigger(v));
		} else if (check(v) == TYPE.STATIC) {
			addStatic(builder, name, v);
		} else if (check(v) == TYPE.LIST) {
			builder.add(name, (JsonArrayBuilder) trigger(v));
		} else {
			builder.add(name, (JsonObjectBuilder) trigger(v));
		}
	}

	private void addStatic(JsonArrayBuilder array, Object obj) {
		if (obj instanceof BigDecimal) {
			array.add((BigDecimal) obj);
		} else if (obj instanceof BigInteger) {
			array.add((BigInteger) obj);
		} else if (obj instanceof Boolean) {
			array.add((Boolean) obj);
		} else if (obj instanceof Double) {
			array.add((Double) obj);
		} else if (obj instanceof Integer) {
			array.add((Integer) obj);
		} else if (obj instanceof Long) {
			array.add((Long) obj);
		} else if (obj instanceof String) {
			array.add((String) obj);
		}
	}

	private void addStatic(JsonObjectBuilder builder, String name, Object obj) {
		if (obj instanceof BigDecimal) {
			builder.add(name, (BigDecimal) obj);
		} else if (obj instanceof BigInteger) {
			builder.add(name, (BigInteger) obj);
		} else if (obj instanceof Boolean) {
			builder.add(name, (Boolean) obj);
		} else if (obj instanceof Double) {
			builder.add(name, (Double) obj);
		} else if (obj instanceof Integer) {
			builder.add(name, (Integer) obj);
		} else if (obj instanceof Long) {
			builder.add(name, (Long) obj);
		} else if (obj instanceof String) {
			builder.add(name, (String) obj);
		}
	}

	private JsonArrayBuilder buildList(Object obj) {
		JsonArrayBuilder list = Json.createArrayBuilder();
		if (obj instanceof List) {
			List<?> temp = (List<?>) obj;
			for (Object v : temp) {
				add(list, v);
			}
			return list;
		} else if (obj instanceof Vector) {
			Vector<?> temp = (Vector<?>) obj;
			for (Object v : temp) {
				add(list, v);
			}
			return list;
		} else if (obj.getClass().isArray()) {
			for (Object v : Arrays.asList(obj)) {
				add(list, v);
			}
			return list;
		}
		throw new RuntimeException();
	}

	private JsonObjectBuilder buildMap(Object obj) {
		if (obj instanceof Map) {
			JsonObjectBuilder map = Json.createObjectBuilder();
			Map<?, ?> temp = (Map<?, ?>) obj;
			for (Object key : temp.keySet()) {
				add(map, key.toString(), temp.get(key));
			}
			return map;
		}
		throw new RuntimeException();
	}

	private JsonObjectBuilder buildObject(Object obj) {
		try {
			JsonObjectBuilder builder = Json.createObjectBuilder();

			Class<?> clz = obj.getClass();
			for (Field field : clz.getDeclaredFields()) {
				field.setAccessible(true);
				if ((field.getModifiers() & Modifier.STATIC) != 0) {
					continue;
				}
				add(builder, field.getName(), field.get(obj));
			}
			return builder;
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public static boolean JsonStringIsEmptyOrNull(JsonObject jsonobj, String key) {
		return !JsonIsKey(jsonobj, key) || Util.StringIsEmptyOrNull(jsonobj.getString(key));
	}

	public static boolean JsonIsKey(JsonObject jsonobj, String key) {
		return jsonobj.keySet().contains(key) && !jsonobj.isNull(key);
	}

	public static String JsonString(JsonObject jsonobj, String key) {
		if (JsonIsKey(jsonobj, key)) {
			return jsonobj.getString(key);
		}
		return null;
	}

	public static boolean JsonBoolean(JsonObject jsonobj, String key) {
		if (JsonIsKey(jsonobj, key)) {
			return jsonobj.getBoolean(key);
		}
		return false;
	}

	public static byte[] JsonBytes(JsonObject jsonobj, String key) {
		String ret = JsonString(jsonobj, key);
		if (ret == null) {
			return null;
		}
		return ret.getBytes();
	}

	public static int JsonInteger(JsonObject jsonobj, String key) {
		if (JsonIsKey(jsonobj, key)) {
			return jsonobj.getInt(key);
		}
		return 0;
	}
}
