using System;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;

namespace WebScraping.Scraper.Flow.Yes24
{
    class Yes24Flow : AbstractScrapFlow
    {
        public Yes24Flow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("Yes24Flow initialize");
        }
        protected override void Finally(FlowModelData flowModelData)
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        public override string StartPage(FlowModelData flowModelData)
        {
            return "https://scm.yes24.com/Login/LogOn";
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
