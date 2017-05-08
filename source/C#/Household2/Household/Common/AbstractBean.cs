using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Common
{
    public class AbstractBean : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}