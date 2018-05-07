using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTM.ORM.Attribute
{
    class Table : System.Attribute
    {
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
