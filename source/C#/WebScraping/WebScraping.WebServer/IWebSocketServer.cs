using System;

namespace WebScraping.WebServer
{
    public interface IWebSocketServer : IDisposable
    {
        void Get(Func<byte[], WebSocketNode> method);
        void Send(WebSocketNode node);
    }
}
