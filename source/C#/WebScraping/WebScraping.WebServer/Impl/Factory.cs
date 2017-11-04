using System;

namespace WebScraping.WebServer.Impl
{
    public static class Factory
    {
        public static IServerSocket GetServerSocket(int port, String path)
        {
            return new ServerSocket(port, path);
        }
    }
}
