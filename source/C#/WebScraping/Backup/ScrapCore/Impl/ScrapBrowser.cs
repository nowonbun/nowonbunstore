using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using mshtml;

namespace ScrapCore
{
    class ScrapBrowser : WebBrowser
    {
        public event Action Scraping;
        public event Action Scraped;
        private ScrapNode node;
        private ScrapState2 state;
        private int scrapingPageCount;
        private int scrapedPageCount;
        private int scrapPageFixCount;
        
        public ScrapBrowser()
        {
            StaticUtil.InternetSetOption(this.Handle);
            ScriptErrorsSuppressed = false;
            this.NewWindow += (s, e) => { e.Cancel = true; };
        }

        protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            base.OnNavigating(e);
            if (Object.Equals(state,ScrapState2.ScrapComplate))
            {
                Initialize();
            }
            scrapingPageCount++;
            if (Object.Equals(state, ScrapState2.ScrapPageCountFix))
            {
                if (scrapPageFixCount <= scrapingPageCount)
                {
                    scrapingPageCount = scrapPageFixCount;
                }
            }
        }

        protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }

        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);

            scrapedPageCount++;
            if (scrapingPageCount < scrapedPageCount)
            {
                return;
            }
            if (scrapedPageCount > scrapingPageCount)
            {
                throw new Exception("This is not correct that is document index");
            }
            Finalize();
        }

        private void Initialize()
        {
            scrapingPageCount = 0;
            scrapedPageCount = 0;
            scrapPageFixCount = 0;
            Scraping();
        }

        public void SetPageFixCount(int count)
        {
            state = ScrapState2.ScrapPageCountFix;
            scrapPageFixCount = count;
        }

        public new void Navigate(string url)
        {
            Initialize();
            base.Navigate(url);
        }

        public void NavigateClick(IHTMLElement element)
        {
            Initialize();
            element.click();
        }

        private void Finalize()
        {
            state = ScrapState2.ScrapComplate;
            Scraped();
            node = new ScrapNode((IHTMLDocument)base.Document.DomDocument);
        }
    }
}
