using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Dao.Common
{
    public abstract class Entity
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
