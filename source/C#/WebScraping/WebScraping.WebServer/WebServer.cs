using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.WebServer.Impl;

namespace WebScraping.WebServer
{
    public class WebServer
    {
        private static WebServer singleton = null;
        private IServerSocket server;
        private int port = 80;

        private WebServer()
        {
            server = Factory.GetServerSocket(port);
        }
        private static WebServer GetInstance()
        {
            if (singleton == null)
            {
                singleton = new WebServer();
            }
            return singleton;
        }

        public static void Start()
        {
            WebServer.GetInstance().server.Run();
        }

        public static void End()
        {
            WebServer.GetInstance().server.Dispose();
        }
    }
}
