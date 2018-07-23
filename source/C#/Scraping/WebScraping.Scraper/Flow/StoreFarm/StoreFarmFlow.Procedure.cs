using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;
using Newtonsoft.Json;

namespace WebScraping.Scraper.Flow.StoreFarm
{
    partial class StoreFarmFlow : AbstractScrapFlow
    {
        private void Login(FlowModelData data)
        {
            var element = data.Document.GetElementsByTagName("H1")[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("<button onclick='$(\"#loginId\").val(\"" + Parameter.Id1 + "\");$(\"#loginId\").trigger(\"input\");'>id</button>");
            sb.Append("<button onclick='$(\"#loginPassword\").val(\"" + Parameter.Pw1 + "\");$(\"#loginPassword\").trigger(\"input\");'>pw</button>");
            element.InnerHtml += sb.ToString();
            var collect = element.GetElementsByTagName("BUTTON");
            (collect[0] as GeckoButtonElement).Click();
            (collect[1] as GeckoButtonElement).Click();
            data.Document.GetElementById<GeckoButtonElement>("loginButton").Click();
        }
        private void Home(FlowModelData data)
        {
            //base.Navigate("https://sell.smartstore.naver.com/#/seller/info");
            //PrivateNavigate(document, "https://sell.smartstore.naver.com/#/seller/info");
            //TODO: issue
            //TODO: this site can not do that scrap is.I think because of angularjs.
            //TODO: I need time for research that scrap is.
        }
        private void Info(FlowModelData data)
        {
            var list = data.Document.GetElementsByClassName("form-control-static").ToList();
            SetCommonData(1, list[1].NodeValue);
            SetCommonData(4, list[2].NodeValue);
            SetCommonData(12, list[3].NodeValue);
            SetCommonData(8, list[5].NodeValue);
            SetCommonData(7, list[6].NodeValue);
            SetCommonData(6, list[7].NodeValue);
            SetCommonData(3, list[8].NodeValue);
            SetCommonData(10, list[13].NodeValue);
            String buffer = data.Document.GetElementByClassName<GeckoElement>("input-content", 15).NodeValue;

            Console.Write(buffer);
        }
    }
}
