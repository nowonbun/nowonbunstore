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
                    {"searchDateType","ODD"},
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
        private bool Sending(GeckoDocument document, Uri uri)
        {
            //https://www.esmplus.com/Escrow/Delivery/SendingExcel?
            //siteGbn =1&searchAccount=10757^isorikids^1&searchDateType=ODD&searchSDT=2017-10-06&searchEDT=2017-11-06&searchKey=ON&searchKeyword=&searchStatus=0&searchAllYn=N&searchType=0&searchDistrType=AL
            logger.Info("4-3.정산예정금 - ( 주문관리 > 배송중/배송완료 )");
            try
            {
                this.buffer.Append("https://www.esmplus.com/Escrow/Delivery/SendingExcel?");
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                {
                    {"siteGbn","1"},
                    {"searchAccount",idkey },
                    {"searchDateType","ODD"},
                    {"searchSDT", startdate.ToString("yyyy-MM-dd")},
                    {"searchEDT", enddate.ToString("yyyy-MM-dd") },
                    {"searchKey","ON" },
                    {"searchKeyword","" },
                    {"searchStatus","0" },
                    {"searchAllYn","N" },
                    {"searchType","0" },
                    {"searchDistrType","AL" }
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
        private Boolean BuyDecision2(GeckoDocument document, Uri uri)
        {
            logger.Info("4-5.정산예정금 ( 주문관리 > 구매결정완료 )");
            try
            {
                //https://www.esmplus.com/Escrow/Delivery/BuyDecisionExcel?
                //siteGbn =0&searchAccount=10757^isorikids^1&searchDateType=TRD&searchSDT=2017-10-08&searchEDT=2017-11-08&searchKey=ON&searchKeyword=&searchStatus=1060&searchAllYn=N&searchDistrType=AL&searchGlobalShopType=&searchOverseaDeliveryYn=
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
                        {"searchStatus","1060" },
                        {"searchAllYn","N" },
                        {"searchDistrType","AL" },
                        {"searchGlobalShopType","" },
                        {"searchOverseaDeliveryYn","" }
                    }));
                logger.Debug(this.buffer.ToString());
                PostAjaxJson(document, this.buffer.ToString(), new Dictionary<String, Object>()
<<<<<<< HEAD
                {
                    {"eSortType","" },
                });
=======
                    {
                        {"eSortType","" },
                    });
>>>>>>> bdb554f7c080ce3d52802bd2f47abc5527557fcb
            }
            finally
            {
                this.buffer.Clear();
            }
            return true;
        }
        private Boolean ReturnRequestManagement(GeckoDocument document, Uri uri)
        {
            logger.Info("5-1.반품율 (클레임관리 > 반품관리 )");
            try
            {
                //https://www.esmplus.com/Escrow/Claim/ExcelDownload?
                //searchAccount=A1^isorikids
                this.buffer.Append("https://www.esmplus.com/Escrow/Claim/ExcelDownload?");
<<<<<<< HEAD
                logger.Debug("idkey=" + idkey);
=======
                logger.Debug("idkey="+idkey);
>>>>>>> bdb554f7c080ce3d52802bd2f47abc5527557fcb
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                    {
                        {"from","ReturnRequest" },
                        {"gridID","GEC012" },
                        {"type","A" },
                        {"searchAccount",idkey},
                        {"searchDateType","ODD"},
                        {"searchSDT", startdate.ToString("yyyy-MM-dd")},
                        {"searchEDT", enddate.ToString("yyyy-MM-dd")},
                        {"searchType","RR"},
                        {"searchKey","ON" },
                        {"searchKeyword","" },
                        {"searchStatus","RR" },
                        {"searchAllYn","N" },
                        {"searchFastRefundYn","" },
                    }));
                logger.Debug(this.buffer.ToString());
                PostAjaxJson(document, this.buffer.ToString(), new Dictionary<String, Object>()
<<<<<<< HEAD
                {
                    {"eSortType","" },
                });
=======
                    {
                        {"eSortType","" },
                    });
>>>>>>> bdb554f7c080ce3d52802bd2f47abc5527557fcb
            }
            finally
            {
                this.buffer.Clear();
            }
            return true;
        }
<<<<<<< HEAD
        private bool ItemsMng(GeckoDocument document, Uri uri)
        {
            /*document.GetElementByName<GeckoInputElement>("chkStatus", 1).Checked = true;
            document.GetElementByName<GeckoInputElement>("chkSiteId", 1).Checked = true;
            document.GetElementByName<GeckoInputElement>("chkSiteA", 0).Checked = true;
            document.GetElementById<GeckoImageElement>("imgItemsSearch").Click();*/
            this.buffer.Append("http://www.esmplus.com/Sell/Items/GetItemMngList");

            var SearchParam = new
            {
                Keyword = "",
                SiteId = "1",
                SellType = 0,
                CategoryCode = "",
                CustCategoryCode = 0,
                TransPolicyNo = 0,
                StatusCode = "11",
                SearchDateType = 0,
                StartDate = "",
                EndDate = "",
                SellerId = "",
                StockQty = -1,
                SellPeriod = 0,
                DeliveryFeeApplyType = 0,
                OptAddDeliveryType = 0,
                SellMinPrice = 0,
                SellMaxPrice = 0,
                OptSelUseIs = -1,
                PremiumEnd = 0,
                PremiumPlusEnd = 0,
                FocusEnd = 0,
                FocusPlusEnd = 0,
                CatalogGroup = 0,
                GoodsIds = "",
                SellMngCode = "",
                OrderByType = 11,
                NotiItemReg = -1,
                UserEvaluate = "",
                SearchClause = "",
                ScoreRange = 0,
                ShopCateReg = -1,
                IsTPLUse = "",
                GoodsName = "",
                SdBrandId = 0,
                SdBrandName = ""
            };
            String json = JsonConvert.SerializeObject(SearchParam);
            logger.Debug(this.buffer.ToString());
            PostAjaxJson(document, this.buffer.ToString(),new Dictionary<String, Object>()
            {
                {"paramsData",json },
                {"page","1" },
                {"start","0" },
                {"limit","500"},
                {"group","[{\"property\":\"GoodsMasterNo\",\"direction\":\"ASC\"}]"}
            });
            return true;
        }
        private bool GetItemMngList(GeckoDocument document, Uri uri)
        {
            String data = document.Body.GetElementsByTagName("PRE")[0].FirstChild.NodeValue;
            logger.Debug(data);
            return false;
        }
=======
>>>>>>> bdb554f7c080ce3d52802bd2f47abc5527557fcb
        private bool ScrapEnd(GeckoDocument document, Uri uri)
        {
            return false;
        }
    }
}
