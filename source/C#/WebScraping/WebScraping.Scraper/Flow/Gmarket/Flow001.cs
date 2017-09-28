using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Node;

namespace WebScraping.Scraper.Flow.Gmarket
{
    class Flow001 : AbstractScrapFlow
    {
        public Flow001(ScrapParameter param, bool login_mode)
            : base(param, login_mode)
        {
            logger.Info("Gmarket initialize");
        }
        public override Action<GeckoDocument, Uri> Procedure(Uri uri)
        {
            logger.Info("Procedure uri : " + uri);
            if (CheckSiteUrl(uri, "Member/SignIn/LogOn"))
            {
                return Login;
            }
            else if (CheckSiteUrl(uri, "Home/Home"))
            {
                return Home;
            }
            return NotAction;
        }
        public override String StartPage()
        {
            return "https://www.esmplus.com/Member/SignIn/LogOn";
        }
        private void Login(GeckoDocument document, Uri uri)
        {
            (document.GetElementsByName("rdoSiteSelect")[1] as GeckoInputElement).Checked = true;
            (document.GetElementById("SiteId") as GeckoInputElement).Value = Parameter.Id;
            (document.GetElementById("SitePassword") as GeckoInputElement).Value = Parameter.Pw;
            (document.GetElementById("btnSiteLogOn") as GeckoAnchorElement).Click();
        }
        private void Home(GeckoDocument document, Uri uri)
        {
            String value = ((((document.GetElementById("header") as GeckoHtmlElement)
                                .GetElementsByTagName("DIV")[0] as GeckoHtmlElement)
                                    .GetElementsByTagName("SPAN")[0] as GeckoHtmlElement)
                                        .GetElementsByTagName("STRONG")[0] as GeckoHtmlElement)
                                            .FirstChild.NodeValue;
            Console.WriteLine(value);

        }
    }
}
