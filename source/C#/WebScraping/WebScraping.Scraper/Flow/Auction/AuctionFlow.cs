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
        public AuctionFlow(ScrapBrowser browser, ScrapParameter param, bool login_mode)
            : base(browser, param, login_mode)
        {
            logger.Info("Action initialize");
            StartPageUrl = "https://www.esmplus.com/Member/SignIn/LogOn";
            FlowMap.Add("Member/SignIn/LogOn", Login);
            FlowMap.Add("Home/Home", Home);
            FlowMap.Add("membership/MyInfo/MyInfoComp", Profile);
            FlowMap.Add("Escrow/Delivery/BuyDecision", GetIDKey);
            FlowMap.Add("Escrow/Delivery/BuyDecisionSearch", BuyDecisionSearch);
        }
        protected override void Finally()
        {
            Console.WriteLine("End!");
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
        private Boolean GetIDKey(GeckoDocument document, Uri uri)
        {
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
                    state.sdt = now.AddMonths(-11).AddDays((now.Day * -1) + 1);
                    state.edt = state.sdt.AddMonths(1).AddDays(-1);
                    //DateTime sdt = edt.AddYears(-1).AddDays(edt.Day * -1).AddDays(1);

                    PostAjaxJson(document, "/Escrow/Delivery/BuyDecisionSearch", new Dictionary<String, Object>()
                    {
                       {"page",state.Page },
                       {"limit","500" },
                       {"siteGbn","0" },
                       {"searchAccount",idkey },
                       {"searchDateType","TRD" },
                       {"searchSDT",state.sdt.ToString("yyyy-MM-dd") },
                       {"searchEDT",state.edt.ToString("yyyy-MM-dd") },
                       {"searchKey","ON" },
                       {"searchKeyword","" },
                       {"searchStatus","5010" },
                       {"searchAllYn","N" },
                       {"SortFeild","TransDate" },
                       {"SortType","Desc" },
                       {"start","0" },
                       {"searchDistrType","AL" },
                       {"searchGlobalShopType","" },
                       {"searchOverseaDeliveryYn","" },
                    });
                    return true;
                    //DecisionSearch
                }
            }
            throw new ScraperException("Failed to get id key..");
        }
        private Boolean BuyDecisionSearch(GeckoDocument document, Uri uri)
        {
            String data = document.Body.TextContent;
            var json = JsonConvert.DeserializeObject<BuyDecisionSearchJson>(data);
            return true;
        }
    }
}
