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

namespace WebScraping.Scraper.Flow.Interpark
{
    class InterparkFlow : AbstractScrapFlow
    {
        public InterparkFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("InterparkFlow initialize");
        }
        protected override void Finally()
        {
            Console.WriteLine("End!");
            Console.ReadLine();
        }
        public override string StartPage()
        {
            return "http://ipss.interpark.com/stdservice/ipssCheckLogin.do?_method=initial&LogOutFlag=Y";
        }
    }
}
