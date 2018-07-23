using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;

namespace WebScraping.Scraper.Flow.Gmarket
{
    partial class GMarketFlow : AbstractScrapFlow
    {
        private void Login(FlowModelData data)
        {
            if (data.Uri.ToString().IndexOf("ReturnValue") != -1)
            {
                //login 실패
                //String label = document.GetElementByClassName<GeckoHtmlElement>("login_text", 0).TextContent;
                //Console.WriteLine(label);
                return;
            }
            data.Document.GetElementByName<GeckoInputElement>("rdoSiteSelect", 1).Checked = true;
            data.Document.GetElementById<GeckoInputElement>("SiteId").Value = Parameter.Id1;
            data.Document.GetElementById<GeckoInputElement>("SitePassword").Value = Parameter.Pw1;
            data.Document.GetElementById<GeckoAnchorElement>("btnSiteLogOn").Click();
            SetCommonData(0, "TRUE");
        }
        private void Home(FlowModelData data)
        {
            base.Navigate("https://www.esmplus.com/Member/CustomerService/CSManagement?menuCode=TDM140");
        }
        private void CSManagement(FlowModelData data)
        {

            var val = data.Document.GetElementById<GeckoElement>("msg_gmarket").GetElementsByTagName("A")[0].FirstChild.NodeValue;
            // 1. 긴급메시지 건수
            SetCommonData(20, val);
            //보류
            //base.Navigate();
        }

    }
}
