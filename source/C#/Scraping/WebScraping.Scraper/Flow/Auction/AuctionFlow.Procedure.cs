using System;
using System.Collections.Generic;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Other;
using Newtonsoft.Json;

namespace WebScraping.Scraper.Flow.Auction
{
    partial class AuctionFlow : AbstractScrapFlow
    {
        private void Login(FlowModelData data)
        {
            if (data.Uri.ToString().IndexOf("ReturnValue") != -1)
            {
                SetCommonData(0, "FALSE");
            }
            data.Document.GetElementByName<GeckoInputElement>("rdoSiteSelect", 0).Checked = true;
            data.Document.GetElementById<GeckoInputElement>("SiteId").Value = Parameter.Id1;
            data.Document.GetElementById<GeckoInputElement>("SitePassword").Value = Parameter.Pw1;
            data.Document.GetElementById<GeckoAnchorElement>("btnSiteLogOn").Click();
        }
        private void Home(FlowModelData data)
        {
            SetCommonData(0, "TRUE");
            base.Navigate("https://www.esmplus.com/Member/CustomerService/CSManagement?menuCode=TDM140");
        }
        private void CSManagement(FlowModelData data)
        {
            var val = data.Document.GetElementById<GeckoElement>("msg_auction").GetElementsByTagName("A")[0].FirstChild.NodeValue;
            SetCommonData(20, val);
            base.Navigate("http://www.esmplus.com/Home/SSO?code=TDM155&id=" + Parameter.Id1);
        }

        private void Profile(FlowModelData data)
        {
            SetCommonData(1, data.Document.GetElementByIdToNodeValue("lblCustName"));
            SetCommonData(3, data.Document.GetElementByIdToNodeValue("lblPresident"));
            SetCommonData(4, data.Document.GetElementByIdToNodeValue("lblRegiNumber"));
            String buffer = data.Document.GetElementByIdToNodeValue("lblBizCateKind");
            if (buffer != null)
            {
                String[] temp = buffer.Split('/');
                SetCommonData(7, temp[0]);
                SetCommonData(8, temp[1]);
            }
            String tel1 = data.Document.GetElementById<GeckoSelectElement>("ddlMobileTel").Value;
            String tel2 = data.Document.GetElementById<GeckoInputElement>("txtMobileTel2").Value;
            String tel3 = data.Document.GetElementById<GeckoInputElement>("txtMobileTel3").Value;
            SetCommonData(9, tel1 + "-" + tel2 + "-" + tel3);
            String fax1 = data.Document.GetElementById<GeckoSelectElement>("ddlOfficeFax").Value;
            String fax2 = data.Document.GetElementById<GeckoInputElement>("txtOfficeFax2").Value;
            String fax3 = data.Document.GetElementById<GeckoInputElement>("txtOfficeFax3").Value;
            SetCommonData(11, fax1 + "-" + fax2 + "-" + fax3);
            String hp1 = data.Document.GetElementById<GeckoSelectElement>("ddlMobileTel").Value;
            String hp2 = data.Document.GetElementById<GeckoInputElement>("txtMobileTel2").Value;
            String hp3 = data.Document.GetElementById<GeckoInputElement>("txtMobileTel3").Value;
            SetCommonData(10, hp1 + "-" + hp2 + "-" + hp3);
            String mail1 = data.Document.GetElementById<GeckoInputElement>("txtEmailId").Value;
            String mail2 = data.Document.GetElementById<GeckoInputElement>("txtEmailDomain").Value;
            SetCommonData(13, mail1 + "@" + mail2);
            SetCommonData(14, data.Document.GetElementByIdToNodeValue("bn"));
            SetCommonData(15, data.Document.GetElementById<GeckoInputElement>("txtName").Value);
            SetCommonData(16, data.Document.GetElementById<GeckoInputElement>("txtAcctNumb").Value);

            base.Navigate("https://www.esmplus.com/Escrow/Delivery/BuyDecision");
        }
        private void BuyDecision(FlowModelData data)
        {
            logger.Info("2-1.매출내역 ( 주문관리 > 구매결정완료 )");
            if (!String.IsNullOrEmpty(idkey))
            {
                try
                {
                    DateTime enddate = DateTime.Now;
                    DateTime startdate = enddate.AddYears(-1).AddDays(1);
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
                    PostAjaxJson(data.Document, this.buffer.ToString(), new Dictionary<String, Object>()
                    {
                        {"eSortType","" },
                    });
                }
                finally
                {
                    this.buffer.Clear();
                }
            }
            GeckoSelectElement item = data.Document.GetElementById<GeckoSelectElement>("searchAccount");
            for (uint i = 0; i < item.Length; i++)
            {
                GeckoOptionElement option = item.Options.item(i);
                if (String.Equals(option.Label, "A_" + Parameter.Id1))
                {
                    //10757^id^_1
                    DateTime enddate = DateTime.Now;
                    DateTime startdate = enddate.AddYears(-1).AddDays(1);
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
                    return;
                }
            }
            throw new ScraperException("Failed to get id key..");
        }
        private void LacSettleDetail(FlowModelData data)
        {
            logger.Info("3-1.정산내역 ( 정산관리 > 정산내역 조회 > 옥션 정산내역 관리 )");
            DateTime enddate = DateTime.Now;
            DateTime startdate = enddate.AddYears(-1).AddDays(1);
            var SearchParam = new
            {
                MemberID = Parameter.Id1,
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
            PostAjaxJson(data.Document, "https://www.esmplus.com/Member/Settle/IacRemitListExcelDownload?SearchParam=" + json, new Dictionary<String, Object>()
            {
                {"eSortType","" },
            });
        }
        private void GeneralDelivery(FlowModelData data)
        {
            logger.Info("4-1.정산예정금 - ( 주문관리 > 발송처리 )");
            try
            {
                //https://www.esmplus.com/Escrow/Delivery/GeneralDeliveryExcel?
                //siteGbn=0&searchAccount=10757^1&searchDateType=ODD&searchSDT=2017-08-05&searchEDT=2017-11-05&searchKey=ON&searchKeyword=&searchStatus=0&
                //searchAllYn =Y&splitYn=no&searchDeliveryType=&searchOrderType=&searchPaking=false&searchDistrType=AL&searchTransPolicyType=
                DateTime enddate = DateTime.Now;
                DateTime startdate = enddate.AddMonths(-3);
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
                PostAjaxJson(data.Document, this.buffer.ToString(), new Dictionary<String, Object>()
                {
                    {"eSortType","" },
                });
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        private void Sending(FlowModelData data)
        {
            //https://www.esmplus.com/Escrow/Delivery/SendingExcel?
            //siteGbn =1&searchAccount=10757^isorikids^1&searchDateType=ODD&searchSDT=2017-10-06&searchEDT=2017-11-06&searchKey=ON&searchKeyword=&searchStatus=0&searchAllYn=N&searchType=0&searchDistrType=AL
            logger.Info("4-3.정산예정금 - ( 주문관리 > 배송중/배송완료 )");
            try
            {
                DateTime enddate = DateTime.Now;
                DateTime startdate = enddate.AddMonths(-1);
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
                PostAjaxJson(data.Document, this.buffer.ToString(), new Dictionary<String, Object>()
                {
                    {"eSortType","" },
                });
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        private void BuyDecision2(FlowModelData data)
        {
            logger.Info("4-5.정산예정금 ( 주문관리 > 구매결정완료 )");
            try
            {
                DateTime enddate = DateTime.Now;
                DateTime startdate = enddate.AddMonths(-1);
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
                PostAjaxJson(data.Document, this.buffer.ToString(), new Dictionary<String, Object>()
                {
                    {"eSortType","" },
                });
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        private void ReturnRequestManagement(FlowModelData data)
        {
            logger.Info("5-1.반품율 (클레임관리 > 반품관리 )");
            try
            {
                //https://www.esmplus.com/Escrow/Claim/ExcelDownload?
                //searchAccount=A1^isorikids
                DateTime enddate = DateTime.Now;
                DateTime startdate = enddate.AddYears(-1).AddDays(1);
                this.buffer.Append("https://www.esmplus.com/Escrow/Claim/ExcelDownload?");
                logger.Debug("idkey=" + idkey);
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
                PostAjaxJson(data.Document, this.buffer.ToString(), new Dictionary<String, Object>()
                {
                    {"eSortType","" },
                });
            }
            finally
            {
                this.buffer.Clear();
            }
        }
        private void ItemsMng(FlowModelData data)
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
            PostAjaxJson(data.Document, this.buffer.ToString(), new Dictionary<String, Object>()
            {
                {"paramsData",json },
                {"page","1" },
                {"start","0" },
                {"limit","500"},
                {"group","[{\"property\":\"GoodsMasterNo\",\"direction\":\"ASC\"}]"}
            });
        }
        private void GetItemMngList(FlowModelData data)
        {
            String val = data.Document.Body.GetElementsByTagName("PRE")[0].FirstChild.NodeValue;
            //logger.Debug(data);
            GetItemMngListJson json = JsonConvert.DeserializeObject<GetItemMngListJson>(val);
            int index = 0;
            foreach (var item in json.Data)
            {
                SetPackageData(6, index++, JsonConvert.SerializeObject(item));
            }
        }
        private void ScrapEnd(FlowModelData data)
        {
        }
    }
}
