using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;

namespace WebScraping.Dao.Create
{
    abstract class AbstractDaoCreate
    {
        private MySqlCommand cmd;
        private bool isTransaction = false;

        public AbstractDaoCreate(String connectionStr)
        {
            cmd = new MySqlCommand();
            cmd.Connection = new MySqlConnection();
            cmd.Connection.ConnectionString = connectionStr;
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
        protected int NonExcuteReader(String query, IList<MySqlParameter> parameter)
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
        protected MySqlParameter SetParameter(String name, Object value, MySqlDbType type)
        {
            MySqlParameter ret = new MySqlParameter();
            ret.ParameterName = name;
            ret.MySqlDbType = type;
            ret.Value = value;
            return ret;
        }
        protected void SaveFile(String path, String data)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] blob = Encoding.UTF8.GetBytes(data);
                stream.Write(blob, 0, blob.Length);
            }
        }
        public abstract void Run(String dirpath);
    }
}
