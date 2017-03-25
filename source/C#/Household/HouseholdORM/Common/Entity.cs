using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdORM
{
    public abstract class Entity : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
