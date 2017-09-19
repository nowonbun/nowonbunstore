using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace WebScraping.WebServer.Impl
{
    class ServerSocket : Socket, IServerSocket, IDisposable
    {
        private int port;
        private event Action<IClientSocket> acceptEvent;
        private Dictionary<String, Process> scraperlist = new Dictionary<string, Process>();
        private Thread _thread;
        private bool live = true;
        private String path;

        public Process AddScraper(String key, Process scraper)
        {
            scraperlist.Add(key, scraper);
            return scraper;
        }
        public Process ExistScraper(String key)
        {
            if (scraperlist.ContainsKey(key))
            {
                return scraperlist[key];
            }
            return null;
        }
        public Process RemoveScraper(String key)
        {
            if (scraperlist.ContainsKey(key))
            {
                Process ret = scraperlist[key];
                scraperlist.Remove(key);
                return ret;
            }
            return null;
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
