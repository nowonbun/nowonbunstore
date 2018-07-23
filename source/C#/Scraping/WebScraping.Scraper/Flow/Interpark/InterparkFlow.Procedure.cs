using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Scraper.Common;
using Gecko;
using Gecko.DOM;
using WebScraping.Scraper.Impl;
using Newtonsoft.Json;
using System.Threading;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Interpark
{
    partial class InterparkFlow : AbstractScrapFlow
    {
        private void Login(FlowModelData data)
        {
            PostAjaxJson(data.Document, "https://ipss.interpark.com/member/login.do", new Dictionary<String, Object>()
            {
                {"_method","loginIframe" }
            });
        }
        private void Login2(FlowModelData data)
        {
            data.Document.GetElementByName<GeckoInputElement>("sc.memId").Value = Parameter.Id1;
            data.Document.GetElementByName<GeckoInputElement>("sc.pwd").Value = Parameter.Pw1;
            data.Document.GetElementByClassName<GeckoButtonElement>("btnRed").Click();
            SetTimeout(() =>
            {
                if (CurrectUri.ToString().Contains("member/login.do"))
                {
                    SetCommonData(0, "FALSE");
                    Response.SetResultCode(ResultCode.RC2000);
                    Exit(data);
                }
            }, 3000);
        }
        private void ScrapEnd(FlowModelData data)
        {
            Response.SetResultCode(ResultCode.RC1000);
            data.IsNextScrap = false;
        }
        private void Main(FlowModelData data)
        {
            SetCommonData(0, "TRUE");
            data.IsNextScrap = true;
            FlowList["ipss/ipssmainscr.do"] = new Common.Flow
            {
                Url = "ipss/ipssmainscr.do",
                Func = Main2,
                Next = "http://ipss.interpark.com/ipss/ipssmainscr.do?_method=urgentCount&_style=ipssPro"
            };
        }
        private void Main2(FlowModelData data)
        {
            String val = data.Document.GetElementsByClassName("line")[0].ChildNodes[3].FirstChild.TextContent;
            SetCommonData(2, val);
            FlowList["ipss/ipssmainscr.do"] = new Common.Flow
            {
                Url = "ipss/ipssmainscr.do",
                Func = Main3,
                Next = "https://ipss.interpark.com/member/memberentrjoin.do?_method=detail&_style=ipssPro#won"
            };
        }
        private void Main3(FlowModelData data)
        {
            String temp = data.Document.Body.TextContent;
            var node = JsonConvert.DeserializeObject<Dictionary<String, Object>>(temp);
            SetCommonData(20, node["urgentQty"].ToString());
        }
        private void MemberentrJoin(FlowModelData data)
        {
            ScrapTable table = data.Document.SelectTableByClass("ta_info");
            SetCommonData(1, table.Get(0, 1).TextContent);
            SetCommonData(4, table.Get(0, 3).TextContent);
            String temp = table.Get(1, 1).TextContent;
            var businessType = temp.Split('/');
            SetCommonData(7, businessType[0]);
            SetCommonData(8, businessType[1]);
            SetCommonData(3, table.Get(2, 1).TextContent);
            SetCommonData(28, table.Get(3, 1).GetElementByTagName<GeckoHtmlElement>("SPAN").GetElementByTagName<GeckoHtmlElement>("SPAN").TextContent);
            SetCommonData(12, table.Get(6, 1).TextContent.Replace("\t", "").Replace("\n", ""));
            string email = data.Document.GetElementByName<GeckoInputElement>("entrMainEmail1").Value +
                           "@" +
                           data.Document.GetElementByName<GeckoInputElement>("entrMainEmail2").Value;
            SetCommonData(10, email);
            String tel = data.Document.GetElementByName<GeckoInputElement>("telno1").Value +
                         "-" +
                         data.Document.GetElementByName<GeckoInputElement>("telno2").Value +
                         "-" +
                         data.Document.GetElementByName<GeckoInputElement>("telno3").Value;
            SetCommonData(9, tel);
            String fax = data.Document.GetElementByName<GeckoInputElement>("faxNo1").Value +
                         "-" +
                         data.Document.GetElementByName<GeckoInputElement>("faxNo2").Value +
                         "-" +
                         data.Document.GetElementByName<GeckoInputElement>("faxNo3").Value;
            SetCommonData(11, fax);

            String mainAdminMpNo = data.Document.GetElementByName<GeckoInputElement>("mainAdminMpNo1").Value +
                         "-" +
                         data.Document.GetElementByName<GeckoInputElement>("mainAdminMpNo2").Value +
                         "-" +
                         data.Document.GetElementByName<GeckoInputElement>("mainAdminMpNo3").Value;
            SetCommonData(5, mainAdminMpNo);

            SetCommonData(14, data.Document.GetElementByName<GeckoSelectElement>("supplyContractForm.bankCd").TextContent.Replace("\t", "").Replace("\n", ""));
            SetCommonData(15, data.Document.GetElementByName<GeckoInputElement>("supplyContractForm.acctNo").Value);
            SetCommonData(16, data.Document.GetElementByName<GeckoInputElement>("supplyContractForm.bnfNm").Value);
        }
        private void ProOMSetlDetail(FlowModelData data)
        {
            if (!data.DataPack.ContainsKey("index"))
            {
                data.DataPack["index"] = 0;
            }
            String val = data.Document.Body.InnerHtml;
            ProOMSetlDetailJson json = JsonConvert.DeserializeObject<ProOMSetlDetailJson>(val);
            SetPackageData(0, data.DataPack["index"]++, JsonConvert.SerializeObject(json.bs[0]));
            int nowtick = (DateTime.Now.Year * 100) + DateTime.Now.Month;
            int checktick = (data.DataPack["st"].Year * 100) + data.DataPack["st"].Month;
            if (nowtick - 300 > checktick)
            {
                return;
            }
            data.DataPack["st"] = data.DataPack["st"].AddMonths(-1);
            buffer.Append("http://ipss.interpark.com/settlement/ProOMSetlDetail.do?");
            this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                    {
                        {"_method","totalPro"},
                        {"_style","grid"},
                        {"sc.strDt", data.DataPack["st"].ToString("yyyyMMdd")},
                        {"sc.endDt",  data.DataPack["st"].AddMonths(1).AddDays(-1).ToString("yyyyMMdd")},
                        {"_search", "false"},
                        {"nd","" },
                        {"rows","30" },
                        {"page","1" },
                        {"sidx","" },
                        {"sord","asc" }
                    }));
            this.Navigate(this.buffer.ToString());
            data.IsSkipAction = true;
        }
        private void ProDeliveryCheckList(FlowModelData data)
        {
            if (!data.DataPack.ContainsKey("index"))
            {
                data.DataPack["index"] = 0;
            }
            String json = data.Document.Body.TextContent;
            SetPackageData(1, data.DataPack["index"]++, json);
        }

        private void ProDeliveryStatusList(FlowModelData data)
        {
            String json = data.Document.Body.TextContent;
            SetPackageData(1, data.DataPack["index"]++, json);
        }

        private void ProBuyConfirmList(FlowModelData data)
        {
            String json = data.Document.Body.TextContent;
            SetPackageData(1, data.DataPack["index"]++, json);
        }

        private void ReportEntrExcel(FlowModelData data)
        {
            WaitFile(data.File.FullName, () =>
            {

                /*BuilderExcelEntity<ReportEntrExcel> builder = new BuilderExcelEntity<ReportEntrExcel>();
                List<ReportEntrExcel> list = builder.Builder(data.File.FullName, 7, 0);
                foreach (var item in list)
                {
                    //SetPackageData(0, index++, ToExcelJson(ReflectFlyweight[typeof(BuyDecisionExcel)], item));
                }
                list.Clear();
                base.Navigate("http://www.esmplus.com/Member/Settle/IacSettleDetail?menuCode=TDM298");*/
            });
        }
    }
}
