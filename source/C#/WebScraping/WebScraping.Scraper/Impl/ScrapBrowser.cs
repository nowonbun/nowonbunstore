#region package declare
using System;
using System.Collections.Generic;
using Gecko;
using WebScraping.Scraper.Node;
using WebScraping.Scraper.Interface;
using WebScraping.Scraper.Flow.Gmarket;
using WebScraping.Scraper.Flow.Auction;
using WebScraping.Scraper.Flow._11thStreet;
using WebScraping.Scraper.Flow.Test;
using WebScraping.Scraper.Flow.Interpark;
using WebScraping.Scraper.Other;
using WebScraping.Library.Log;
using WebScraping.Library.Config;
using System.IO;
using System.Net;
#endregion

namespace WebScraping.Scraper.Impl
{
    class ScrapBrowser : GeckoWebBrowser
    {
        class FlowType
        {
            public Type Flow { get; set; }
            public Boolean LoginMode { get; set; }
        }
        private IScrapFlow flow = null;
        private ScrapParameter parameter = null;
        private Logger logger = LoggerBuilder.Init().Set(typeof(ScrapBrowser));
        private IDictionary<String, FlowType> FlowMap = new Dictionary<String, FlowType>();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        public ScrapBrowser()
        {
            FlowMap.Add("001", new FlowType() { Flow = typeof(GMarketFlow), LoginMode = false });
            FlowMap.Add("002", new FlowType() { Flow = typeof(AuctionFlow), LoginMode = false });
            FlowMap.Add("003", new FlowType() { Flow = typeof(StreetFlow), LoginMode = false });
            FlowMap.Add("004", new FlowType() { Flow = typeof(InterparkFlow), LoginMode = false});

            FlowMap.Add("501", new FlowType() { Flow = typeof(GMarketFlow), LoginMode = true });
            FlowMap.Add("502", new FlowType() { Flow = typeof(AuctionFlow), LoginMode = true });
            FlowMap.Add("503", new FlowType() { Flow = typeof(StreetFlow), LoginMode = true });
            FlowMap.Add("504", new FlowType() { Flow = typeof(InterparkFlow), LoginMode = true });

            FlowMap.Add("999", new FlowType() { Flow = typeof(TestFlow), LoginMode = false });
        }

        protected override void OnNavigated(GeckoNavigatedEventArgs e)
        {
            base.OnNavigated(e);
        }

        public void Set(ScrapParameter param)
        {
            if (!FlowMap.ContainsKey(param.Code))
            {
                throw new ScraperException("Code is not defined. Please check code of parameter. Code : " + param.Code);
            }
            FlowType type = FlowMap[param.Code];
            this.flow = Activator.CreateInstance(type.Flow, this, param, type.LoginMode) as IScrapFlow;
            this.parameter = param;
            Gecko.LauncherDialog.Download += (sender, e) =>
            {
                String tempPath = ConfigSystem.ReadConfig("Config", "Temp", "Path");
                String file = Path.Combine(tempPath, DateTime.Now.ToString("yyyyMMddHHmmss") + e.Filename);
                nsILocalFile objTarget = (nsILocalFile)Xpcom.NewNativeLocalFile(file);
                e.HelperAppLauncher.SaveToDisk(objTarget, false);
                Action<String, String> action = flow.DownloadProcedure(e.Url);
                action(e.Url, file);
            };
            timer.Interval = 10 * 1000;
            timer.Tick += (s, e) =>
            {
                Ping(parameter.Keycode);
            };
            timer.Start();
            this.Navigate(flow.StartPage());
        }

        protected override void OnDocumentCompleted(Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            try
            {
                Func<GeckoDocument, Uri, Boolean> action = flow.Procedure(e.Uri);
                GeckoElement script = this.Document.CreateElement("script");
                script.SetAttribute("type", "text/javascript");
                script.TextContent = "window.alert = function(){};";
                this.Document.Head.AppendChild(script);

                if (!action(this.Document, e.Uri))
                {
                    flow.End();
                    Scraper.Exit();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                Scraper.Exit();
            }
        }
        protected override void OnCreateWindow(GeckoCreateWindowEventArgs e)
        {
            e.Cancel = true;
        }
        private void Ping(String code)
        {
            this.logger.Debug("PING : http://localhost:" + ServerInfo.GetPort() + "/Ping?Code=" + parameter.Keycode);
            if (Debug.IsDebug())
            {
                return;
            }
            HttpWebRequest req = WebRequest.CreateHttp("http://localhost:" + ServerInfo.GetPort() + "/Ping?Code=" + parameter.Keycode);
            req.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    String res = reader.ReadToEnd();
                    logger.Info(res);
                }
            }
        }
        public void NotifyEnd(String code)
        {
            this.logger.Debug("EndScrap : http://localhost:" + ServerInfo.GetPort() + "/EndScrap?Code=" + parameter.Keycode);
            if (Debug.IsDebug())
            {
                return;
            }
            HttpWebRequest req = WebRequest.CreateHttp("http://localhost:" + ServerInfo.GetPort() + "/EndScrap?Code=" + parameter.Keycode);
            req.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    String res = reader.ReadToEnd();
                    logger.Info(res);
                }
            }
        }
    }
}
