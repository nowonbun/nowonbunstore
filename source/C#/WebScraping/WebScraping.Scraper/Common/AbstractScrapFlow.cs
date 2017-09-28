using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Node;
using Gecko;
using Gecko.DOM;
using WebScraping.Library.Log;

namespace WebScraping.Scraper.Common
{
    abstract class AbstractScrapFlow : IScrapFlow
    {
        protected ScrapParameter Parameter { get; private set; }

        protected Logger logger { get; private set; }
        public AbstractScrapFlow(ScrapParameter param, bool login_mode)
        {
            Parameter = param;
            logger = LoggerBuilder.Init().Set(this.GetType());
        }
        protected bool CheckSiteUrl(Uri uri, String key)
        {
            return uri.ToString().IndexOf(key) != -1;
        }
        protected virtual void NotAction(GeckoDocument document, Uri uri)
        {
            logger.Info("NotAction uri : " + uri);
        }
        public abstract String StartPage();
        public abstract Action<GeckoDocument, Uri> Procedure(Uri uri);
    }
}
