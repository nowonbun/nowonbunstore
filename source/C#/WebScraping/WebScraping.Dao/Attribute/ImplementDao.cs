using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Dao.Attribute
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ImplementDao : System.Attribute
    {
        private String className;
        public ImplementDao(String className)
        {
            this.className = className;
        }
        public String ClassName
        {
            get { return this.className; }
        }
    }
}
