using AisProjectCore.Domain.Attribute;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    public class MysqlRepo : IRepository
    {
        private string server;
        private string database;
        private string user;
        private string password;
        private int port;

        private MySqlConnection conn;
                
        /// <summary>
        /// check Key column
        /// </summary>
        /// <param name="p">column property type</param>
        /// <returns>
        ///     true    : key coolumn
        ///     false   : not key column
        /// </returns>
        private bool IsKeyColumn(PropertyInfo p)
        {
            try
            {
                var attributes = p.GetCustomAttribute(typeof(Key));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        /// <summary>
        /// generate where string
        /// </summary>
        /// <typeparam name="T">entity type</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="containKeyColumn">if true, contain column with [Key] attribute</param>
        /// <returns>where string</returns>
        private string ToWhere<T>(T entity, bool containKeyColumn)
        {
            string query = "";
            Type t = entity.GetType();
            var properties = t.GetProperties();
            bool isFirst = true;
            foreach(var property in properties)
            {
                // check key column
                if(IsKeyColumn(property) && !containKeyColumn)
                {
                    continue;
                }

                if(isFirst)
                {
                    isFirst = false;
                } else
                {
                    query += " AND ";
                }
                string name = property.Name;
                string value = property.GetValue(entity).ToString();

                query += $"'{name}' = '{value}'";
                
            }

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string ToWhereWithKeyColumn<T>(T entity)
        {
            Type t = entity.GetType();
            var properties = t.GetProperties();

            foreach (var property in properties)
            {
                if(IsKeyColumn(property))
                {
                    return $"{property.Name} = {property.GetValue(entity).ToString()}";
                }
            }

            throw new KeyNotFoundException($"This entity object {t.Name} is not contained key column");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GetInsertQuery<T>(string table, T entity)
        {
            string[] query_string = { "INSERT INTO ", "VALUES(" };
            string[] query_inject = { table, ToWhere(entity, false) };
            string query = "";

            for(int i = 0; i<2; i++)
            {
                query += query_string[i];
                query += query_inject[i];
            }

            return query + ");";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private string GetDeleteQuery<P>(string table, P where)
        {
            string[] query_string = { "DELETE FROM ", " WHERE " };
            string[] query_inject = { table, ToWhereWithKeyColumn(where)};
            string query = "";

            for(int i = 0; i<2; i++)
            {
                query += query_string[i];
                query += query_inject[i];
            }

            return query + ';';
        }

        /// <summary>
        /// Generate Update queery
        /// </summary>
        /// <typeparam name="T">Table entity type</typeparam>
        /// <param name="table">Table name</param>
        /// <param name="entity">Entity data</param>
        /// <returns>insert query string</returns>
        private string GetUpdateQuery<T>(string table, T entity)
        {
            string[] query_string = { "UPDATE ", " SET ", " WHERE " };
            string[] query_inject = { table, ToWhere(entity, false), ToWhereWithKeyColumn(entity) };
            string query = "";

            for(int i = 0; i<3; i++)
            {
                query += query_string[i];
                query += query_inject[i];
            }

            return query += ';';
        }

        /// <summary>
        /// Generate select query
        /// </summary>
        /// <typeparam name="P">Where object type</typeparam>
        /// <param name="table">table name</param>
        /// <param name="where">where entity</param>
        /// <returns>select query string</returns>
        private string GetSelectQuery<P>(string table, P where)
        {
            string[] query_string = { "SELECT ", " FROM ", " WHERE " };
            string[] query_inject = { "*", table, where != null ? ToWhere(where, true) : "" };
            string query = "";

            for(int i = 0; i<2; i++)
            {
                query += query_string[i];
                query += query_inject[i];
            }

            if(where != null)
            {
                query += query_string[2];
                query += query_inject[2];
            }

            query += ';';
            return query;
        }

        /// <summary>
        /// Get connect string
        /// </summary>
        /// <returns></returns>
        private string GetConnectString()
        {
            return ConnStringFrom(server, database, user, password, port);
        }

        /// <summary>
        /// Get connect string
        /// </summary>
        /// <param name="servr"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <returns>connnectstring</returns>
        private string ConnStringFrom(string servr, string database, string user, string password, int port)
        {
            return $"server={server};user={user};database={database};password={password};port={port}";
        }

        /// <summary>
        /// Get table name from table entity type
        /// </summary>
        /// <typeparam name="T">table entity type</typeparam>
        /// <returns></returns>
        private string GetTableName<T>()
        {
            Type t = typeof(T);
            try
            {
                var tableattrb = t.GetCustomAttribute<Table>() as Table;
                return tableattrb.Name;
            } catch (Exception)
            {
                return t.Name;
            }
        }

        /// <summary>
        /// Convert mysql column data to C# basictype data
        /// </summary>
        /// <param name="property_type">c# basictype</param>
        /// <param name="data">mysql data</param>
        /// <returns>converted data</returns>
        private object ConvertData(Type property_type, object data)
        {
            switch (property_type.Name)
            {
                case "char":        return Convert.ToChar(data);
                case "short":       return Convert.ToInt16(data);
                case "int":         return Convert.ToInt32(data);
                case "long":        return Convert.ToInt64(data);
                case "float":       return float.Parse(data.ToString());
                case "double":      return Convert.ToDouble(data);
                case "DateTime":    return Convert.ToDateTime(data.ToString());
                default:            return null;
            }
        }

        /// <summary>
        /// Generate entity list from mysql datareader
        /// </summary>
        /// <typeparam name="T">result entity type</typeparam>
        /// <param name="reader">mysql data reader</param>
        /// <returns>entity list</returns>
        private IList<T> ListFromReader<T>(MySqlDataReader reader)
        {
            Type type = typeof(T);
            List<T> result = new List<T>();
            while(reader.Read())
            {
                // instance entity
                object instance = Activator.CreateInstance(type);

                // get properties
                PropertyInfo[] properties = type.GetProperties();
                foreach(var property in properties)
                {
                    string field_name = property.Name;
                    Type field_type = property.PropertyType;

                    // convert and set data
                    property.SetValue(instance, ConvertData(field_type, reader[field_name]));
                }

                // add entity to list
                result.Add((T)instance);
            }
            return result;
        }

        /// <summary>
        /// request query to mysql with return value
        /// </summary>
        /// <typeparam name="T">return entity type</typeparam>
        /// <param name="query">query string</param>
        /// <returns>entity data list</returns>
        private IList<T> Query<T>(string query)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    return ListFromReader<T>(reader);
                }
            } catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// request query to mysql without return value;
        /// </summary>
        /// <param name="query">query string</param>
        private void Query(string query)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Set connect parameter
        /// </summary>
        /// <param name="sv">server address</param>
        /// <param name="db">database name</param>
        /// <param name="usr">user name</param>
        /// <param name="pw">password</param>
        /// <param name="pt">port number</param>
        public void SetConnectString(string sv, string db, string usr, string pw, int pt = 3306)
        {
            server = sv;
            database = db;
            user = usr;
            password = pw;
            port = pt;
        }
        
        /// <summary>
        /// Connect mysql
        /// </summary>
        public void Connect()
        {
            if (conn != null)
                conn.Close();
            
            SetConnectString("localhost", "ais_webservice", "root", "root");

            conn = new MySqlConnection(GetConnectString());
            try
            {
                conn.Open();
            } catch (MySqlException e)
            {
                conn = null;
                Console.WriteLine(e.ErrorCode + ": " + e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Close mysql connect
        /// </summary>
        public void Close()
        {
            if (conn != null)
                conn.Close();
        }

        /// <summary>
        /// Insert data to table
        /// </summary>
        /// <typeparam name="T">Table entity type</typeparam>
        /// <param name="entity">Insert entity data</param>
        public void Insert<T>(T entity)
        {
            string table = GetTableName<T>();
            string query = GetInsertQuery(table, entity);
            Query(query);
        }

        /// <summary>
        /// Update data in table
        /// </summary>
        /// <typeparam name="T">Table entity type</typeparam>
        /// <param name="entity">Update entity data</param>
        /// <param name="where">Where entity data</param>
        public void Update<T>(T entity)
        {
            string table = GetTableName<T>();
            string query = GetUpdateQuery(table, entity);
            Query(query);
        }

        /// <summary>
        /// Delete data from table
        /// </summary>
        /// <typeparam name="P">Where entity type</typeparam>
        /// <param name="where">where data</param>
        public void Delete<P>(P where)
        {
            string table = GetTableName<P>();
            string query = GetDeleteQuery(table, where);
            Query(query);
        }

        /// <summary>
        /// Get data from table
        /// </summary>
        /// <typeparam name="T">Table entity type</typeparam>
        /// <typeparam name="P">Where entity type</typeparam>
        /// <param name="table">target table name</param>
        /// <param name="wheres">where data</param>
        /// <returns>Entity datas</returns>
        public IList<T> Select<T, P>(P where)
        {
            string table = GetTableName<T>();
            string query = GetSelectQuery(table, where);
            return Query<T>(query);
        }

        /// <summary>
        /// Get all data from table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="table">target table name</param>
        /// <returns>Entity datas</returns>
        public IList<T> SelectAll<T>(string table)
        {
            string query = GetSelectQuery(table, default(T));
            return Query<T>(query);
        }

        /// <summary>
        /// IDispose
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
