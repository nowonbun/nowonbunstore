using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Gecko;
using Gecko.DOM;
using System.Net;
using WebScraping.Scraper.Node;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Flow.Gmarket;

namespace WebScraping.Scraper.Impl
{
    class ScraperContext : ApplicationContext
    {
        ScrapBrowser browser = new ScrapBrowser();
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        public ScraperContext(String key, String param)
        {
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));

            ScrapParameter sp = GetParameter(param);
            browser.Set(sp);
        }
        
        private ScrapParameter GetParameter(String param)
        {
            String[] temp = param.Split('&');
            ScrapParameter ret = new ScrapParameter();
            foreach (String t in temp)
            {
                String[] buffer = t.Split('=');
                String key = buffer[0].ToUpper();
                String data = buffer[1];
                switch (key)
                {
                    case "ID":
                        ret.Id = data;
                        break;
                    case "PW":
                        ret.Pw = data;
                        break;
                    case "CODE":
                        ret.Code = data;
                        break;
                }
            }
            return ret;
        }
    }
}
