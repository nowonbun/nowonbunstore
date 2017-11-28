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
        public GMarketFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Gmarket initialize");
            FlowMap.Add("Member/SignIn/LogOn", Login);
            FlowMap.Add("Home/Home", Home);
            FlowMap.Add("Member/CustomerService/CSManagement", CSManagement);
        }
        protected override void Finally()
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        public override string StartPage()
        {
            return "https://www.esmplus.com/Member/SignIn/LogOn";
        }
    }
}
