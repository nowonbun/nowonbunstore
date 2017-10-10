using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace WebScraping.WebServer.Impl
{
    class ServerSocket : Socket, IServerSocket, IDisposable
    {
        private int port;
        private event Action<IClientSocket> acceptEvent;
        private Dictionary<String, Scraper> scraperlist = new Dictionary<string, Scraper>();
        private Thread _thread;
        private bool live = true;
        private String path;

        public String StartScraper(String param)
        {
            Scraper scraper = new Scraper(param, path);
            String key = scraper.Run();
            AddScraper(key, scraper);
            return key;
        }
        public Scraper AddScraper(String key, Scraper scraper)
        {
            scraperlist.Add(key, scraper);
            return scraper;
        }
        public Scraper ExistScraper(String key)
        {
            if (scraperlist.ContainsKey(key))
            {
                return scraperlist[key];
            }
            return null;
        }
        public Scraper RemoveScraper(String key)
        {
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

        public ServerSocket(int port, String path)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            this.port = port;
            this.path = path;
            base.Bind(new IPEndPoint(IPAddress.Any, port));
            base.Listen(100);
            acceptEvent += (c) => { };
            this._thread = new Thread(() =>
            {
                while (live)
                {
                    try
                    {
                        ClientSocket client = new ClientSocket(this, Accept(), path);
                        client.Run();
                        acceptEvent(client);
                    }
                    catch (Exception e)
                    {
                        //TODO: Log
                    }
                }
            });
        }
        public void SetAcceptEvent(Action<IClientSocket> e)
        {
            acceptEvent += e;
        }
        public void Run()
        {
            this._thread.Start();
        }
        public new void Dispose()
        {
            live = false;
            SendPingPong();
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
