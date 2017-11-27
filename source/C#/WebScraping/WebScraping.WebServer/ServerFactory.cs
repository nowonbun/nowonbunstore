using System;
using WebScraping.WebServer.Impl;

namespace WebScraping.WebServer
{
    public class ServerFactory
    {
        private static ServerFactory singleton = null;
        public static IServer NewInstance(int port)
        {
            if (singleton != null)
            {
                throw new Exception("already allocation!!");
            }
            singleton = new ServerFactory(port);
            (singleton.server as Server).ServerStart();
            return singleton.server;
        }
        public static IServer GetInstance()
        {
            if (singleton == null)
            {
                throw new Exception("not allocation!!");
            }
            return singleton.server;
        }
        private IServer server;
        private ServerFactory(int port)
        {
            server = new Server(port);
        }
    }
}
