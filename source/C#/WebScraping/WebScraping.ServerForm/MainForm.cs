using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gecko;
using System.IO;
using WebScraping.Library.Log;
using WebScraping.Library.Config;
using System.Collections.Generic;
using System.Linq;

namespace WebScraping.ServerForm
{
    public partial class MainForm : Form
    {
        private Logger logger;
        private ScriptHook hook;
        private String path;

        public MainForm()
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));
            logger.Info("FireFox Dll Initialize");
            InitializeComponent();
            InitializeFlow();

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            hook = new ScriptHook();
            webBrowser1.Navigate("http://localhost:" + ServerInfo.GetPort() + "/ControllView");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            hook.Dispose();
            base.OnClosing(e);
        }
        /*
        public String StartScraper(String param)
        {
            this.logger.Info("Start scraper param : " + param);
            Scraper scraper = new Scraper(param, path);
            String key = scraper.Run();
            AddScraper(key, scraper);
            return key;
        }
        public Scraper AddScraper(String key, Scraper scraper)
        {
            this.logger.Info("Add scraper key : " + key);
            scraperlist.Add(key, scraper);
            return scraper;
        }
        public Scraper ExistScraper(String key)
        {
            if (scraperlist.ContainsKey(key))
            {
                this.logger.Info("Check_OK scraper key : " + key);
                return scraperlist[key];
            }
            this.logger.Info("Check_NG scraper key : " + key);
            return null;
        }
        public Scraper RemoveScraper(String key)
        {
            this.logger.Info("Remove scraper key : " + key);
            if (scraperlist.ContainsKey(key))
            {
                Scraper ret = scraperlist[key];
                scraperlist.Remove(key);
                return ret;
            }
            return null;
        }
        public IList<Scraper> GetScraperList()
        {
            return scraperlist.Select(node => { return node.Value; }).ToList();
        }
        public void PingScraper(String key)
        {
            this.logger.Info("Ping scraper key : " + key);
            if (scraperlist.ContainsKey(key))
            {
                Scraper ret = scraperlist[key];
                ret.Parameter.Pingtime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            }
        }
        public void Command(String msg, Stream stream)
        {
            byte[] header = null;
            logger.Debug(msg);
            if (msg == null)
            {
                header = CreateResponse(501, "Not Implemented");
                stream.Write(header, 0, header.Length);
            }
            String[] buffer = msg.Split('?');
            if (buffer.Length < 1 || buffer[0].Length < 2)
            {
                header = CreateResponse(501, "Not Implemented");
                stream.Write(header, 0, header.Length);
            }
            String cmd = buffer[0].ToUpper().Substring(1);
            String param = null;
            if (buffer.Length > 1)
            {
                param = buffer[1];
            }
            if ("CONTROLLVIEW".Equals(cmd))
            {
                var app_dir = Path.GetDirectoryName(this.path);
                byte[] data = GetHtmlFile(app_dir + "\\Web\\index.html");
                if (data != null)
                {
                    header = CreateResponse(200, "OK", 1);
                    stream.Write(header, 0, header.Length);
                    stream.Write(data, 0, data.Length);
                    return;
                }
            }
            else if ("JQUERY".Equals(cmd))
            {
                var app_dir = Path.GetDirectoryName(this.path);
                byte[] data = GetHtmlFile(app_dir + "\\Web\\jquery-3.2.1.min.js");
                if (data != null)
                {
                    header = CreateResponse(200, "OK", 2);
                    stream.Write(header, 0, header.Length);
                    stream.Write(data, 0, data.Length);
                    return;
                }
            }
            else if ("SCRAP".Equals(cmd))
            {
                this.logger.Info("Call scraping...");
                this.server.StartScraper(param);
                header = CreateResponse(200, "OK", 1);
                stream.Write(header, 0, header.Length);
                return;
            }
            else if ("LIST".Equals(cmd))
            {
                this.logger.Info("Call data list...");
                IList<Scraper> scraperlist = this.server.GetScraperList();
                IList<Parameter> jsonlist = scraperlist.Select(node => { return node.Parameter; }).ToList();
                String json = JsonConvert.SerializeObject(jsonlist);
                this.logger.Debug(json);
                byte[] data = Encoding.UTF8.GetBytes(json);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                stream.Write(data, 0, data.Length);
                return;
            }
            else if ("PING".Equals(cmd))
            {
                this.logger.Info("PING");
                var temp = CreateParam(param);
                String code = temp["CODE"];
                this.logger.Debug("Ping Code = " + code);
                this.server.PingScraper(code);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                return;
            }
            else if ("ENDSCRAP".Equals(cmd))
            {
                this.logger.Info("EndScrap");
                var temp = CreateParam(param);
                String code = temp["CODE"];
                this.logger.Debug("Exit Code = " + code);
                this.server.RemoveScraper(code);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                return;
            }
            else if ("Restart".Equals(cmd))
            {

            }
            else if ("LOG".Equals(cmd))
            {
                logger.Info("Javascript logger : " + param);
                header = CreateResponse(200, "OK", 2);
                stream.Write(header, 0, header.Length);
                return;
            }
            header = CreateResponse(501, "Not Implemented");
            stream.Write(header, 0, header.Length);
        }
        */
    }
}
