package common;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import com.datastax.driver.core.ResultSet;
import com.datastax.driver.core.Row;

public abstract class IDao<T> {
	private Class<T> clzz;

	public interface Lambda1 {
		void run(ResultSet result);
	}

	public interface Lambda2<R> {
		R run(ResultSet result);
	}

	protected IDao() {
		this.clzz = setClassType();
	}

	protected abstract Class<T> setClassType();

	protected void query(Lambda1 expression, String query) {
		Service.Transaction((session) -> {
			expression.run(session.execute(query));
		});
	}

	protected void query(Lambda1 expression, String query, Object... args) {
		Service.Transaction((session) -> {
			expression.run(session.execute(query, args));
		});
	}

	protected <R> R query(Lambda2<R> expression, String query) {
		return Service.Transaction((session) -> {
			return expression.run(session.execute(query));
		});
	}

	protected <R> R query(Lambda2<R> expression, String query, Object... args) {
		return Service.Transaction((session) -> {
			return expression.run(session.execute(query, args));
		});
	}

	protected void query(String query) {
		Service.Transaction((session) -> {
			session.execute(query);
		});
	}

	protected void query(String query, Object... args) {
		Service.Transaction((session) -> {
			session.execute(query, args);
		});
	}

	public List<T> select() {
		return select(null);
	}

	public List<T> select(T where) {
		try {
			String tablename = clzz.getDeclaredAnnotation(Table.class).name();
			Field partitionkey = null;
			List<Field> field = new ArrayList<>();
			List<Field> entityTemplate = new ArrayList<>();
			List<Object> param = new ArrayList<>();
			List<Field> constructorparam = new ArrayList<>();
			for (Field f : getAllFields(clzz)) {
				Column column = f.getDeclaredAnnotation(Column.class);
				Object p;
				if (column == null) {
					continue;
				}
				f.setAccessible(true);
				entityTemplate.add(f);
				if (column.partitionkey()) {
					constructorparam.add(0, f);
				}
				if (column.key()) {
					constructorparam.add(f);
				}
				if (where == null) {
					continue;
				}
				if ((p = f.get(where)) == null) {
					continue;
				}
				if (column.partitionkey()) {
					partitionkey = f;
					param.add(0, p);
				} else if (column.key()) {
					field.add(f);
					param.add(p);
				} else {
					continue;
				}
			}
			if (where != null && partitionkey == null) {
				throw new RuntimeException("The entity is not setting the partition key.");
			}
			StringBuffer sb = new StringBuffer();
			sb.append(" SELECT * FROM ");
			sb.append(Service.getKeyspace());
			sb.append(".");
			sb.append(tablename);
			if (partitionkey != null) {
				sb.append(" WHERE ");
				sb.append(partitionkey.getName());
				sb.append(" = ? ");
				for (Field f : field) {
					sb.append(" AND ");
					sb.append(f.getName());
					sb.append(" = ? ");
				}
			}
			return query((result) -> {
				try {
					List<Row> rowlist = result.all();
					List<T> list = new ArrayList<>();
					List<Object> buffer = new ArrayList<>();
					List<Class<?>> buffer2 = new ArrayList<>();
					for (Row row : rowlist) {
						buffer.clear();
						buffer2.clear();
						for (Field f : constructorparam) {
							Column column = f.getDeclaredAnnotation(Column.class);
							buffer.add(row.get(column.name(), f.getType()));
							buffer2.add(f.getType());
						}
						Constructor<T> constructor = clzz.getDeclaredConstructor(buffer2.toArray(new Class<?>[0]));
						constructor.setAccessible(true);
						T entity = (T) constructor.newInstance(buffer.toArray());
						for (Field f : entityTemplate) {
							f.set(entity, row.get(f.getDeclaredAnnotation(Column.class).name(), f.getType()));
						}
						list.add(entity);
					}
					return list;
				} catch (Throwable e) {
					throw new RuntimeException(e);
				}
			}, sb.toString(), param.toArray());
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public void merge(T data) {
		try {
			String tablename = clzz.getDeclaredAnnotation(Table.class).name();
			List<Field> field = new ArrayList<>();
			List<Object> param = new ArrayList<>();
			for (Field f : getAllFields(clzz)) {
				if (f.getDeclaredAnnotation(Column.class) == null) {
					continue;
				}
				f.setAccessible(true);
				param.add(f.get(data));
				field.add(f);
			}
			StringBuffer sb = new StringBuffer();
			StringBuffer value = new StringBuffer();
			sb.append(" INSERT INTO ");
			sb.append(Service.getKeyspace());
			sb.append(".");
			sb.append(tablename);
			sb.append(" ( ");
			for (Field f : field) {
				if (value.length() > 0) {
					sb.append(" , ");
					value.append(" , ");
				} else {
					value.append(" ( ");
				}
				sb.append(f.getDeclaredAnnotation(Column.class).name());
				value.append(" ? ");
			}
			sb.append(" ) ");
			value.append(" ) ");

			this.query(sb.toString() + " values " + value.toString(), param.toArray());
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public void delete(T where) {
		try {
			String tablename = clzz.getDeclaredAnnotation(Table.class).name();
			Field partitionkey = null;
			List<Field> field = new ArrayList<>();
			List<Object> param = new ArrayList<>();
			if (where == null) {
				throw new RuntimeException("The parameter is null.");
			}
			for (Field f : getAllFields(clzz)) {
				Column column = f.getDeclaredAnnotation(Column.class);
				Object p;
				if (column == null) {
					continue;
				}
				f.setAccessible(true);
				if ((p = f.get(where)) == null) {
					continue;
				}
				if (column.partitionkey()) {
					partitionkey = f;
					param.add(0, p);
				} else if (column.key()) {
					field.add(f);
					param.add(p);
				} else {
					continue;
				}
			}
			if (partitionkey == null) {
				throw new RuntimeException("The entity is not setting the partition key.");
			}
			StringBuffer sb = new StringBuffer();
			sb.append(" DELETE FROM ");
			sb.append(Service.getKeyspace());
			sb.append(".");
			sb.append(tablename);
			sb.append(" WHERE ");
			sb.append(partitionkey.getName());
			sb.append(" = ? ");
			for (Field f : field) {
				sb.append(" AND ");
				sb.append(f.getName());
				sb.append(" = ?  ");
			}
			this.query(sb.toString(), param.toArray());
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}

	public void truncate() {
		String tablename = clzz.getDeclaredAnnotation(Table.class).name();
		StringBuffer sb = new StringBuffer();
		sb.append(" TRUNCATE  ");
		sb.append(Service.getKeyspace());
		sb.append(".");
		sb.append(tablename);
		this.query(sb.toString());
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
}
