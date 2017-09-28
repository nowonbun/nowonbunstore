using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Node
{
    public class ScrapParameter
    {
        public ScrapParameter(String param)
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
        }
        public String Code { get; private set; }
        public String Id { get; private set; }
        public String Pw { get; private set; }

        public override string ToString()
        {
            return String.Format("Code={0}, Id={1}", Code, Id);
        }
    }
}
