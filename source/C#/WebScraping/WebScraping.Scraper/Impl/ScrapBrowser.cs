using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using WebScraping.Scraper.Node;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Flow.Gmarket;
using WebScraping.Scraper.Other;
using WebScraping.Library.Log;

namespace WebScraping.Scraper.Impl
{
    class ScrapBrowser : GeckoWebBrowser
    {
        private IScrapFlow flow = null;
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScrapBrowser));
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        protected override void OnNavigated(GeckoNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }
        public void Set(ScrapParameter param)
        {
            switch (param.Code)
            {
                case "001":
                    flow = new Flow001(param, false);
                    break;
                case "501":
                    flow = new Flow001(param, true);
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
            Action<GeckoDocument,Uri> action = flow.Procedure(e.Uri);
            action(this.Document,e.Uri);
        }
        protected override void OnCreateWindow(GeckoCreateWindowEventArgs e)
        {
            e.Cancel = true;
            //base.OnCreateWindow(e);
        }
    }
}
