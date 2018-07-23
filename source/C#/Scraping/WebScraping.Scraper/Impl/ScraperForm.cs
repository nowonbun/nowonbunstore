using System;
using System.Windows.Forms;
using System.IO;
using Gecko;
using WebScraping.Library.Log;
using WebScraping;
using Newtonsoft.Json;

namespace WebScraping.Scraper.Impl
{
    class ScraperForm : Form
    {
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScraperContext));
        private ScrapBrowser browser = new ScrapBrowser();

        public ScraperForm(Parameter param)
        {
            logger.Info("ScraperContext initialize");
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));

            browser.Dock = DockStyle.Fill;
            this.Size = new System.Drawing.Size(1000, 500);
            this.Controls.Add(browser);

            logger.Debug("ScrapParameter : " + param);
            browser.Set(param);
            
        }
    }
}
