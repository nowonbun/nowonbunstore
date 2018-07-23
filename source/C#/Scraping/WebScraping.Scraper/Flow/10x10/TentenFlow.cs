using System;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;

namespace WebScraping.Scraper.Flow._10x10
{
    class TentenFlow : AbstractScrapFlow
    {
        public TentenFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("AllatpayFlow initialize");
        }
        protected override void Finally(FlowModelData flowModelData)
        {
            Console.WriteLine("End!");
            Console.ReadLine();
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
        public override string StartPage(FlowModelData flowModelData)
        {
            return "http://scm.10x10.co.kr/";
        }
    }
}
