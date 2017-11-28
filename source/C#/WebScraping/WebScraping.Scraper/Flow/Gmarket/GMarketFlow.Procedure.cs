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

namespace WebScraping.Scraper.Flow.Gmarket
{
    partial class GMarketFlow : AbstractScrapFlow
    {
        private Boolean Login(GeckoDocument document, Uri uri)
        {
            if (uri.ToString().IndexOf("ReturnValue") != -1)
            {
                //login 실패
                //String label = document.GetElementByClassName<GeckoHtmlElement>("login_text", 0).TextContent;
                //Console.WriteLine(label);
                return false;
            }
            document.GetElementByName<GeckoInputElement>("rdoSiteSelect", 1).Checked = true;
            document.GetElementById<GeckoInputElement>("SiteId").Value = Parameter.Id;
            document.GetElementById<GeckoInputElement>("SitePassword").Value = Parameter.Pw;
            document.GetElementById<GeckoAnchorElement>("btnSiteLogOn").Click();
            SetCommonData(0, "TRUE");
            return true;
        }
        private Boolean Home(GeckoDocument document, Uri uri)
        {
            base.Navigate("https://www.esmplus.com/Member/CustomerService/CSManagement?menuCode=TDM140");
            return true;
        }
        private Boolean CSManagement(GeckoDocument document, Uri uri)
        {
            
            var data = document.GetElementById<GeckoElement>("msg_gmarket").GetElementsByTagName("A")[0].FirstChild.NodeValue;
            // 1. 긴급메시지 건수
            SetCommonData(20, data);
            //보류
            //base.Navigate();
            return true;
        }

    }
}
