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
    class AuctionFlow : AbstractScrapFlow
    {
        private String idkey;
        private String idcode;
        public DateTime startdate;
        public DateTime enddate;
        private IDictionary<String, BuyDescisionNode> buyNodeList = new Dictionary<String, BuyDescisionNode>();
        private StringBuilder buffer = new StringBuilder();
        private IList<FieldInfo> buyDescisionExcelReflect = null;
        private IList<FieldInfo> lacRemitListExcelReflect = null;

        public AuctionFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Action initialize");
            buyDescisionExcelReflect = new List<FieldInfo>(typeof(BuyDecisionExcel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            lacRemitListExcelReflect = new List<FieldInfo>(typeof(LacRemitListExcel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

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
            FlowMap.Add("Areas/Manual/SellerGuide", ScrapEnd);
            FlowMap.Add("Escrow/Delivery/GeneralDelivery", GeneralDelivery);
            DownloadMap.Add("BuyDecisionExcel", BuyDecisionExcel);
            DownloadMap.Add("IacRemitListExcelDownload", LacRemitListExcelDownload);
            DownloadMap.Add("GeneralDeliveryExcel", GeneralDeliveryExcel);
        }
        protected override void Finally()
        {
            logger.Info("Action scraping is exit");
        }
        private Boolean Login(GeckoDocument document, Uri uri)
        {
            if (uri.ToString().IndexOf("ReturnValue") != -1)
            {
                //String label = document.GetElementByClassName<GeckoHtmlElement>("login_text", 0).TextContent;
                //Console.WriteLine(label);
                SetCommonData(0, "FALSE");
                return false;
            }
            document.GetElementByName<GeckoInputElement>("rdoSiteSelect", 0).Checked = true;
            document.GetElementById<GeckoInputElement>("SiteId").Value = Parameter.Id;
            document.GetElementById<GeckoInputElement>("SitePassword").Value = Parameter.Pw;
            document.GetElementById<GeckoAnchorElement>("btnSiteLogOn").Click();
            SetCommonData(0, "TRUE");
            return true;
        }
        private Boolean Home(GeckoDocument document, Uri uri)
        {
            /*String value = document.GetElementById<GeckoHtmlElement>("header")
                                      .GetElementByTagName<GeckoHtmlElement>("DIV", 0)
                                          .GetElementByTagName<GeckoHtmlElement>("SPAN", 0)
                                              .GetElementByTagName<GeckoHtmlElement>("STRONG", 0).FirstChild.NodeValue;
            Console.WriteLine(value);*/
            base.Navigate("http://www.esmplus.com/Home/SSO?code=TDM155&id=" + Parameter.Id);
            return true;
        }
        private Boolean Profile(GeckoDocument document, Uri uri)
        {
            SetCommonData(1, document.GetElementByIdToNodeValue("lblCustName"));
            SetCommonData(3, document.GetElementByIdToNodeValue("lblPresident"));
            SetCommonData(4, document.GetElementByIdToNodeValue("lblRegiNumber"));
            String buffer = document.GetElementByIdToNodeValue("lblBizCateKind");
            if (buffer != null)
            {
                String[] temp = buffer.Split('/');
                SetCommonData(7, temp[0]);
                SetCommonData(8, temp[1]);
            }
            String tel1 = document.GetElementById<GeckoSelectElement>("ddlMobileTel").Value;
            String tel2 = document.GetElementById<GeckoInputElement>("txtMobileTel2").Value;
            String tel3 = document.GetElementById<GeckoInputElement>("txtMobileTel3").Value;
            SetCommonData(9, tel1 + "-" + tel2 + "-" + tel3);
            String fax1 = document.GetElementById<GeckoSelectElement>("ddlOfficeFax").Value;
            String fax2 = document.GetElementById<GeckoInputElement>("txtOfficeFax2").Value;
            String fax3 = document.GetElementById<GeckoInputElement>("txtOfficeFax3").Value;
            SetCommonData(11, fax1 + "-" + fax2 + "-" + fax3);
            String hp1 = document.GetElementById<GeckoSelectElement>("ddlMobileTel").Value;
            String hp2 = document.GetElementById<GeckoInputElement>("txtMobileTel2").Value;
            String hp3 = document.GetElementById<GeckoInputElement>("txtMobileTel3").Value;
            SetCommonData(10, hp1 + "-" + hp2 + "-" + hp3);
            String mail1 = document.GetElementById<GeckoInputElement>("txtEmailId").Value;
            String mail2 = document.GetElementById<GeckoInputElement>("txtEmailDomain").Value;
            SetCommonData(13, mail1 + "@" + mail2);
            SetCommonData(14, document.GetElementByIdToNodeValue("bn"));
            SetCommonData(15, document.GetElementById<GeckoInputElement>("txtName").Value);
            SetCommonData(16, document.GetElementById<GeckoInputElement>("txtAcctNumb").Value);

            base.Navigate("https://www.esmplus.com/Escrow/Delivery/BuyDecision");
            return true;
        }
        private Boolean BuyDecision(GeckoDocument document, Uri uri)
        {
            logger.Info("2-1.매출내역 ( 주문관리 > 구매결정완료 )");
            if (!String.IsNullOrEmpty(idkey))
            {
                try
                {
                    this.buffer.Append("https://www.esmplus.com/Escrow/Delivery/BuyDecisionExcel?");
                    this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                    {
                        {"siteGbn","0"},
                        {"searchAccount",idkey},
                        {"searchDateType","TRD"},
                        {"searchSDT", startdate.ToString("yyyy-MM-dd")},
                        {"searchEDT", enddate.ToString("yyyy-MM-dd")},
                        {"searchKey","ON" },
                        {"searchKeyword","" },
                        {"searchStatus","5010" },
                        {"searchAllYn","N" },
                        {"searchDistrType","AL" },
                        {"searchGlobalShopType","" },
                        {"searchOverseaDeliveryYn","" }
                    }));
                    logger.Debug(this.buffer.ToString());
                    PostAjaxJson(document, this.buffer.ToString(), new Dictionary<String, Object>()
                    {
                        {"eSortType","" },
                    });
                }
                finally
                {
                    this.buffer.Clear();
                }
                return true;
            }
            GeckoSelectElement item = document.GetElementById<GeckoSelectElement>("searchAccount");
            for (uint i = 0; i < item.Length; i++)
            {
                GeckoOptionElement option = item.Options.item(i);
                if (String.Equals(option.Label, "A_" + Parameter.Id))
                {
                    //10757^id^_1
                    idkey = option.Value;
                    idcode = idkey.Split('^')[0];
                    this.logger.Info("idkey - " + idkey);
                    this.logger.Info("idcode - " + idcode);
                    try
                    {
                        this.buffer.Append("https://www.esmplus.com/Escrow/Delivery/BuyDecision?");
                        this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                        {
                            {"siteGbn","0"},
                            {"status","5010"},
                            {"type","N" },
                            {"searchTotal","-" },
                            {"searchAccount",idkey},
                            {"searchDateType","TRD"},
                            {"searchSDT", startdate.ToString("yyyy-MM-dd")},
                            {"searchEDT", enddate.ToString("yyyy-MM-dd")},
                            {"searchKey","ON" },
                            {"searchKeyword","" },
                            {"searchStatus","5010" },
                            {"listAllView","false" },
                            {"searchDistrType","AL" },
                            {"searchGlobalShopType","" },
                            {"searchOverseaDeliveryYn","" }
                        }));
                        base.Navigate(this.buffer.ToString());
                    }
                    finally
                    {
                        this.buffer.Clear();
                    }
                    return true;
                }
            }
            throw new ScraperException("Failed to get id key..");
        }
        private bool LacSettleDetail(GeckoDocument document, Uri uri)
        {
            logger.Info("3-1.정산내역 ( 정산관리 > 정산내역 조회 > 옥션 정산내역 관리 )");
            var SearchParam = new
            {
                MemberID = Parameter.Id,
                ItemNo = "",
                BuyerName = "",
                BuyerId = "",
                OrderNo = 0,
                DateType = "R",
                DateFrom = startdate.ToString("yyyy-MM-dd"),
                DateTo = enddate.ToString("yyyy-MM-dd"),
                CategoryId = "",
                RemittanceType = "0",
                GroupOrderSeqNo = "0",
                PageNo = 1,
                PageSize = 20
            };
            String json = JsonConvert.SerializeObject(SearchParam);
            logger.Debug(json);
            PostAjaxJson(document, "https://www.esmplus.com/Member/Settle/IacRemitListExcelDownload?SearchParam=" + json, new Dictionary<String, Object>()
            {
                {"eSortType","" },
            });
            return true;
        }
        private bool GeneralDelivery(GeckoDocument document, Uri uri)
        {
            logger.Info("4-1.정산예정금 - ( 주문관리 > 발송처리 )");
            try
            {
                //https://www.esmplus.com/Escrow/Delivery/GeneralDeliveryExcel?
                //siteGbn=0&searchAccount=10757^1&searchDateType=ODD&searchSDT=2017-08-05&searchEDT=2017-11-05&searchKey=ON&searchKeyword=&searchStatus=0&
                //searchAllYn =Y&splitYn=no&searchDeliveryType=&searchOrderType=&searchPaking=false&searchDistrType=AL&searchTransPolicyType=
                this.buffer.Append("https://www.esmplus.com/Escrow/Delivery/GeneralDeliveryExcel?");
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                {
                    {"siteGbn","0"},
                    {"searchAccount",idcode+"^1" },
                    {"searchDateType","0DD"},
                    {"searchSDT", startdate.ToString("yyyy-MM-dd")},
                    {"searchEDT", enddate.ToString("yyyy-MM-dd") },
                    {"searchKey","ON" },
                    {"searchKeyword","" },
                    {"searchStatus","0" },
                    {"searchAllYn","Y" },
                    {"splitYn","no" },
                    {"searchDeliveryType","nomal" },
                    {"searchOrderType","" },
                    {"searchPaking","false" },
                    {"searchDistrType","AL" },
                    {"searchTransPolicyType","" }
                }));
                PostAjaxJson(document, this.buffer.ToString(), new Dictionary<String, Object>()
                {
                    {"eSortType","" },
                });
            }
            finally
            {
                this.buffer.Clear();
            }
            return true;
        }
        private bool ScrapEnd(GeckoDocument document, Uri uri)
        {
            return false;
        }

        private void BuyDecisionExcel(String url, String file)
        {
            WaitFile(file, () =>
            {
                logger.Info("2-2.매출내역 ( 주문관리 > 구매결정완료 ) Excel");
                logger.Debug("BuyDecisionExcel");
                logger.Debug("BuyDecisionExcel Excel analysis");
                BuilderExcelEntity<BuyDecisionExcel> builder = new BuilderExcelEntity<BuyDecisionExcel>();
                List<BuyDecisionExcel> list = builder.Builder(file);
                logger.Debug("It complete to build excel ");
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(0, index++, ToJson(buyDescisionExcelReflect, item));
                }
                list.Clear();
                base.Navigate("http://www.esmplus.com/Member/Settle/IacSettleDetail?menuCode=TDM298");
            });
        }
        private void LacRemitListExcelDownload(String url, String file)
        {
            WaitFile(file, () =>
            {
                logger.Info("3-2.정산내역 ( 정산관리 > 정산내역 조회 > 옥션 정산내역 관리 ) Excel");
                logger.Debug("IacRemitListExcelDownload");
                logger.Debug("IacRemitListExcelDownload Excel analysis");
                BuilderExcelEntity<LacRemitListExcel> builder = new BuilderExcelEntity<LacRemitListExcel>();
                List<LacRemitListExcel> list = builder.Builder(file);
                int index = 0;
                foreach (var item in list)
                {
                    SetPackageData(1, index++, ToJson(lacRemitListExcelReflect, item));
                }
                list.Clear();

                try
                {
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
        private void GeneralDeliveryExcel(String url, String file)
        {
            WaitFile(file, () =>
            {
                logger.Info("4-2.정산예정금 - ( 주문관리 > 발송처리 ) Excel");
                logger.Debug("GeneralDeliveryExcel");
                logger.Debug("GeneralDeliveryExcel Excel analysis");

                base.Navigate("http://www.esmplus.com/Areas/Manual/SellerGuide/main.html");
            });
        }

        private BuyDescisionNode GetBuyDescisionNode(String date)
        {
            DateTime dt = DateTime.Parse(date);
            date = dt.ToString("yyyy-MM");
            if (!buyNodeList.ContainsKey(date))
            {
                BuyDescisionNode node = new BuyDescisionNode();
                node.YearMonth = DateTime.Parse(date + "-01");
                node.DeliveryFee = 0;
                node.OrderAmnt = 0;
                node.SellPrice = 0;
                node.SttlExpectedAmnt = 0;
                buyNodeList.Add(date, node);
            }
            return buyNodeList[date];
        }

    }
}
