using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Node
{
    public class ScrapParameter
    {
        public ScrapParameter(String keycode, String param)
        {
            String[] temp = param.Split('&');
            foreach (String t in temp)
            {
                String[] buffer = t.Split('=');
                String key = buffer[0].ToUpper();
                String data = buffer[1];
                switch (key)
                {
                    case "ID":
                        this.Id = data;
                        break;
                    case "PW":
                        this.Pw = data;
                        break;
                    case "CODE":
                        this.Code = data;
                        break;
                }
            }
            this.Keycode = keycode;
        }
        public String Code { get; private set; }
        public String Id { get; private set; }
        public String Pw { get; private set; }
        public String Keycode { get; private set; }

        public override string ToString()
        {
            return String.Format("Key={0}, Code={1}, Id={2}", Keycode, Code, Id);
        }
    }
}
