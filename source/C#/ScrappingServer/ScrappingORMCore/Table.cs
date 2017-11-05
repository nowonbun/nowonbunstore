using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrappingORMCore
{
    /// <summary>
    /// author : SoonYub Hwang
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Table : Attribute
    {
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
