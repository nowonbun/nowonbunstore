using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Library.Config
{
    public static class BrokerInfo
    {
        public static int GetPort()
        {
            String temp = ConfigSystem.ReadConfig("Config", "Broker", "Port");
            try
            {
                return Int32.Parse(temp);
            }
            catch
            {
                return 19999;
            }
        }
    }
}
