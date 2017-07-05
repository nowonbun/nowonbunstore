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

        private WebServer() { }

        public static void Start(int port, Action<IClientSocket> e)
        {
            if (singleton != null)
            {
                throw new Exception("already");
            }
            singleton = new WebServer();
            singleton.server = Factory.GetServerSocket(port);
            singleton.server.SetAcceptEvent(e);
            singleton.server.Run();
        }

        public static void End()
        {
            if (singleton == null)
            {
                throw new Exception("not ready");
            }
            singleton.server.Dispose();
        }
    }
}
