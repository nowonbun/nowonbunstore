using System;
using System.Text;
using WebScraping.Scraper.Common;
using System.Collections.Generic;
using WebScraping.Scraper.Impl;

namespace WebScraping.Scraper.Flow.Interpark
{
    partial class InterparkFlow : AbstractScrapFlow
    {
        private StringBuilder buffer = new StringBuilder();

        public InterparkFlow(ScrapBrowser browser, Parameter param)
            : base(browser, param)
        {
            logger.Info("InterparkFlow initialize");
        }
        protected override void ScrapType0(FlowModelData flowModelData)
        {
            FlowList.Add("stdservice/ipssCheckLogin.do", Login, null);
            FlowList.Add("member/login.do", Login2, null);
            FlowList.Add("ipss/ipssmainscr.do", Main, "http://ipss.interpark.com/ipss/ipssmainscr.do?_method=initTabMain&_style=ipssPro&newType=Y");
            FlowList.Add("member/memberentrjoin.do", MemberentrJoin, null, (data) =>
            {
                Response.SetResultCode(ResultCode.RC1000);
                data.IsNextScrap = false;
            });
        }
        protected override void ScrapType1(FlowModelData flowModelData)
        {
            FlowList.Add("stdservice/ipssCheckLogin.do", Login, null);
            FlowList.Add("member/login.do", Login2, null, (data) =>
             {
                 data.DataPack["st"] = DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.Day).AddDays(1);
             });
            FlowList.Add("ipss/ipssmainscr.do", Main, null, (data) =>
             {
                 buffer.Clear();
                 buffer.Append("http://ipss.interpark.com/settlement/ProOMSetlDetail.do?");
                 this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                     {
                        {"_method","totalPro"},
                        {"_style","grid"},
                        {"sc.strDt",data.DataPack["st"].ToString("yyyyMMdd")},
                        {"sc.endDt", data.DataPack["st"].AddMonths(1).AddDays(-1).ToString("yyyyMMdd")},
                        {"_search", "false"},
                        {"nd","" },
                        {"rows","30" },
                        {"page","1" },
                        {"sidx","" },
                        {"sord","asc" }
                     }));
                 base.Navigate(buffer.ToString());
             });
            FlowList.Add("settlement/ProOMSetlDetail.do", ProOMSetlDetail, null, (data) =>
             {
                 Response.SetResultCode(ResultCode.RC1000);
                 data.IsNextScrap = false;
             });
        }
        protected override void ScrapType2(FlowModelData flowModelData)
        {
            FlowList.Add("stdservice/ipssCheckLogin.do", Login, null);
            FlowList.Add("member/login.do", Login2, null, (data) =>
            {
                data.DataPack["st"] = DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.Day).AddDays(1);
            });
            FlowList.Add("ipss/ipssmainscr.do", Main, null, (data) =>
            {
                buffer.Clear();
                buffer.Append("http://ipss.interpark.com/settlement/ProOMSetlDetail.do?");
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                {
                    {"_method","totalPro"},
                    {"_style","grid"},
                    {"sc.strDt",data.DataPack["st"].ToString("yyyyMMdd")},
                    {"sc.endDt", data.DataPack["st"].AddMonths(1).AddDays(-1).ToString("yyyyMMdd")},
                    {"_search", "false"},
                    {"nd","" },
                    {"rows","30" },
                    {"page","1" },
                    {"sidx","" },
                    {"sord","asc" }
                }));
                base.Navigate(buffer.ToString());
            });
            FlowList.Add("settlement/ProOMSetlDetail.do", ProOMSetlDetail, null, (data) =>
            {
                Response.SetResultCode(ResultCode.RC1000);
                data.IsNextScrap = false;
            });
        }
        protected override void ScrapType3(FlowModelData flowModelData)
        {
            FlowList.Add("stdservice/ipssCheckLogin.do", Login, null);
            FlowList.Add("member/login.do", Login2, null);
            FlowList.Add("ipss/ipssmainscr.do", Main, null, (data) =>
            {
                buffer.Clear();
                buffer.Append("http://ipss.interpark.com/delivery/ProDeliveryCheckList.do?");
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                {
                    {"_method","list"},
                    {"_style","grid"},
                    {"sc.strDt", DateTime.Now.AddDays(-14).ToString("yyyyMMdd")},
                    {"sc.endDt", DateTime.Now.ToString("yyyyMMdd") },
                    {"_search", "false"},
                    //{"nd","" },
                    //{"rows","30" },
                    {"page","1" },
                    //{"sidx","" },
                    //{"sord","asc" },
                    {"sc.dateTp","2" },
                    {"sc.strHour","00" },
                    {"sc.endHour","23" },
                    {"sc.page", "1" }
                }));
                base.Navigate(buffer.ToString());
            });
            FlowList.Add("delivery/ProDeliveryCheckList.do", ProDeliveryCheckList, null, (data) =>
            {
                buffer.Clear();
                buffer.Append("https://ipss.interpark.com/delivery/ProDeliveryStatusList.do?");
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                {
                    {"_method","list"},
                    {"_style","grid"},
                    {"sc.strDt", DateTime.Now.AddDays(-14).ToString("yyyyMMdd")},
                    {"sc.endDt", DateTime.Now.ToString("yyyyMMdd") },
                    {"_search", "false"},
                    //{"nd","" },
                    //{"rows","30" },
                    {"page","1" },
                    //{"sidx","" },
                    //{"sord","asc" },
                    {"sc.dateTp","2" },
                    {"sc.strHour","00" },
                    {"sc.endHour","23" },
                    {"sc.page", "1" }
                }));
                base.Navigate(buffer.ToString());
            });
            FlowList.Add("delivery/ProDeliveryStatusList.do", ProDeliveryStatusList, null, (data) =>
            {
                buffer.Clear();
                buffer.Append("http://ipss.interpark.com/order/ProBuyConfirmList.do?");
                this.buffer.Append(CreateGetParameter(new Dictionary<String, String>()
                {
                    {"_method","list"},
                    {"_style","grid"},
                    {"sc.strDt", DateTime.Now.AddDays(-14).ToString("yyyyMMdd")},
                    {"sc.endDt", DateTime.Now.ToString("yyyyMMdd") },
                    {"_search", "false"},
                    //{"nd","" },
                    //{"rows","30" },
                    {"page","1" },
                    //{"sidx","" },
                    //{"sord","asc" },
                    {"sc.dateTp","2" },
                    {"sc.strHour","00" },
                    {"sc.endHour","23" },
                    {"sc.page", "1" }
                }));
                base.Navigate(buffer.ToString());
            });
            FlowList.Add("order/ProBuyConfirmList.do", ProBuyConfirmList, null, (data) =>
            {
                Response.SetResultCode(ResultCode.RC1000);
                data.IsNextScrap = false;
            });
        }
        protected override void ScrapType4(FlowModelData flowModelData)
        {
            FlowList.Add("stdservice/ipssCheckLogin.do", Login, null);
            FlowList.Add("member/login.do", Login2, null);
            FlowList.Add("ipss/ipssmainscr.do", Main, "http://ipss.interpark.com/delivery/reportEntr.do?_method=excel&_style=ipssPro&sc.yyyymm="+Parameter.Sdate.Substring(0,6));
            FlowList.Add("delivery/reportEntr.do", ReportEntrExcel, null);
            //"http://ipss.interpark.com/delivery/reportEntr.do?_method=excel&_style=ipssPro&sc.yyyymm=" + Parameter.Sdate.Substring(0, 6)
            //DownloadMap.Add("delivery/reportEntr.do",null);
        }
        protected override void ScrapType5(FlowModelData flowModelData)
        {

        }
        protected override void Finally(FlowModelData flowModelData)
        {

        }
        public override string StartPage(FlowModelData flowModelData)
        {
            return "http://ipss.interpark.com/stdservice/ipssCheckLogin.do?_method=initial";
        }
    }
}
