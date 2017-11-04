using System;

namespace WebScraping.WebServer
{
    public interface IServerSocket : IDisposable
    {
        void Run();
    }
}
