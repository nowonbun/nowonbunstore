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
        

        public AuctionFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Action initialize");

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
            FlowMap.Add("Sell/Items/ItemsMng", ItemsMng);
            FlowMap.Add("Sell/Items/GetItemMngList", GetItemMngList);

            DownloadMap.Add("BuyDecisionExcel", BuyDecisionExcel);
            DownloadMap.Add("IacRemitListExcelDownload", LacRemitListExcelDownload);
            DownloadMap.Add("GeneralDeliveryExcel", GeneralDeliveryExcel);
            DownloadMap.Add("SendingExcel", SendingExcel);
            DownloadMap.Add("ExcelDownload", ExcelDownload);

            base.ReflectFlyweightKeys.Add(typeof(BuyDecisionExcel));
            base.ReflectFlyweightKeys.Add(typeof(LacRemitListExcel));
            base.ReflectFlyweightKeys.Add(typeof(GeneralDeliveryExcel));
            base.ReflectFlyweightKeys.Add(typeof(SendingExcel));
            base.ReflectFlyweightKeys.Add(typeof(ReturnRequest));
            base.ReflectFlyweightKeys.ForEach(type =>
            {
                ReflectFlyweight.Add(type, new List<FieldInfo>(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
            });
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
