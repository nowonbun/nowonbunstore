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
        public GMarketFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("Gmarket initialize");
            FlowList.Add("Member/SignIn/LogOn", Login, null);
            FlowList.Add("Home/Home", Home, null);
            FlowList.Add("Member/CustomerService/CSManagement", CSManagement, null);
        }
        protected override void Finally(FlowModelData flowModelData)
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        public override string StartPage(FlowModelData flowModelData)
        {
            return "https://www.esmplus.com/Member/SignIn/LogOn";
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
