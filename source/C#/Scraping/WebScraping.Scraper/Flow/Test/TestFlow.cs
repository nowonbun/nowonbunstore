using System;
using System.Collections.Generic;
using System.Text;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;
using WebScraping.Scraper.Other;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using WebScraping.Library.Excel;
using System.Reflection;

namespace WebScraping.Scraper.Flow.Test
{
    class TestFlow : AbstractScrapFlow
    {
        public TestFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            FlowList.Add("", Test, null);
        }
        public override string StartPage(FlowModelData flowModelData)
        {
            return "http://www.only1fs.com/";
        }
        private void Test(FlowModelData data)
        {
            logger.Debug("DEBUG.......");
            ThreadPool.QueueUserWorkItem(c =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    SetCommonData(i, "Common Test");
                    SetPackageData(i, 0, "Pacakage Test");
                }
            });
        }
        protected override void Finally(FlowModelData flowModelData)
        {

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
