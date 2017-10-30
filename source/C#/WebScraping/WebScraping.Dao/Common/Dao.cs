using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Linq;
using WebScraping.Dao.Interface;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Common
{
    abstract class Dao<T> : IDao
    {
        private MySqlCommand cmd;
        private bool isTransaction = false;
        private Dictionary<Type, Dictionary<String, FieldInfo>> flyweight = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        public Dao()
        {
            cmd = new MySqlCommand();
            cmd.Connection = new MySqlConnection();
        }
        public void SetConnectionString(String connectString)
        {
            cmd.Connection.ConnectionString = connectString;
        }
        public MySqlCommand Commander
        {
            get
            {
                return cmd;
            }
        }
        protected MySqlParameter CreateParameter(String name, Object value, MySqlDbType type)
        {
            MySqlParameter ret = new MySqlParameter();
            ret.ParameterName = name;
            ret.MySqlDbType = type;
            ret.Value = value;
            return ret;
        }
        protected IList<MySqlParameter> SetParameter(T entity)
        {
            IList<MySqlParameter> ret = new List<MySqlParameter>();
            List<FieldInfo> fds = GetFieldsInfo(typeof(T));
            foreach (FieldInfo fd in fds)
            {
                Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                String columnName = String.Concat("@", cn.ColumnName);
                ret.Add(this.CreateParameter(columnName, fd.GetValue(entity), cn.ColumnType));
            }
            return ret;
        }
        protected String CreateInsertQuery()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder value = new StringBuilder();
            Boolean c = true;
            Type type = typeof(T);
            Table table = type.GetCustomAttribute(typeof(Table)) as Table;
            List<FieldInfo> fds = GetFieldsInfo(type);
            sb = sb.Append(" insert ").Append(" into ").Append(table.TableName).Append(" (");
            value = value.Append(" values (");

            foreach (FieldInfo fd in fds)
            {
                Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                String columnName = String.Concat("@", cn.ColumnName);
                if (c)
                {
                    c = false;
                }
                else
                {
                    sb = sb.Append(",");
                    value = value.Append(",");
                }
                sb = sb.Append(cn.ColumnName);
                value = value.Append(columnName);
            }
            sb = sb.Append(")");
            value.Append(")");
            sb.Append(value);
            return sb.ToString();
        }
        protected int InsertByEntity(T entity, Boolean scope = false)
        {
            return this.Transaction(() =>
            {
                if (scope)
                {
                    return this.ExcuteReader(String.Concat(CreateInsertQuery(), ";select scope_identity();"), SetParameter(entity), dr =>
                    {
                        return Convert.ToInt32(dr.GetDecimal(0));
                    });
                }
                else
                {
                    return this.ExcuteNonReader(CreateInsertQuery(), SetParameter(entity));
                }
            });
        }
        protected String CreateUpdateQuery()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder where = new StringBuilder();
            StringBuilder set = new StringBuilder();
            Type type = typeof(T);
            Table table = type.GetCustomAttribute(typeof(Table)) as Table;
            IList<FieldInfo> fds = GetFieldsInfo(type);
            foreach (FieldInfo fd in fds)
            {
                Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                String columnName = String.Concat("@", cn.ColumnName);
                if (cn.Key)
                {
                    if (where.Length == 0)
                    {
                        where.Append(" WHERE ");
                    }
                    else
                    {
                        where.Append(" AND ");
                    }
                    where = where.Append(" ").Append(cn.ColumnName).Append(" = ").Append(columnName);
                }
                else
                {
                    if (set.Length == 0)
                    {
                        set.Append(" SET ");
                    }
                    else
                    {
                        set.Append(" , ");
                    }
                    set = set.Append(" ").Append(cn.ColumnName).Append(" = ").Append(columnName);
                }
            }
            sb.Append(" UPDATE ").Append(table.TableName).Append(" ");
            sb.Append(set);
            sb.Append(where);
            return sb.ToString();
        }
        protected int UpdateByEntity(T entity)
        {
            return this.Transaction(() =>
            {
                return this.ExcuteNonReader(CreateUpdateQuery(), SetParameter(entity));
            });
        }
        protected String CreateDeleteQuery()
        {
            Type type = typeof(T);
            Table table = type.GetCustomAttribute(typeof(Table)) as Table;
            StringBuilder sb = new StringBuilder();
            StringBuilder where = new StringBuilder();
            IList<MySqlParameter> parameter = new List<MySqlParameter>();
            List<FieldInfo> fds = GetFieldsInfo(type);

            sb.Append(" DELETE FROM ");
            sb.Append(table.TableName);

            foreach (FieldInfo fd in fds)
            {
                Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                String columnName = String.Concat("@", cn.ColumnName);
                if (where.Length == 0)
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }
                where = where.Append(" ").Append(cn.ColumnName);
                where = where.Append(" = ");
                where = where.Append(columnName);
            }
            sb.Append(where);
            return sb.ToString();
        }
        protected int DeleteByEntity(T entity, bool notflag = false)
        {
            return Transaction(() =>
            {
                return this.ExcuteNonReader(CreateDeleteQuery(), SetParameter(entity));
            });
        }
        [Obsolete("Not used", true)]
        protected String CreateSelectQuery()
        {
            Type type = typeof(T);
            Table table = type.GetCustomAttribute(typeof(Table)) as Table;
            StringBuilder sb = new StringBuilder();
            StringBuilder where = new StringBuilder();
            List<FieldInfo> fds = GetFieldsInfo(type);

            foreach (FieldInfo fd in fds)
            {
                Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                if (!cn.Key)
                {
                    continue;
                }
                String columnName = "@" + cn.ColumnName;
                if (where.Length == 0)
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                where = where.Append(" ").Append(cn.ColumnName);
                where = where.Append(" = ");
                where = where.Append(columnName);
            }
            sb.Append(where);
            return sb.ToString();
        }
        [Obsolete("Not used", true)]
        protected T SelectByEntity(T entity)
        {
            return this.Transaction(() =>
            {
                IList<MySqlParameter> parameter = new List<MySqlParameter>();
                List<FieldInfo> fds = GetFieldsInfo(typeof(T));
                foreach (FieldInfo fd in fds)
                {
                    Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                    if (!cn.Key)
                    {
                        continue;
                    }
                    String columnName = String.Concat("@", cn.ColumnName);
                    parameter.Add(this.CreateParameter(columnName, fd.GetValue(entity), cn.ColumnType));
                }
                T ret = (T)Activator.CreateInstance(typeof(T));
                this.ExcuteReader(CreateSelectQuery(), parameter, (dr) =>
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (dr.IsDBNull(i))
                        {
                            continue;
                        }
                        String columnName = dr.GetName(i);
                        GetFieldInfo(ret, columnName).SetValue(ret, dr.GetValue(i));
                    }
                });
                return ret;
            });
        }
        protected IList<T> SelectAll()
        {
            Type type = typeof(T);
            Table table = type.GetCustomAttribute(typeof(Table)) as Table;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" * ");
            sb.Append(" FROM ");
            sb.Append(table.TableName);
            return this.Transaction(() =>
            {
                IList<T> ret = new List<T>();
                this.ExcuteReader(sb.ToString(), null, (dr) =>
                {
                    ret.Add(SetClass<T>(dr));
                });
                return ret;
            });
        }
        protected R SetClass<R>(MySqlDataReader dr)
        {
            R ret = (R)Activator.CreateInstance(typeof(R));
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.IsDBNull(i))
                {
                    continue;
                }
                String columnName = dr.GetName(i);
                GetFieldInfo(ret, columnName).SetValue(ret, dr.GetValue(i));
            }
            return ret;
        }
        private List<FieldInfo> GetFieldsInfo(Type cls)
        {
            List<FieldInfo> ret = new List<FieldInfo>();
            FieldInfo[] pis = cls.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo pi in pis)
            {
                Column c = pi.GetCustomAttribute(typeof(Column)) as Column;
                if (c == null)
                {
                    continue;
                }
                if (c.Identity)
                {
                    continue;
                }
                ret.Add(pi);
            }
            return ret;
        }
        private FieldInfo GetFieldInfo(Object cls, String columnName)
        {
            if (!flyweight.ContainsKey(cls.GetType()))
            {
                flyweight.Add(cls.GetType(), new Dictionary<string, FieldInfo>());
            }
            Dictionary<string, FieldInfo> subFlyweight = flyweight[cls.GetType()];
            if (!subFlyweight.ContainsKey(columnName))
            {
                FieldInfo pi = cls.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).AsParallel().First(item =>
                {
                    Column cn = item.GetCustomAttribute(typeof(Column)) as Column;
                    if (Object.Equals(columnName.ToUpper(), cn.ColumnName.ToUpper()))
                    {
                        return true;
                    }
                    return false;
                });
                subFlyweight.Add(columnName, pi);
            }
            return subFlyweight[columnName];
        }
        protected IList<T> Transaction(Func<IList<T>> action)
        {
            IList<T> ret = null;
            cmd.Parameters.Clear();
            try
            {
                cmd.Connection.Open();
                try
                {
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    isTransaction = true;
                    ret = action();
                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    cmd.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    isTransaction = false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ret;
        }
        protected T Transaction(Func<T> action)
        {
            T ret = default(T);
            cmd.Parameters.Clear();
            try
            {
                cmd.Connection.Open();
                try
                {
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    isTransaction = true;
                    ret = action();
                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    cmd.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    isTransaction = false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ret;
        }
        protected Object Transaction(Func<Object> action)
        {
            Object ret = null;
            cmd.Parameters.Clear();
            try
            {
                cmd.Connection.Open();
                try
                {
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    isTransaction = true;
                    ret = action();
                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    cmd.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    isTransaction = false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ret;
        }
        protected int Transaction(Func<int> action)
        {
            int ret = -1;
            cmd.Parameters.Clear();
            try
            {
                cmd.Connection.Open();
                try
                {
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    isTransaction = true;
                    ret = action();
                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    cmd.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    isTransaction = false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ret;
        }
        protected void Transaction(Action action)
        {
            cmd.Parameters.Clear();
            try
            {
                cmd.Connection.Open();
                try
                {
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    isTransaction = true;
                    action();
                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    cmd.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    isTransaction = false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        protected int ExcuteReader(String query, IList<MySqlParameter> parameter, Action<MySqlDataReader> dataReader)
        {
            if (!isTransaction)
            {
                throw new Exception("not transaction");
            }
            int count = 0;
            cmd.CommandText = query;
            if (parameter != null)
            {
                cmd.Parameters.AddRange(parameter.ToArray());
            }
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    dataReader(dr);
                    count++;
                }
            }
            return count;
        }
        protected int ExcuteReader(String query, IList<MySqlParameter> parameter, Func<MySqlDataReader, int> dataReader)
        {
            if (!isTransaction)
            {
                throw new Exception("not transaction");
            }
            int count = 0;
            cmd.CommandText = query;
            if (parameter != null)
            {
                cmd.Parameters.AddRange(parameter.ToArray());
            }
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    count += dataReader(dr);
                }
            }
            return count;
        }
        protected int ExcuteNonReader(String query, IList<MySqlParameter> parameter)
        {
            if (!isTransaction)
            {
                throw new Exception("not transaction");
            }
            cmd.CommandText = query;
            if (parameter != null)
            {
                cmd.Parameters.AddRange(parameter.ToArray());
            }
            return cmd.ExecuteNonQuery();
        }
    }
}
