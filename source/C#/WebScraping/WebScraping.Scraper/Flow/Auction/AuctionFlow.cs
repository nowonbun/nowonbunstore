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
using WebScraping.Scraper.Other;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class AuctionFlow : AbstractScrapFlow
    {
        private class State
        {
            public int StateIndex { get; set; }
            public int Page { get; set; }
            public DateTime sdt;
            public DateTime edt;
        }
        private String idkey;
        private String idcode;
        private State state = new State();
        private IDictionary<String, BuyDescisionNode> buyNodeList = new Dictionary<String, BuyDescisionNode>();

        public AuctionFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Action initialize");
            StartPageUrl = "https://www.esmplus.com/Member/SignIn/LogOn";
            FlowMap.Add("Member/SignIn/LogOn", Login);
            FlowMap.Add("Home/Home", Home);
            FlowMap.Add("membership/MyInfo/MyInfoComp", Profile);
            FlowMap.Add("Escrow/Delivery/BuyDecision", BuyDecisionSearch);
            browser.InitializeDownLoad(ExcelDownload);
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
        private Boolean BuyDecisionSearch(GeckoDocument document, Uri uri)
        {
            if (!String.IsNullOrEmpty(idkey))
            {
                //document.GetElementById<GeckoAnchorElement>("excelDown").Click();
                StringBuilder urlbuffer = new StringBuilder();
                urlbuffer.Append("https://www.esmplus.com/Escrow/Delivery/BuyDecisionExcel?")
                    .Append("siteGbn=0&searchAccount=")
                    .Append(idkey)
                    .Append("&searchDateType=TRD&searchSDT=")
                    .Append(state.sdt.ToString("yyyy-MM-dd"))
                    .Append("&searchEDT=")
                    .Append(state.edt.ToString("yyyy-MM-dd"))
                    .Append("&searchKey=ON&searchKeyword=&searchStatus=5010&searchAllYn=N&searchDistrType=AL&searchGlobalShopType=&searchOverseaDeliveryYn=");
                //base.Navigate(urlbuffer.ToString());
                logger.Debug(urlbuffer.ToString());
                PostAjaxJson(document, urlbuffer.ToString(), new Dictionary<String, Object>()
                {
                    {"eSortType","" },
                });
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
                    /*
                        page:1
                        limit:20
                        siteGbn:0
                        searchAccount:10757
                        searchDateType:TRD
                        searchSDT:2017-09-09
                        searchEDT:2017-10-09
                        searchKey:ON
                        searchKeyword:
                        searchStatus:5010
                        searchAllYn:N
                        SortFeild:TransDate
                        SortType:Desc
                        start:0
                        searchDistrType:AL
                        searchGlobalShopType:
                        searchOverseaDeliveryYn:
                    */
                    //1년전부터
                    state.StateIndex = 0;
                    state.Page = 1;
                    DateTime now = DateTime.Now;
                    state.sdt = now.AddYears(-1).AddDays(1);
                    state.edt = now;
                    StringBuilder urlbuffer = new StringBuilder();
                    urlbuffer.Append("https://www.esmplus.com/Escrow/Delivery/BuyDecision?")
                        .Append("gbn=0&status=5010&type=N&searchTotal=-&searchAccount=").Append(idkey)
                        .Append("&searchDateType=TRD&searchSDT=")
                        .Append(state.sdt.ToString("yyyy-MM-dd"))
                        .Append("&searchEDT=")
                        .Append(state.edt.ToString("yyyy-MM-dd"))
                        .Append("&searchKey=ON&searchKeyword=&searchStatus=5010&listAllView=false&searchDistrType=AL&searchGlobalShopType=&searchOverseaDeliveryYn=");

                    base.Navigate(urlbuffer.ToString());
                    return true;
                }
            }
            throw new ScraperException("Failed to get id key..");
        }

        private void ExcelDownload(String url, String file)
        {
            logger.Debug(url);
            logger.Debug(file);
            if (url.IndexOf("BuyDecisionExcel") != -1)
            {
                logger.Debug("BuyDecisionExcel");
                ThreadPool.QueueUserWorkItem((c) =>
                {
                    while (true)
                    {
                        if (File.Exists(file))
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    logger.Debug("Excel analysis");
                    BuilderExcelEntity<BuyDecisionExcel> builder = new BuilderExcelEntity<BuyDecisionExcel>();
                    List<BuyDecisionExcel> list = builder.Builder(file);
                    logger.Debug("It complete to build excel ");
                    foreach (var item in list)
                    {
                        try
                        {
                            BuyDescisionNode node = GetBuyDescisionNode(item.BuyDate);
                            node.DeliveryFee += item.DeliveryPrice;
                            node.OrderAmnt += item.ProductQuanity;
                            node.SellPrice += item.SellPrice;
                            node.SttlExpectedAmnt += item.ExpectPrice;
                        }
                        catch (Exception e)
                        {
                            logger.Error(item.OrderNumber);
                            logger.Error(e.ToString());
                        }
                    }
                    return;
                });
            }
        }

        private int TransInt(String data)
        {
            if (data == null)
            {
                return 0;
            }
            try
            {
                String buffer = data.Replace(",", "");
                return int.Parse(buffer);
            }
            catch
            {
                return 0;
            }
        }
        private Decimal TransDecimal(String data)
        {
            if (data == null)
            {
                return Decimal.Zero;
            }
            try
            {
                String buffer = data.Replace(",", "");
                return Decimal.Parse(buffer);
            }
            catch
            {
                return Decimal.Zero;
            }
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
