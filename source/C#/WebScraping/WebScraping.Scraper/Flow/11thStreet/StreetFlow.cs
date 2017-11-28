using System;
using System.Collections.Generic;
using System.Text;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;
using WebScraping.Scraper.Node;
using System.Reflection;

namespace WebScraping.Scraper.Flow._11thStreet
{
    partial class StreetFlow : AbstractScrapFlow
    {
        public StreetFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("StreetFlow initialize");
            FlowMap.Add("login/Login.page", Login);
            FlowMap.Add("Index.tmall", Index);
        }
        public override string StartPage()
        {
            return "https://login.soffice.11st.co.kr/login/Login.page";
        }

        protected override void Finally()
        {
            logger.Info("Street scraping is exit");
        }
    }
}
