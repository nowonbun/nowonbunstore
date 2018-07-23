using System;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;
using Gecko;
using Gecko.DOM;

namespace WebScraping.Scraper.Flow.StoreFarm
{
    partial class StoreFarmFlow : AbstractScrapFlow
    {
        public StoreFarmFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("StoreFarmFlow initialize");

            FlowList.Add("#/login", Login, null);
            FlowList.Add("#", Home, null);
            FlowList.Add("#/home/dashboard", Home, null);
            FlowList.Add("#/seller/info", Info, null);
        }
        protected override void Finally(FlowModelData flowModelData)
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        public override string StartPage(FlowModelData flowModelData)
        {
            //return "https://sell.storefarm.naver.com/#/login?url=https:~2F~2Fsell.storefarm.naver.com~2F%23~2Fhome~2Fdashboard";
            return "https://sell.storefarm.naver.com/#/login";
        }
        private void PrivateNavigate(GeckoDocument document, String url)
        {
            var aa = document.GetElementsByClassName("naver");
            document.GetElementByClassName<GeckoAnchorElement>("naver").Href = url;
            document.GetElementByClassName<GeckoAnchorElement>("naver").Click();
        }
        protected override void ScrapType0(FlowModelData flowModelData)
        {

        }
        protected override void ScrapType1(FlowModelData flowModelData)
        {

        }
        protected override void ScrapType2(FlowModelData flowModelData)
        {

        }
        protected override void ScrapType3(FlowModelData flowModelData)
        {

        }
        protected override void ScrapType4(FlowModelData flowModelData)
        {

        }
        protected override void ScrapType5(FlowModelData flowModelData)
        {

        }
    }
}
