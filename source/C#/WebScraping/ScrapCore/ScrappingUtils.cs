using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapCore
{
    public enum ScrapState
    {
        INIT = 1,   
        RUNNING = 2,
        COMPLETE = 3,
        ERROR = -1,
        EMPTY = -2
    }
    public static class ScrappingUtils
    {
        public static bool ContainOf(String target, String value)
        {
            if (String.IsNullOrEmpty(target))
            {
                return false;
            }
            if (target.IndexOf(value) == -1)
            {
                return false;
            }
            return true;
        }
    }
}
