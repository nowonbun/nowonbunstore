using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIsSocketServices
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Messgae : System.Attribute
    {
        public String KeyName
        {
            get;
            set;
        }
        public Messgae(String keyName)
        {
            this.KeyName = keyName;
        }
    }
}
