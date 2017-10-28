using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using WebScraping.Scraper.Node;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Flow.Gmarket;
using WebScraping.Scraper.Flow.Auction;
using WebScraping.Scraper.Other;
using WebScraping.Library.Log;
using WebScraping.Library.Config;
using System.IO;

namespace WebScraping.Scraper.Impl
{
    class ScrapBrowser : GeckoWebBrowser
    {
        private IScrapFlow flow = null;
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScrapBrowser));
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        public ScrapBrowser()
        {

        }
        public void InitializeDownLoad(Action<String, String> download)
        {
            Gecko.LauncherDialog.Download += (sender, e) =>
            {
                String tempPath = ConfigSystem.ReadConfig("Config", "Temp", "Path");
                String file = Path.Combine(tempPath, e.Filename);
                nsILocalFile objTarget = (nsILocalFile)Xpcom.NewNativeLocalFile(file);
                e.HelperAppLauncher.SaveToDisk(objTarget, false);
                download(e.Url, file);
            };
        }

        protected override void OnNavigated(GeckoNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }

        public void Set(ScrapParameter param)
        {
            switch (param.Code)
            {
                case "001":
                    flow = new GMarketFlow(this, param, false);
                    break;
                case "002":
                    flow = new AuctionFlow(this, param, false);
                    break;
                case "501":
                    flow = new GMarketFlow(this, param, true);
                    break;
                case "502":
                    flow = new AuctionFlow(this, param, false);
                    break;
            }
            if (flow == null)
            {
                throw new ScraperException("Code is not defined. Please check code of parameter. Code : " + param.Code);
            }
            this.Navigate(flow.StartPage());
        }

        protected override void OnDocumentCompleted(Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            Func<GeckoDocument, Uri, Boolean> action = flow.Procedure(e.Uri);
            GeckoElement script = this.Document.CreateElement("script");
            script.SetAttribute("type", "text/javascript");
            script.TextContent = "window.alert = function(){};";
            this.Document.Head.AppendChild(script);

            if (!action(this.Document, e.Uri))
            {
                flow.End();
                Scraper.Exit();
            }
        }
        protected override void OnCreateWindow(GeckoCreateWindowEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
