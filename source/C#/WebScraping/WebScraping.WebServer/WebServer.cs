using System;
using WebScraping.WebServer.Impl;
using WebScraping.Library.Log;

namespace WebScraping.WebServer
{
    public class WebServer
    {
        private static WebServer singleton = null;
        private IServerSocket server;
        private Logger logger;

        private WebServer()
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
        }

        public static void Start(int port, String path)
        {
            if (singleton != null)
            {
                throw new Exception("Webserver is already");
            }
            singleton = new WebServer();
            singleton.server = Factory.GetServerSocket(port, path);
            singleton.server.Run();
        }

        public static void End()
        {
            if (singleton == null)
            {
                throw new Exception("Webserver is not ready");
            }
            singleton.server.Dispose();
        }
    }
}
