using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Gecko;
using Gecko.DOM;

namespace WebScraping.Scraper.Impl
{
    class ScraperContext : ApplicationContext
    {
        ScrapBrowser browser = new ScrapBrowser();
        public ScraperContext()
        {
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));
            browser.Navigate("http://only1fs.com/");
        }
    }
}
