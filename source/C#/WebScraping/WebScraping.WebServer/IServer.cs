using System;

namespace WebScraping.WebServer
{
    public interface IServer
    {
        void Set(String2 key, Action<Request, Response> method);
        void SetRootPath(String path);
        void SetWebSocket(Func<String2, Opcode, WebSocketNode> method);
        void SendWebSocket(WebSocketNode node);
    }
}
