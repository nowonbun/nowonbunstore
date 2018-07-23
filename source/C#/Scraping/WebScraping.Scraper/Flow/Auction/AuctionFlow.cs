using System;
using System.Collections.Generic;
using System.Text;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;
using System.Reflection;

namespace WebScraping.Scraper.Flow.Auction
{
    partial class AuctionFlow : AbstractScrapFlow
    {
        private String idkey;
        private String idcode;
        private StringBuilder buffer = new StringBuilder();


        public AuctionFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("Action initialize");

            FlowList.Add("Member/SignIn/LogOn", Login, null);
            FlowList.Add("Home/Home", Home, null);
            FlowList.Add("membership/MyInfo/MyInfoComp", Profile, null);
            FlowList.Add("Escrow/Delivery/BuyDecision", BuyDecision, null);
            FlowList.Add("Member/Settle/IacSettleDetail", LacSettleDetail, null);
            FlowList.Add("Escrow/Delivery/GeneralDelivery", GeneralDelivery, null);
            FlowList.Add("Escrow/Delivery/Sending", Sending, null);
            FlowList.Add("Areas/Manual/SellerGuide", ScrapEnd, null);
            FlowList.Add("Escrow/Claim/ReturnRequestManagement", ReturnRequestManagement, null);
            FlowList.Add("Sell/Items/ItemsMng", ItemsMng, null);
            FlowList.Add("Sell/Items/GetItemMngList", GetItemMngList, null);
            FlowList.Add("Member/CustomerService/CSManagement", CSManagement, null);

            FlowList.Add("BuyDecisionExcel", BuyDecisionExcel, null);
            FlowList.Add("IacRemitListExcelDownload", LacRemitListExcelDownload, null);
            FlowList.Add("GeneralDeliveryExcel", GeneralDeliveryExcel, null);
            FlowList.Add("SendingExcel", SendingExcel, null);
            FlowList.Add("ExcelDownload", ExcelDownload, null);

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

        private void SetModifyBuyDecision()
        {
            //4-5.정산예정금 ( 주문관리 > 구매결정완료 )
            FlowList["Escrow/Delivery/BuyDecision"] = new Common.Flow
            {
                Func = BuyDecision2
            };
            //DownloadMap["BuyDecisionExcel"] = BuyDecisionExcel2;
        }
        protected override void Finally(FlowModelData flowModelData)
        {
            logger.Info("Action scraping is exit");
        }
        public override string StartPage(FlowModelData flowModelData)
        {
            return "https://www.esmplus.com/Member/SignIn/LogOn";
        }
    }
}
