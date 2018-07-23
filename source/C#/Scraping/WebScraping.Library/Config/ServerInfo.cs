using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Library.Config
{
    public static class ServerInfo
    {
        public static String GetIp()
        {
            return ConfigSystem.ReadConfig("Config", "Server", "Ip");
        }
        public static int GetPort()
        {
            String temp = ConfigSystem.ReadConfig("Config", "Server", "Port");
            try
            {
                return Int32.Parse(temp);
            }
            catch
            {
                return 9999;
            }
        }
    }
}
