using System;
using System.Data.OleDb;

namespace PTM.ORM.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Column : System.Attribute
    {
        public String ColumnName
        {
            get;
            set;
        }
        public OleDbType ColumnType
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
        public int ColumnSize
        {
            get;
            set;
        }
        public Column(String ColumnName, OleDbType ColumnType)
        {
            this.ColumnName = ColumnName;
            this.ColumnType = ColumnType;
            this.Key = false;
            this.Identity = false;
            this.ColumnSize = -1;
        }
    }
}
