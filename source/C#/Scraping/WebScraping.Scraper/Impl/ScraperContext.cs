using System;
using System.Windows.Forms;
using System.IO;
using Gecko;
using WebScraping.Library.Log;
using Newtonsoft.Json;

namespace WebScraping.Scraper.Impl
{
    class ScraperContext : ApplicationContext
    {
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScraperContext));
        private ScrapBrowser browser = new ScrapBrowser();

        public ScraperContext(Parameter param)
        {
            logger.Info("ScraperContext initialize");
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));

            logger.Debug("ScrapParameter : " + param);
            browser.Set(param);
        }
    }
}
