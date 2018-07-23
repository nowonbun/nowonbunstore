#region package declare
using System;
using System.Collections.Generic;
using Gecko;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Other;
using WebScraping.Library.Log;
using WebScraping.Library.Config;
using System.IO;
using System.Net;
using WebScraping.Scraper.Common;
#endregion

namespace WebScraping.Scraper.Impl
{
    class ScrapBrowser : GeckoWebBrowser
    {
        private IScrapFlow flow = null;
        private Parameter parameter = null;
        private FlowModelData flowModelData = new FlowModelData();

        private Logger logger = LoggerBuilder.Init().Set(typeof(ScrapBrowser));
        private IDictionary<int, Type> FlowMap = new Dictionary<int, Type>();
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //Hmall is not web system. that's window application.We hava to find in method.
        //Dnshop is not exact url.
        //Nsmall shopping is not web system, too
        public ScrapBrowser()
        {
            //TODO: I think that to manage is also very difficult.
            //TODO: It is not solution to C#. that solution is RPA? I need research about it.
            FlowMap.Add(1, typeof(Flow.Gmarket.GMarketFlow));
            FlowMap.Add(2, typeof(Flow.Auction.AuctionFlow));
            FlowMap.Add(3, typeof(Flow._11thStreet.StreetFlow));
            FlowMap.Add(5, typeof(Flow.Interpark.InterparkFlow));
            FlowMap.Add(6, typeof(Flow.Interpark.InterparkFlow));
            FlowMap.Add(7, typeof(Flow.KCP.KCPFlow));
            FlowMap.Add(8, typeof(Flow.StoreFarm.StoreFarmFlow));
            FlowMap.Add(9, typeof(Flow.Halfclub.HalfclubFlow));
            FlowMap.Add(10, typeof(Flow.Cjmall.CjmallFlow));
            FlowMap.Add(11, typeof(Flow.Ssgadm.SsgadmFlow));
            FlowMap.Add(13, typeof(Flow.Lotteimall.LotteimallFlow));
            FlowMap.Add(17, typeof(Flow._10x10.TentenFlow));
            FlowMap.Add(18, typeof(Flow.Gsshop.GsshopFlow));
            FlowMap.Add(19, typeof(Flow.Uplus.UplusFlow));
            FlowMap.Add(22, typeof(Flow.Istyle24.Istyle24Flow));
            FlowMap.Add(23, typeof(Flow.Ksnet.KsnetFlow));
            FlowMap.Add(24, typeof(Flow.Allthegate.AllthegateFlow));
            FlowMap.Add(26, typeof(Flow.Lotte.LotteFlow));
            FlowMap.Add(27, typeof(Flow.Teledit.TeleditFlow));
            FlowMap.Add(29, typeof(Flow.Wizwid.WizwidFlow));
            FlowMap.Add(30, typeof(Flow.Coupang.CoupangFlow));
            FlowMap.Add(31, typeof(Flow.Ticketmonster.TicketmonsterFlow));
            FlowMap.Add(32, typeof(Flow.Wemakeprice.WemakepriceFlow));
            FlowMap.Add(33, typeof(Flow.Yes24.Yes24Flow));
            FlowMap.Add(34, typeof(Flow.LotteShop.LotteShopFlow));
            FlowMap.Add(36, typeof(Flow.Cardsales.CardsalesFlow));
            FlowMap.Add(37, typeof(Flow.Akmall.AkmallFlow));
            FlowMap.Add(38, typeof(Flow.Homeplus.HomeplusFlow));
            FlowMap.Add(39, typeof(Flow.Fashionplus.FashionplusFlow));
            FlowMap.Add(40, typeof(Flow.LottePartnerPlus.LottePartnerPlusFlow));
            FlowMap.Add(41, typeof(Flow.Ehyundai.EhyundaiFlow));
            FlowMap.Add(42, typeof(Flow.Kisvan.KisvanFlow));
            FlowMap.Add(43, typeof(Flow.Smilepay.SmilepayFlow));
            FlowMap.Add(45, typeof(Flow.Kscm.KscmFlow));
            FlowMap.Add(46, typeof(Flow.NaverPay.NaverPayFlow));
            FlowMap.Add(49, typeof(Flow.Starfield.StarfieldFlow));

            FlowMap.Add(999, typeof(Flow.Test.TestFlow));
        }

        protected override void OnNavigated(GeckoNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }

        public void Set(Parameter param)
        {
            if (!FlowMap.ContainsKey(param.MallCD))
            {
                throw new ScraperException("The code is not defined. Please check code of parameter. Code : " + param.MallCD);
            }
            Type type = FlowMap[param.MallCD];
            this.flow = Activator.CreateInstance(type, this, param) as IScrapFlow;
            this.flow.Initialize(flowModelData);
            this.parameter = param;

            Gecko.LauncherDialog.Download += (sender, e) =>
            {
                String tempPath = ConfigSystem.ReadConfig("Config", "Temp", "Path");
                String file = Path.Combine(tempPath, DateTime.Now.ToString("yyyyMMddHHmmss") + e.Filename);
                nsILocalFile objTarget = (nsILocalFile)Xpcom.NewNativeLocalFile(file);
                e.HelperAppLauncher.SaveToDisk(objTarget, false);
                Procedure(new Uri(e.Url), null, file);
            };
            this.Navigate(flow.StartPage(flowModelData));
        }

        protected override void OnDocumentCompleted(Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            Procedure(e.Uri, e.Window, null);
        }

        private void Procedure(Uri uri, GeckoWindow window, String file)
        {
            try
            {
                Action<FlowModelData> action;
                String next;
                Action<FlowModelData> callback;
                flow.Procedure(uri, out action, out next, out callback);
                if (window != null)
                {
                    GeckoElement script = this.Document.CreateElement("script");
                    script.SetAttribute("type", "text/javascript");
                    script.TextContent = "window.alert = function(){};";
                    window.Document.OwnerDocument.Head.AppendChild(script);
                    using (AutoJSContext context = new AutoJSContext(this.Window))
                    {
                        string result;
                        context.EvaluateScript("window.alert = function(){};", (nsISupports)window.DomWindow, out result);
                    }
                    flowModelData.Document = window.Document.OwnerDocument;
                }
                flowModelData.Uri = uri;
                flowModelData.NextUrl = next;
                flowModelData.CallBack = callback;
                flowModelData.IsNextScrap = true;
                flowModelData.IsSkipAction = false;
                if (file != null)
                {
                    flowModelData.File = new FileInfo(file);
                }
                action(flowModelData);
                if (flowModelData.IsSkipAction)
                {
                    return;
                }
                if (flowModelData.CallBack != null)
                {
                    flowModelData.CallBack(flowModelData);
                }
                if (!String.IsNullOrEmpty(flowModelData.NextUrl))
                {
                    this.Navigate(flowModelData.NextUrl);
                }
                if (!flowModelData.IsNextScrap)
                {
                    //resultParameter.Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    flow.End(flowModelData);
                    Scraper.Exit();
                }
            }
            catch (Exception ex)
            {
                //resultParameter.ResultCD = "9999";
                //resultParameter.ResultMSG = ex.ToString();

                flow.Error(ex);
                logger.Error(ex.ToString());
                Scraper.Exit();
            }
        }

        protected override void OnCreateWindow(GeckoCreateWindowEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
