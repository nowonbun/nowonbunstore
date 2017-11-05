using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrappingORMCore
{
    public class Entity : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
