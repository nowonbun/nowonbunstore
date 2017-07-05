using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Attribute
{
    /// <summary>
    /// Field or PropertyがColumnであることを示す。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Column : System.Attribute
    {
        /// <summary>
        /// Column名を示す。
        /// </summary>
        public string Name { get; set; }
        public Column(string name)
        {
            Name = name;
        }
    }
}
