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
using WebScraping.Library.Log;

namespace WebScraping.Scraper.Impl
{
    class ScraperContext : ApplicationContext
    {
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScraperContext));
        private ScrapBrowser browser = new ScrapBrowser();

        public ScraperContext(String key, String param)
        {
            logger.Info("ScraperContext initialize");
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));

            ScrapParameter sp = new ScrapParameter(param);
            logger.Debug("ScrapParameter : " + sp);
            browser.Set(sp);
        }
    }
}
