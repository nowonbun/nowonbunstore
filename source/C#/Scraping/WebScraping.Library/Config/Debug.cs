using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Library.Config
{
    public static class Debug
    {
        public static Boolean IsDebug()
        {
            String debug = ConfigSystem.ReadConfig("Config", "Debug", "Debug");
            return debug.Equals("1");
        }
        public static String GetDebugParam()
        {
            return ConfigSystem.ReadConfig("Config", "Debug", "Param");
        }
    }
}
