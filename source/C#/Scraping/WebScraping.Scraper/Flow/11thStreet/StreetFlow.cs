using System;
using System.Collections.Generic;
using System.Text;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;
using System.Reflection;

namespace WebScraping.Scraper.Flow._11thStreet
{
    partial class StreetFlow : AbstractScrapFlow
    {
        public StreetFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("StreetFlow initialize");
            FlowList.Add("login/Login.page", Login, null);
            FlowList.Add("Index.tmall", Index, null);
            FlowList.Add("marketing/SellerMenuAction.tmall", SellerMenuAction, null);
            FlowList.Add("register/SellerInfoEdit.tmall", SellerInfoEdit, null);
            FlowList.Add("stats/StatsPeriodProdSel.tmall", StatsPeriodProdSel, null);
            FlowList.Add("remittance/SellerRemittanceAction.tmall", SellerRemittanceAction, null);
            FlowList.Add("escrow/OrderingLogisticsAction.tmall", OrderingLogisticsAction, null);
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
            return "https://login.soffice.11st.co.kr/login/Login.page";
        }

        protected override void Finally(FlowModelData flowModelData)
        {
            logger.Info("Street scraping is exit");
        }
    }
}
