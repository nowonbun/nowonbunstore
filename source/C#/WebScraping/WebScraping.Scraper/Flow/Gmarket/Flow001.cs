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
            StartPageUrl = "https://www.esmplus.com/Member/SignIn/LogOn";
            FlowMap.Add("Member/SignIn/LogOn", Login);
            FlowMap.Add("Home/Home", Home);
        }
        private void Login(GeckoDocument document, Uri uri)
        {
            if (uri.ToString().IndexOf("ReturnValue") != -1)
            {
                //login 실패
                String label = document.GetElementByClassName<GeckoHtmlElement>("login_text", 0).TextContent;
                Console.WriteLine(label);
                return;
            }
            document.GetElementByName<GeckoInputElement>("rdoSiteSelect", 1).Checked = true;
            document.GetElementById<GeckoInputElement>("SiteId").Value = Parameter.Id;
            document.GetElementById<GeckoInputElement>("SitePassword").Value = Parameter.Pw;
            document.GetElementById<GeckoAnchorElement>("btnSiteLogOn").Click();
        }
        private void Home(GeckoDocument document, Uri uri)
        {
            String value = document.GetElementById<GeckoHtmlElement>("header")
                                      .GetElementByTagName<GeckoHtmlElement>("DIV", 0)
                                          .GetElementByTagName<GeckoHtmlElement>("SPAN", 0)
                                              .GetElementByTagName<GeckoHtmlElement>("STRONG", 0).FirstChild.NodeValue;
            Console.WriteLine(value);

        }
    }
}
