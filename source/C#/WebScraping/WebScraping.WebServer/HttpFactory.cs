using System;
using WebScraping.Library.Log;
using WebScraping.WebServer.Impl;

namespace WebScraping.WebServer
{
    public class HttpFactory
    {
        private static HttpFactory singleton = null;
        private IWebHttpServer webserver = null;
        private IWebSocketServer websocket = null;
        private Logger logger = null;
        private HttpFactory(int port, int wsport)
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
            webserver = new WebHttpServer(port);
            websocket = new WebSocketServer(wsport);
        }
        public static HttpFactory NewInstance(int port, int wsport)
        {
            if (singleton == null)
            {
                singleton = new HttpFactory(port, wsport);
            }
            return singleton;
        }
        public IWebHttpServer WebServer
        {
            get { return this.webserver; }
        }
        public IWebSocketServer WebSocketServer
        {
            get { return this.websocket; }
        }

    }
}
