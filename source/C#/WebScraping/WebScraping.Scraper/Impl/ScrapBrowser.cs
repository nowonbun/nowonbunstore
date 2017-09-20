using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;

namespace WebScraping.Scraper.Impl
{
    class ScrapBrowser : GeckoWebBrowser
    {
        protected override void OnNavigated(GeckoNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }
        protected override void OnDocumentCompleted(Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            GeckoDocument doc = Document;
            GeckoElementCollection col = doc.GetElementsByTagName("HTML");
            foreach (var c in col)
            {
                Console.WriteLine(c.OuterHtml);
            }
        }
    }
}
