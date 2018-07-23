using System;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;

namespace WebScraping.Scraper.Flow.Halfclub
{
    partial class HalfclubFlow : AbstractScrapFlow
    {
        public HalfclubFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("HalfclubFlow initialize");
            FlowList.Add("index.aspx", Login, null);
            FlowList.Add("Home/Default.aspx", Default, null);
            FlowList.Add("PrivateMng/CompanyModify.aspx", CompanyModify, null);
        }
        protected override void Finally(FlowModelData flowModelData)
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        public override string StartPage(FlowModelData flowModelData)
        {
            return "https://scm.halfclub.com/index.aspx";
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
