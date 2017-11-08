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

namespace WebScraping.Scraper.Flow.Auction
{
    partial class AuctionFlow : AbstractScrapFlow
    {
        private String idkey;
        private String idcode;
        public DateTime startdate;
        public DateTime enddate;
        private StringBuilder buffer = new StringBuilder();
        private IDictionary<Type, IList<FieldInfo>> reflectFlyweight = new Dictionary<Type, IList<FieldInfo>>();

        public AuctionFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Action initialize");
            reflectFlyweight.Add(typeof(BuyDecisionExcel), new List<FieldInfo>(typeof(BuyDecisionExcel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
            reflectFlyweight.Add(typeof(LacRemitListExcel), new List<FieldInfo>(typeof(LacRemitListExcel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
            reflectFlyweight.Add(typeof(GeneralDeliveryExcel), new List<FieldInfo>(typeof(GeneralDeliveryExcel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
            reflectFlyweight.Add(typeof(SendingExcel), new List<FieldInfo>(typeof(SendingExcel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));

            DateTime now = DateTime.Now;
            //startdate = now.AddYears(-1).AddDays(1);
            startdate = now.AddMonths(-1);
            enddate = now;

            StartPageUrl = "https://www.esmplus.com/Member/SignIn/LogOn";
            FlowMap.Add("Member/SignIn/LogOn", Login);
            FlowMap.Add("Home/Home", Home);
            FlowMap.Add("membership/MyInfo/MyInfoComp", Profile);
            FlowMap.Add("Escrow/Delivery/BuyDecision", BuyDecision);
            FlowMap.Add("Member/Settle/IacSettleDetail", LacSettleDetail);
            FlowMap.Add("Escrow/Delivery/GeneralDelivery", GeneralDelivery);
            FlowMap.Add("Escrow/Delivery/Sending", Sending);
            FlowMap.Add("Areas/Manual/SellerGuide", ScrapEnd);
            FlowMap.Add("Escrow/Claim/ReturnRequestManagement", ReturnRequestManagement);
            DownloadMap.Add("BuyDecisionExcel", BuyDecisionExcel);
            DownloadMap.Add("IacRemitListExcelDownload", LacRemitListExcelDownload);
            DownloadMap.Add("GeneralDeliveryExcel", GeneralDeliveryExcel);
            DownloadMap.Add("SendingExcel", SendingExcel);
            DownloadMap.Add("ExcelDownload", ExcelDownload);
        }

        private void SetModifyBuyDecision()
        {
            //4-5.정산예정금 ( 주문관리 > 구매결정완료 )
            FlowMap["Escrow/Delivery/BuyDecision"] = BuyDecision2;
            DownloadMap["BuyDecisionExcel"] = BuyDecisionExcel2;
        }
        protected override void Finally()
        {
            logger.Info("Action scraping is exit");
        }
    }
}
