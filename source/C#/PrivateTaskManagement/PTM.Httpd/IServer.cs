using System;
using PTM.Httpd.Util;
using System.Collections.Generic;

namespace PTM.Httpd
{
    public interface IServer
    {
        void Set(String2 key, Action<Request, Response> method);
        void SetRootPath(String path);
        void SetZip(String path);
        void SetDefaultFile(String path);
        void SetWebSocket(Func<String2, WebSocketNode> method);
        Dictionary<String, Object> GetSession(String key);
        String2 GetZipFile(String key);
    }
}
