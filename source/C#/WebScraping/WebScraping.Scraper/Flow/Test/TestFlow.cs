using System;
using System.Collections.Generic;
using System.Text;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;
using WebScraping.Scraper.Node;
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
        public TestFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            FlowMap.Add("", Test);
        }
        public override string StartPage()
        {
            return "http://www.only1fs.com/";
        }
        private Boolean Test(GeckoDocument document, Uri uri)
        {
            logger.Debug("DEBUG.......");
            ThreadPool.QueueUserWorkItem(c =>
            {
                for (int i = 0; i < 60; i++)
                {
                    Thread.Sleep(1000);
                    logger.Debug("Second " + (i + 1));
                }

                this.End();
                Scraper.Exit();
            });
            return true;
        }
        protected override void Finally()
        {

        }
    }
}
