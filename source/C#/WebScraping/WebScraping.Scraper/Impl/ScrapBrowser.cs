using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using WebScraping.Scraper.Node;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Flow.Gmarket;

namespace WebScraping.Scraper.Impl
{
    class ScrapBrowser : GeckoWebBrowser
    {
        private IScrapFlow flow = null;
        protected override void OnNavigated(GeckoNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }
        protected override void OnDocumentCompleted(Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            flow.Procedure(e.Uri.ToString());
        }
        public void Set(ScrapParameter param)
        {
            switch (param.Code)
            {
                case "001":
                    flow = new Flow001(param.Id, param.Pw, false);
                    break;
                case "501":
                    flow = new Flow001(param.Id, param.Pw, true);
                    break;
            }
            if (flow == null)
            {
                new Exception();
            }
            this.Navigate(flow.SetStartPage());
        }
    }
}
