using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Data.OleDb;
using PTM.ORM.Attribute;

namespace PTM.ORM.Common
{
    abstract class Dao<T> : IDao
    {
        private String tmpPath;
        private bool isTransaction = false;
        private Dictionary<String, FieldInfo> flyweight = new Dictionary<string, FieldInfo>();
        private OleDbCommand cmd;

        public Dao(OleDbConnection conn)
        {
            this.cmd = new OleDbCommand();
            cmd.Connection = conn;

            if (ExistsTable())
            {
                return;
            }

            Type entity = GetEntityType();
            Table table = entity.GetCustomAttribute(typeof(Table)) as Table;
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE TABLE ").Append(table.TableName).Append(" (");
            GetEntityType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList().ForEach(item =>
            {
                Column cn = item.GetCustomAttribute(typeof(Column)) as Column;
                flyweight.Add(cn.ColumnName, item);
                sb.Append(cn.ColumnName).Append(" ");
                //https://docs.microsoft.com/en-us/sql/odbc/microsoft/microsoft-access-data-types
                switch (cn.ColumnType)
                {
                    case OleDbType.Integer:
                        sb.Append(" int ");
                        break;
                    case OleDbType.VarChar:
                        if (cn.ColumnSize > 0)
                        {
                            sb.Append(" varchar(").Append(cn.ColumnSize).Append(")");
                        }
                        else
                        {
                            sb.Append(" text ");
                        }
                        break;
                    case OleDbType.Date:
                        sb.Append(" datetime ");
                        break;
                    default:
                        sb.Append(" text ");
                        break;
                }
                if (cn.Identity)
                {
                    sb.Append("identity ");
                }
                if (cn.Key)
                {
                    sb.Append(" primary key ");
                }
                sb.Append(",");
            });
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" )");
            this.Transaction(() =>
            {
                this.ExcuteNonReader(sb.ToString());
            });
        }
        public bool ExistsTable()
        {
            try
            {
                Type entity = GetEntityType();
                Table table = entity.GetCustomAttribute(typeof(Table)) as Table;
                cmd.Connection.Open();
                var map = cmd.Connection.GetSchema("Tables");
                for (int i = 0; i < map.Rows.Count; i++)
                {
                    if (table.TableName.Equals(map.Rows[i][2]))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        public void SetConnectionString(String connectString)
        {
            cmd.Connection.ConnectionString = connectString;
        }
        public void SetCsvPath(String tmpPath)
        {
            this.tmpPath = tmpPath;
        }
        public OleDbCommand Commander
        {
            get
            {
                return cmd;
            }
        }
        protected OleDbParameter CreateParameter(String name, Object value, OleDbType type)
        {
            OleDbParameter ret = new OleDbParameter();
            ret.ParameterName = name;
            ret.OleDbType = type;
            ret.Value = value;
            return ret;
        }
        protected IList<OleDbParameter> SetParameter(T entity, bool skip_identity = true)
        {
            IList<OleDbParameter> ret = new List<OleDbParameter>();
            List<FieldInfo> fds = GetFieldsInfo(typeof(T), skip_identity);
            foreach (FieldInfo fd in fds)
            {
                Column cn = fd.GetCustomAttribute(typeof(Column)) as Column;
                String columnName = String.Concat("@", cn.ColumnName);
                ret.Add(this.CreateParameter(columnName, fd.GetValue(entity), cn.ColumnType));
            }
            return ret;
        }
        protected void ClearParameter()
        {
            cmd.Parameters.Clear();
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
                    this.ExcuteNonReader(CreateInsertQuery(), SetParameter(entity));
                    return this.ExcuteReader("select @@identity", null, dr =>
                    {
                        return dr.GetInt32(0);
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
            IList<FieldInfo> fds = GetFieldsInfo(type, false);
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
                    where = where.Append(" ").Append(cn.ColumnName).Append("=").Append(columnName);
                }
                else
                {
                    if (cn.Identity)
                    {
                        continue;
                    }
                    if (set.Length == 0)
                    {
                        set.Append(" SET ");
                    }
                    else
                    {
                        set.Append(" , ");
                    }
                    set = set.Append(" ").Append(cn.ColumnName).Append("=").Append(columnName);
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
                //OLE bug????
                var para = SetParameter(entity, false);
                var temp = para[0];
                para.Remove(temp);
                para.Add(temp);
                return this.ExcuteNonReader(CreateUpdateQuery(), para);
            });
        }
        protected String CreateDeleteQuery()
        {
            Type type = typeof(T);
            Table table = type.GetCustomAttribute(typeof(Table)) as Table;
            StringBuilder sb = new StringBuilder();
            StringBuilder where = new StringBuilder();
            IList<OleDbParameter> parameter = new List<OleDbParameter>();
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
                IList<OleDbParameter> parameter = new List<OleDbParameter>();
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
                        GetFieldInfo(columnName).SetValue(ret, dr.GetValue(i));
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
        protected R SetClass<R>(OleDbDataReader dr)
        {
            R ret = (R)Activator.CreateInstance(typeof(R));
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.IsDBNull(i))
                {
                    continue;
                }
                String columnName = dr.GetName(i);
                GetFieldInfo(columnName).SetValue(ret, dr.GetValue(i));
            }
            return ret;
        }
        private List<FieldInfo> GetFieldsInfo(Type cls, bool skip_Identity = true)
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
                if (c.Identity && skip_Identity)
                {
                    continue;
                }
                ret.Add(pi);
            }
            return ret;
        }
        private FieldInfo GetFieldInfo(String columnName)
        {
            if (!flyweight.ContainsKey(columnName))
            {
                FieldInfo pi = GetEntityType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).AsParallel().First(item =>
                {
                    Column cn = item.GetCustomAttribute(typeof(Column)) as Column;
                    if (Object.Equals(columnName.ToUpper(), cn.ColumnName.ToUpper()))
                    {
                        return true;
                    }
                    return false;
                });
                flyweight.Add(columnName, pi);
            }
            return flyweight[columnName];
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
        protected int ExcuteReader(String query, IList<OleDbParameter> parameter, Action<OleDbDataReader> dataReader)
        {
            if (!isTransaction)
            {
                throw new Exception("ExcuteNonReader_000 : not transaction");
            }
            int count = 0;
            cmd.CommandText = query;
            if (parameter != null)
            {
                cmd.Parameters.AddRange(parameter.ToArray());
            }
            using (OleDbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    dataReader(dr);
                    count++;
                }
            }
            return count;
        }
        protected int ExcuteReader(String query, IList<OleDbParameter> parameter, Func<OleDbDataReader, int> dataReader)
        {
            if (!isTransaction)
            {
                throw new Exception("ExcuteNonReader_001 : not transaction");
            }
            int count = 0;
            cmd.CommandText = query;
            if (parameter != null)
            {
                cmd.Parameters.AddRange(parameter.ToArray());
            }
            using (OleDbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    count += dataReader(dr);
                }
            }
            return count;
        }
        protected int ExcuteNonReader(String query, IList<OleDbParameter> parameter = null)
        {
            if (!isTransaction)
            {
                throw new Exception("ExcuteNonReader_002 : not transaction");
            }
            cmd.CommandText = query;
            if (parameter != null)
            {
                cmd.Parameters.AddRange(parameter.ToArray());
            }
            return cmd.ExecuteNonQuery();
        }
        protected String CreateCsv(String data)
        {
            String filepath = Path.Combine(tmpPath, DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv");
            using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
            }
            return filepath;
        }
        protected Type GetEntityType()
        {
            return typeof(T);
        }
        /*protected int ExcuteBulk(String tablename, String file)
        {
            var bulk = new MySqlBulkLoader(cmd.Connection);
            bulk.TableName = tablename;
            bulk.FileName = file;
            bulk.FieldTerminator = "||";
            bulk.LineTerminator = "\r\n";
            return bulk.Load();
        }*/
    }
}
