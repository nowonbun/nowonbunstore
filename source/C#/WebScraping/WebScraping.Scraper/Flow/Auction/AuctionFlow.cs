using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;
using WebScraping.Scraper.Node;

namespace WebScraping.Scraper.Flow.Auction
{
    class AuctionFlow : AbstractScrapFlow
    {
        public AuctionFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Action initialize");
            StartPageUrl = "https://www.esmplus.com/Member/SignIn/LogOn";
            FlowMap.Add("Member/SignIn/LogOn", Login);
            FlowMap.Add("Home/Home", Home);
            FlowMap.Add("membership/MyInfo/MyInfoComp", Profile);
        }
        protected override void Finally()
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        private Boolean Login(GeckoDocument document, Uri uri)
        {
            if (uri.ToString().IndexOf("ReturnValue") != -1)
            {
                //String label = document.GetElementByClassName<GeckoHtmlElement>("login_text", 0).TextContent;
                //Console.WriteLine(label);
                SetCommonData(0, "FALSE");
                return false;
            }
            document.GetElementByName<GeckoInputElement>("rdoSiteSelect", 0).Checked = true;
            document.GetElementById<GeckoInputElement>("SiteId").Value = Parameter.Id;
            document.GetElementById<GeckoInputElement>("SitePassword").Value = Parameter.Pw;
            document.GetElementById<GeckoAnchorElement>("btnSiteLogOn").Click();
            SetCommonData(0, "TRUE");
            return true;
        }
        private Boolean Home(GeckoDocument document, Uri uri)
        {
            /*String value = document.GetElementById<GeckoHtmlElement>("header")
                                      .GetElementByTagName<GeckoHtmlElement>("DIV", 0)
                                          .GetElementByTagName<GeckoHtmlElement>("SPAN", 0)
                                              .GetElementByTagName<GeckoHtmlElement>("STRONG", 0).FirstChild.NodeValue;
            Console.WriteLine(value);*/
            base.Navigate("http://www.esmplus.com/Home/SSO?code=TDM155&id=" + Parameter.Id);
            return true;
        }
        private Boolean Profile(GeckoDocument document, Uri uri)
        {
            SetCommonData(1, document.GetElementByIdToNodeValue("lblCustName"));
            SetCommonData(3, document.GetElementByIdToNodeValue("lblPresident"));
            SetCommonData(4, document.GetElementByIdToNodeValue("lblRegiNumber"));
            String buffer = document.GetElementByIdToNodeValue("lblBizCateKind");
            if (buffer != null)
            {
                String[] temp = buffer.Split('/');
                SetCommonData(7, temp[0]);
                SetCommonData(8, temp[1]);
            }
            String tel1 = document.GetElementById<GeckoSelectElement>("ddlMobileTel").Value;
            String tel2 = document.GetElementById<GeckoInputElement>("txtMobileTel2").Value;
            String tel3 = document.GetElementById<GeckoInputElement>("txtMobileTel3").Value;
            SetCommonData(9, tel1 + "-" + tel2 + "-" + tel3);

            return false;
        }
    }
}
