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
        protected String StartPageUrl { get; set; }
        protected Dictionary<String, Action<GeckoDocument, Uri>> FlowMap = new Dictionary<string, Action<GeckoDocument, Uri>>();

        protected Logger logger { get; private set; }
        public AbstractScrapFlow(ScrapParameter param, bool login_mode)
        {
            Parameter = param;
            logger = LoggerBuilder.Init().Set(this.GetType());
        }
        protected virtual void NotAction(GeckoDocument document, Uri uri)
        {
            logger.Info("NotAction uri : " + uri);
        }
        public String StartPage()
        {
            return StartPageUrl;
        }
        public Action<GeckoDocument, Uri> Procedure(Uri uri)
        {
            logger.Info("Procedure uri : " + uri);
            String key = FlowMap.Keys.Where(k => { return uri.ToString().IndexOf(k) != -1; }).SingleOrDefault();
            if (key == null)
            {
                return NotAction;
            }
            return FlowMap[key];
        }
    }
}
