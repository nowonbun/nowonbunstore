using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Attribute
{
    /// <summary>
    /// ClassがTable entityであることを示す。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Table : System.Attribute
    {
        public string Name{ get;set; }

        public Table(string name)
        {
            Name = name;
        }
    }
}
