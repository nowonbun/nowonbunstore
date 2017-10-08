using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace WebScraping.Dao.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Column : System.Attribute
    {
        public String ColumnName
        {
            get;
            set;
        }
        public MySqlDbType ColumnType
        {
            get;
            set;
        }
        public Boolean Key
        {
            get;
            set;
        }
        public Boolean Identity
        {
            get;
            set;
        }

        public String LogicalName
        {
            get;
            set;
        }
        public Column(String ColumnName, MySqlDbType ColumnType)
        {

            this.ColumnName = ColumnName;
            this.ColumnType = ColumnType;
            this.Key = false;
            this.Identity = false;
        }
    }
}
