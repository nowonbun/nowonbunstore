using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using WebScraping.Dao.Interface;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Common
{
    abstract class Dao<T> : IDao
    {
        private MySqlCommand cmd;
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

        protected MySqlParameter CreateParameter(String paramName, Object paramObject)
        {
            return new MySqlParameter(paramName, paramObject);
        }
        protected int UpdateByEntity(T entity)
        {
            cmd.Parameters.Clear();
            StringBuilder sb = new StringBuilder();
            Type clsTp = typeof(T);
            Table table = clsTp.GetCustomAttribute(typeof(Table)) as Table;
            IList<FieldInfo> fields = GetWhere(entity);
            StringBuilder where = new StringBuilder();
            StringBuilder set = new StringBuilder();
            foreach (FieldInfo field in fields)
            {
                Column cn = field.GetCustomAttribute(typeof(Column)) as Column;
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
                    String temp = "@" + cn.ColumnName;
                    where = where.Append(" ").Append(cn.ColumnName).Append(" = ").Append(temp);
                    var param = new MySqlParameter(temp, cn.ColumnType);
                    param.Value = field.GetValue(entity);
                    cmd.Parameters.Add(param);
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
                    String temp = "@" + cn.ColumnName;
                    set = set.Append(" ").Append(cn.ColumnName).Append(" = ").Append(temp);
                    var param = new MySqlParameter(temp, cn.ColumnType);
                    param.Value = field.GetValue(entity);
                    cmd.Parameters.Add(param);
                }
            }
            sb.Append(" UPDATE ").Append(table.TableName).Append(" ");
            sb.Append(set);
            sb.Append(where);
            cmd.CommandText = sb.ToString();
            cmd.Connection.Open();
            try
            {
                return cmd.ExecuteNonQuery();
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
        protected int InsertByEntity(T entity, Boolean scope = false)
        {
            cmd.Parameters.Clear();
            StringBuilder sb = new StringBuilder();
            StringBuilder value = new StringBuilder();
            Type clsTp = typeof(T);
            Table table = clsTp.GetCustomAttribute(typeof(Table)) as Table;

            sb = sb.Append(" insert ").Append(" into ").Append(table.TableName).Append(" (");
            value = value.Append(" values (");
            if (entity == null)
            {
                throw new Exception("Not entity.");
            }
            List<FieldInfo> pis = GetWhere(entity);
            Boolean c = true;
            foreach (FieldInfo pi in pis)
            {
                if (c)
                {
                    c = false;
                }
                else
                {
                    sb = sb.Append(",");
                    value = value.Append(",");
                }
                Column cn = pi.GetCustomAttribute(typeof(Column)) as Column;
                sb = sb.Append(cn.ColumnName);
                String temp = "@" + cn.ColumnName;
                value = value.Append(temp);
                var param = new MySqlParameter(temp, cn.ColumnType);
                param.Value = pi.GetValue(entity);
                cmd.Parameters.Add(param);
            }
            sb = sb.Append(")");
            value.Append(")");
            sb.Append(value);
            if (scope)
            {
                sb = sb.Append(";");
                sb = sb.Append("select scope_identity();");
            }
            cmd.CommandText = sb.ToString();
            cmd.Connection.Open();
            try
            {
                if (scope)
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        return Convert.ToInt32(dr.GetDecimal(0));
                    }
                    throw new Exception("No applied data.");
                }
                return cmd.ExecuteNonQuery();
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


        protected List<Dictionary<String, Object>> Execute(String query, params MySqlParameter[] param)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = query;
            foreach (MySqlParameter p in param)
            {
                cmd.Parameters.Add(p);
            }
            cmd.Connection.Open();
            try
            {
                List<Dictionary<String, Object>> ret = new List<Dictionary<string, object>>();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Dictionary<String, Object> item = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        item.Add(dr.GetName(i), dr.GetValue(i));
                    }
                    ret.Add(item);
                }
                return ret;
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

        protected int ExecuteNotResult(String query, params MySqlParameter[] param)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = query;
            foreach (MySqlParameter p in param)
            {
                cmd.Parameters.Add(p);
            }
            cmd.Connection.Open();
            try
            {
                return cmd.ExecuteNonQuery();
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

        protected IList<T> Select(String query, params MySqlParameter[] param)
        {
            Type clsTp = typeof(T);
            IList<T> ret = new List<T>();
            cmd.Parameters.Clear();
            cmd.CommandText = query;
            foreach (MySqlParameter p in param)
            {
                cmd.Parameters.Add(p);
            }
            try
            {
                cmd.Connection.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T cls = (T)Activator.CreateInstance(clsTp);
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            if (dr.IsDBNull(i))
                            {
                                continue;
                            }
                            String columnName = dr.GetName(i);
                            GetField(cls, columnName).SetValue(cls, dr.GetValue(i));
                        }
                        ret.Add(cls);
                    }
                }
                return ret;
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

        protected IList<T> SelectByEntity(T entity, bool notflag = false)
        {
            cmd.Parameters.Clear();
            IList<T> ret = new List<T>();
            Type clsTp = typeof(T);
            Table table = clsTp.GetCustomAttribute(typeof(Table)) as Table;
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT ");
            sb.Append(" * ");
            sb.Append(" FROM ");
            sb.Append(table.TableName);
            if (entity != null)
            {
                List<FieldInfo> pis = GetWhere(entity);
                String where = SetParameter(entity, pis, cmd.Parameters, notflag);
                sb.Append(where);
            }
            cmd.CommandText = sb.ToString();
            try
            {
                cmd.Connection.Open();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        T cls = (T)Activator.CreateInstance(clsTp);
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            if (dr.IsDBNull(i))
                            {
                                continue;
                            }
                            String columnName = dr.GetName(i);
                            GetField(cls, columnName).SetValue(cls, dr.GetValue(i));
                        }
                        ret.Add(cls);
                    }
                }
                return ret;
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

        private String SetParameter(T entity, List<FieldInfo> pis, MySqlParameterCollection parameter, bool notflag)
        {
            StringBuilder where = new StringBuilder();

            foreach (FieldInfo pi in pis)
            {
                if (where.Length == 0)
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }
                Column cn = pi.GetCustomAttribute(typeof(Column)) as Column;
                String temp = "@" + cn.ColumnName;
                where = where.Append(" ").Append(cn.ColumnName);
                if (notflag)
                {
                    where = where.Append(" <> ");
                }
                else
                {
                    where = where.Append(" = ");
                }
                where = where.Append(temp);

                var param = new MySqlParameter(temp, cn.ColumnType);
                param.Value = pi.GetValue(entity);
                parameter.Add(param);
            }
            return where.ToString();
        }

        private List<FieldInfo> GetWhere(Object cls)
        {
            List<FieldInfo> ret = new List<FieldInfo>();
            Type clsType = cls.GetType();
            FieldInfo[] pis = clsType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
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
                Object data = pi.GetValue(cls);
                if (data != null)
                {
                    ret.Add(pi);
                }
            }
            return ret;
        }

        private FieldInfo GetField(Object cls, String columnName)
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
    }
}
