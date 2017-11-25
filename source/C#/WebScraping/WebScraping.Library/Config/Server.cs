using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Library.Config
{
    public static class ServerInfo
    {
        public static int GetPort()
        {
            String temp = ConfigSystem.ReadConfig("Config", "Server", "Port");
            try
            {
                return Int32.Parse(temp);
            }
            catch
            {
                return 19999;
            }
        }
        public static String GetWebRoot()
        {
            return ConfigSystem.ReadConfig("Config", "Server", "Root");
        }
    }
}
