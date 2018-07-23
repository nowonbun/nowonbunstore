using System;
using System.Collections.Generic;
using WebScraping.Scraper.Common;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    partial class AuctionFlow : AbstractScrapFlow
    {
        private void BuyDecisionExcel(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {
                logger.Info("2-2.매출내역 ( 주문관리 > 구매결정완료 ) Excel");
                logger.Debug("BuyDecisionExcel");
                logger.Debug("BuyDecisionExcel Excel analysis");
                BuilderExcelEntity<BuyDecisionExcel> builder = new BuilderExcelEntity<BuyDecisionExcel>();
                List<BuyDecisionExcel> list = builder.Builder(data.File.FullName);
                logger.Debug("It complete to build excel ");
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(0, index++, ToExcelJson(ReflectFlyweight[typeof(BuyDecisionExcel)], item));
                }
                list.Clear();
                base.Navigate("http://www.esmplus.com/Member/Settle/IacSettleDetail?menuCode=TDM298");
            });
        }
        private void LacRemitListExcelDownload(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {
                logger.Info("3-2.정산내역 ( 정산관리 > 정산내역 조회 > 옥션 정산내역 관리 ) Excel");
                logger.Debug("IacRemitListExcelDownload");
                logger.Debug("IacRemitListExcelDownload Excel analysis");
                BuilderExcelEntity<LacRemitListExcel> builder = new BuilderExcelEntity<LacRemitListExcel>();
                List<LacRemitListExcel> list = builder.Builder(data.File.FullName);
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(1, index++, ToExcelJson(ReflectFlyweight[typeof(LacRemitListExcel)], item));
                }
                list.Clear();

                try
                {
                    DateTime enddate = DateTime.Now;
                    DateTime startdate = enddate.AddYears(-1).AddDays(1);
                    this.buffer.Append("https://www.esmplus.com/Escrow/Delivery/GeneralDelivery?");
                    this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                        {
                            {"gbn","0"},
                            {"status","0"},
                            {"type","" },
                            {"searchAccount",idcode+"^1" },
                            {"searchDateType","" },
                            {"searchSDT", startdate.ToString("yyyy-MM-dd")},
                            {"searchEDT", enddate.ToString("yyyy-MM-dd") },
                            {"searchKey","" },
                            {"searchKeyword","" },
                            {"searchDeliveryType","nomal" },
                            {"searchOrderType","" },
                            {"searchPacking","" },
                            {"totalAccumulate","-" },
                            {"searchTransPolicyType","" }
                        }));
                    base.Navigate(this.buffer.ToString());
                }
                finally
                {
                    this.buffer.Clear();
                }
            });
        }
        private void GeneralDeliveryExcel(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {
                logger.Info("4-2.정산예정금 - ( 주문관리 > 발송처리 ) Excel");
                logger.Debug("GeneralDeliveryExcel");
                logger.Debug("GeneralDeliveryExcel Excel analysis");

                BuilderExcelEntity<GeneralDeliveryExcel> builder = new BuilderExcelEntity<GeneralDeliveryExcel>();
                List<GeneralDeliveryExcel> list = builder.Builder(data.File.FullName);
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(2, index++, ToExcelJson(ReflectFlyweight[typeof(GeneralDeliveryExcel)], item));
                }
                list.Clear();
                base.Navigate("https://www.esmplus.com/Escrow/Delivery/Sending?menuCode=TDM111");
            });
        }
        private void SendingExcel(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {
                logger.Info("4-4.정산예정금 ( 주문관리 > 배송중/배송완료 )");
                logger.Debug("SendingExcel");
                logger.Debug("SendingExcel Excel analysis");
                BuilderExcelEntity<SendingExcel> builder = new BuilderExcelEntity<SendingExcel>();
                List<SendingExcel> list = builder.Builder(data.File.FullName);
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(3, index++, ToExcelJson(ReflectFlyweight[typeof(SendingExcel)], item));
                }
                list.Clear();

                //중복플로우 처리변경
                SetModifyBuyDecision();
                base.Navigate("https://www.esmplus.com/Escrow/Delivery/BuyDecision?menuCode=TDM112");
            });
        }
        private void BuyDecisionExcel2(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {
                logger.Info("4-5.정산예정금 ( 주문관리 > 구매결정완료 ) Excel");
                logger.Debug("BuyDecisionExcel2");
                logger.Debug("BuyDecisionExcel2 Excel analysis");
                BuilderExcelEntity<BuyDecisionExcel> builder = new BuilderExcelEntity<BuyDecisionExcel>();
                List<BuyDecisionExcel> list = builder.Builder(data.File.FullName);
                logger.Debug("It complete to build excel ");
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(4, index++, ToExcelJson(ReflectFlyweight[typeof(BuyDecisionExcel)], item));
                }
                list.Clear();
                base.Navigate("https://www.esmplus.com/Escrow/Claim/ReturnRequestManagement?menuCode=TDM118");
            });
        }
        private void ExcelDownload(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {
                logger.Info("5-2.반품율 (클레임관리 > 반품관리 ) Excel");
                logger.Debug("ExcelDownload");
                logger.Debug("ExcelDownload Excel analysis");
                BuilderExcelEntity<ReturnRequest> builder = new BuilderExcelEntity<ReturnRequest>();
                List<ReturnRequest> list = builder.Builder(data.File.FullName);
                logger.Debug("It complete to build excel ");
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(5, index++, ToExcelJson(ReflectFlyweight[typeof(ReturnRequest)], item));
                }
                list.Clear();

                base.Navigate("http://www.esmplus.com/Sell/Items/ItemsMng?menuCode=TDM100");
                //base.Navigate("http://www.esmplus.com/Areas/Manual/SellerGuide/main.html");
            });
        }
    }
}
