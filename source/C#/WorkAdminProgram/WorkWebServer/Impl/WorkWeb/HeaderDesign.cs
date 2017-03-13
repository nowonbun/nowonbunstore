using System;
using System.Text;
using System.Collections.Generic;

namespace WorkWebServer
{
    partial class WorkWebServerImpl : WorkWeb
    {
        class HeaderOption
        {
            IDictionary<String, String> option = new Dictionary<String, String>();
            public static HeaderOption Create()
            {
                return new HeaderOption();
            }
            public void AddOption(String key, String value)
            {
                option.Add(key, value);
            }
            public IDictionary<String, String> ToResult()
            {
                return option;
            }
        }

        private String2 Build(int webCode, HeaderOption option)
        {
            String2 ret = new String2(Encoding.UTF8);
            if (webCode == 200)
            {
                ret += "HTTP/1.1 200 OK" + String2.CRLF;
            }
            else
            {
                ret += "HTTP/1.1 " + webCode.ToString() + " NG" + String2.CRLF;
            }
            foreach (String key in option.ToResult().Keys)
            {
                ret += key + ": " + option.ToResult()[key] + String2.CRLF;
            }
            ret += "Keep-Alive: timeout=15, max=93" + String2.CRLF;
            ret += "Connection: Keep-Alive" + String2.CRLF + String2.CRLF;
            return ret;
        }
    }
}
