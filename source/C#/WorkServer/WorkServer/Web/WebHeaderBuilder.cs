using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkServer
{
    class WebHeaderBuilder
    {
        private int WebCode;
        private IDictionary<String, String> Option = new Dictionary<String, String>();
        public static WebHeaderBuilder GetHeader(int WebCode)
        {
            return new WebHeaderBuilder(WebCode);
        }
        public WebHeaderBuilder(int WebCode)
        {
            this.WebCode = WebCode;
        }
        public void AddOption(String key, String content)
        {
            Option.Add(key, content);
        }
        public String2 Build()
        {
            String2 ret = new String2(Encoding.UTF8);
            if (WebCode == 200)
            {
                ret += "HTTP/1.1 200 OK" + String2.CRLF;
            }
            else
            {
                ret += "HTTP/1.1 " + WebCode.ToString() + " NG" + String2.CRLF;
            }
            foreach (String key in Option.Keys)
            {
                ret += key + ": " + Option[key] + String2.CRLF;
            }
            ret += "Keep-Alive: timeout=15, max=93" + String2.CRLF;
            ret += "Connection: Keep-Alive" + String2.CRLF + String2.CRLF;
            return ret;
        }
    }
}
