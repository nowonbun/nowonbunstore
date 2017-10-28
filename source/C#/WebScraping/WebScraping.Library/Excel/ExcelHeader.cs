using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Library.Excel
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ExcelHeader : System.Attribute
    {
        public ExcelHeader(String HeaderName,int ColumnIndex)
        {
            this.HeaderName = HeaderName;
            this.ColumnIndex = ColumnIndex;
        }
        public String HeaderName
        {
            get;
            set;
        }
        public int ColumnIndex
        {
            get;
            set;
        }
    }
}
