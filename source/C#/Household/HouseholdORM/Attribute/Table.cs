using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdORM
{
    /// <summary>
    /// author : SoonYub Hwang
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Table : System.Attribute
    {
        public enum TableType
        {
            TRANSACTION,
            MASTER
        }
        /// <summary>
        /// This is attribute of entity what is defined to table name from database.
        /// </summary>
        public String TableName
        {
            get;
            set;
        }
        public Table(String TableName)
        {
            this.TableName = TableName;
        }
    }
}
