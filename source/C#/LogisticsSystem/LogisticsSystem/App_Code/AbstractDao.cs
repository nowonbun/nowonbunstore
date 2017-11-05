using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace LogisticsSystem.App_Code
{
    public interface IDao
    {

    }
    public abstract class AbstractDao<T> : IDisposable, IDao
    {
        private SqlCommand m_Cmd = null;
        private List<SqlParameter> m_para;


        public AbstractDao()
        {
            m_para = new List<SqlParameter>();
            m_Cmd = new SqlCommand();
            m_Cmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
        }

        public void Dispose()
        {
            m_Cmd.Connection.Dispose();
            m_Cmd.Dispose();
        }

        protected int Insert(AbstractModel entity, String tableName)
        {
            IList<FieldInfo> keys = entity.GetKeys();
            StringBuilder query = new StringBuilder();
            StringBuilder queryValue = new StringBuilder();
            query.Append(" INSERT INTO ").Append(tableName).Append(" (");
            queryValue.Append(" ( ");
            ParameterInit();
            for (int i = 0; i < keys.Count; i++)
            {
                String cn = keys[i].Name;
                object data = keys[i].GetValue(entity);
                if (data == null)
                {
                    continue;
                }
                if (Object.Equals(typeof(DateTime), data.GetType()))
                {
                    DateTime temp = (DateTime)data;
                    if (Object.Equals(default(DateTime),temp))
                    {
                        continue;
                    }
                }
                if (i > 0)
                {
                    query.Append(",");
                    queryValue.Append(",");
                }
                query.Append(cn);
                queryValue.Append("@" + cn);
                ParameterAdd(cn, data);
            }
            query.Append(")");
            queryValue.Append(" ) ");
            return Insert(query.ToString() + "values" + queryValue.ToString(), GetParameter());
        }

        private int Insert(String query, SqlParameter[] para = null)
        {
            int pRet = -1;
            m_Cmd.CommandText = query;
            m_Cmd.Parameters.Clear();
            if (para != null)
            {
                m_Cmd.Parameters.AddRange(para);
            }
            try
            {
                m_Cmd.Connection.Open();
                pRet = m_Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("Insert Excepion");
                LogWriter.Instance().LogWrite("Error Code - " + e.ToString());
                LogWriter.Instance().LogWrite("QUERY - " + query);
                foreach (SqlParameter pBuffer in para)
                {
                    LogWriter.Instance().LogWrite("PARAMETER - " + pBuffer.ToString() + " : " + pBuffer.Value.ToString());
                }
                pRet = -1;
                throw e;
            }
            finally
            {
                m_Cmd.Connection.Close();
            }
            return pRet;
        }

        protected dynamic SelectCount(String query, SqlParameter[] para = null)
        {
            DataTable ret = SelectDataTable(query, para);
            if (ret.Rows.Count < 1)
            {
                return 0;
            }
            return ret.Rows[0][0];
        }

        protected DataTable SelectDataTable(String query, SqlParameter[] para = null)
        {
            DataTable ret = new DataTable();
            m_Cmd.CommandText = query;
            m_Cmd.Parameters.Clear();
            if (para != null)
            {
                m_Cmd.Parameters.AddRange(para);
            }
            try
            {
                m_Cmd.Connection.Open();
                using (SqlDataReader dr = m_Cmd.ExecuteReader())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        ret.Columns.Add(dr.GetName(i).ToLower(), dr.GetFieldType(i));
                    }
                    while (dr.Read())
                    {
                        DataRow row = ret.NewRow();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            row[dr.GetName(i).ToLower()] = dr.GetValue(i);
                        }
                        ret.Rows.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("Select Excepion");
                LogWriter.Instance().LogWrite("Error Code - " + e.ToString());
                LogWriter.Instance().LogWrite("QUERY - " + query);
                if (para != null)
                {
                    foreach (SqlParameter pBuffer in para)
                    {
                        LogWriter.Instance().LogWrite("PARAMETER - " + pBuffer.ToString() + " : " + pBuffer.Value.ToString());
                    }
                }
            }
            finally
            {
                m_Cmd.Connection.Close();
            }
            return ret;
        }

        protected IList<T> Select(String query, SqlParameter[] para = null)
        {
            IList<T> ret = new List<T>();
            DataTable buffer = SelectDataTable(query, para);
            foreach (DataRow row in buffer.Rows)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                for (int i = 0; i < buffer.Columns.Count; i++)
                {
                    DataColumn column = buffer.Columns[i];
                    object data = row[column];
                    if (Object.Equals(data.GetType(), typeof(DBNull)))
                    {
                        continue;
                    }
                    FieldInfo field = typeof(T).GetField(column.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance);
                    field.SetValue(obj, data);
                }
                ret.Add(obj);
            }
            return ret;
        }

        protected void ParameterInit()
        {
            m_para.Clear();
        }

        protected void ParameterAdd(String key, Object value)
        {
            m_para.Add(new SqlParameter(key, value));
        }

        protected SqlParameter[] GetParameter()
        {
            if (m_para.Count > 0)
            {
                SqlParameter[] pRet = new SqlParameter[m_para.Count];
                for (int i = 0; i < m_para.Count; i++)
                {
                    pRet[i] = m_para[i];
                }
                return pRet;
            }
            else
            {
                return null;
            }
        }

        protected int Delete(String query, SqlParameter[] para = null)
        {
            int pRet = -1;
            m_Cmd.CommandText = query;
            m_Cmd.Parameters.Clear();
            if (para != null)
            {
                m_Cmd.Parameters.AddRange(para);
            }
            try
            {
                m_Cmd.Connection.Open();
                pRet = m_Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("Delete Excepion");
                LogWriter.Instance().LogWrite("Error Code - " + e.ToString());
                LogWriter.Instance().LogWrite("QUERY - " + query);
                foreach (SqlParameter pBuffer in para)
                {
                    LogWriter.Instance().LogWrite("PARAMETER - " + pBuffer != null ? pBuffer.ToString() : "" + " : " + pBuffer != null && pBuffer.Value != null ? pBuffer.Value.ToString() : "");
                }
                pRet = -1;
            }
            finally
            {
                m_Cmd.Connection.Close();
            }
            return pRet;
        }

        protected int Update(String query, SqlParameter[] para = null)
        {
            int pRet = -1;
            m_Cmd.CommandText = query;
            m_Cmd.Parameters.Clear();
            if (para != null)
            {
                m_Cmd.Parameters.AddRange(para);
            }

            try
            {
                m_Cmd.Connection.Open();
                pRet = m_Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("Update Excepion");
                LogWriter.Instance().LogWrite("Error Code - " + e.ToString());
                LogWriter.Instance().LogWrite("QUERY - " + query);
                foreach (SqlParameter pBuffer in para)
                    LogWriter.Instance().LogWrite("PARAMETER - " + pBuffer.ToString() + " : " + pBuffer.Value.ToString());
                pRet = -1;
            }
            finally
            {
                m_Cmd.Connection.Close();
            }
            return pRet;
        }

        protected Int64 ScopeIndentity(String table)
        {
            Int64 pRet = 0;
            m_Cmd.CommandType = CommandType.Text;
            m_Cmd.CommandText = "SELECT IDENT_CURRENT('" + table + "');";
            m_Cmd.Parameters.Clear();
            try
            {
                m_Cmd.Connection.Open();
                using (SqlDataReader dr = m_Cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        pRet = Convert.ToInt64(dr.GetValue(0));
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("Scope Excepion");
                LogWriter.Instance().LogWrite("Error Code - " + e.ToString());
            }
            finally
            {
                m_Cmd.Connection.Close();
            }
            return pRet;
        }
    }
}
