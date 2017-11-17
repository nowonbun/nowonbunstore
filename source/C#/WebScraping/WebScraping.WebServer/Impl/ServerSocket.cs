using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebScraping.Library.Log;

namespace WebScraping.WebServer.Impl
{
    class ServerSocket : Socket, IServerSocket, IDisposable
    {
        private int port;
        private Dictionary<String, Scraper> scraperlist = new Dictionary<string, Scraper>();
        private Thread _thread;
        private bool live = true;
        private String path;
        private Logger logger;

        public ServerSocket(int port, String path)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            this.logger = logger = LoggerBuilder.Init().Set(this.GetType()); ;
            this.port = port;
            this.path = path;
            base.Bind(new IPEndPoint(IPAddress.Any, port));
            base.Listen(100);
            this._thread = new Thread(() =>
            {
                while (live)
                {
                    try
                    {
                        ClientSocket client = new ClientSocket(this, Accept(), path);
                        client.Run();
                    }
                    catch (Exception e)
                    {
                        logger.Error(e.ToString());
                    }
                }
            });
        }

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
        public void Run()
        {
            this.logger.Info("Server start...");
            this._thread.Start();
        }
        public new void Dispose()
        {
            this.logger.Info("Server dispose...");
            live = false;
            SendPingPong();
            this._thread.Abort();
            base.Dispose();
        }
        private void SendPingPong()
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                s.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            }
        }
    }
}
