using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
