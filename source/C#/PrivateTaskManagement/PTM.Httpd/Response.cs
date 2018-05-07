using System;
using System.Collections.Generic;
using System.IO;
using PTM.Httpd.Util;

namespace PTM.Httpd
{
    public class Response
    {
        private Dictionary<String2, String2> _header = new Dictionary<String2, String2>();
        private List<String2> _cookie = new List<String2>();
        private String2 _body;
        private Request _request;
        private IServer _server;
        public Dictionary<String2, String2> Header { get { return _header; } }
        public String2 Version { get; private set; }
        public String2 State { get; private set; }
        public String2 StateComment { get; private set; }
        public String2 Body
        {
            get
            {
                return _body;
            }
            set
            {
                StateOK();
                _body = value;
            }
        }

        public Response(IServer server, Request request)
        {
            this._server = server;
            this._request = request;
            Body = new String2(0);
            Version = "HTTP/1.1";
            State = "403";
            StateComment = "Not Found";
            ContextType = "text/html; charset=UTF-8";
            _header.Add("Server", "MyServer");
            _header.Add("Cache-Control", "no-cache");
            _header.Add("Connection", "Keep-Alive");
            _header.Add("Keep-Alive", "timeout = 5, max = 99s");
        }

        public void StateOK()
        {
            State = "200";
            StateComment = "OK";
        }

        public void SetResponseCode(int code, String2 comment)
        {
            State = String.Format("{0}", code);
            StateComment = comment;
        }

        public String2 ContextType
        {
            get
            {
                if (_header.ContainsKey("Content-Type"))
                {
                    return _header["Content-Type"];
                }
                return null;
            }
            set
            {
                if (_header.ContainsKey("Content-Type"))
                {
                    _header.Remove("Content-Type");
                }
                _header.Add("Content-Type", value);
            }
        }

        public String2 ReadFile(String filepath)
        {
            FileInfo info = new FileInfo(filepath);
            if (!info.Exists)
            {
                State = "403";
                StateComment = "Not Found";
                return _body;
            }
            if (".JS".Equals(info.Extension.ToUpper()))
            {
                ContextType = "text/javascript; charset=UTF-8";
            }
            if (".CSS".Equals(info.Extension.ToUpper()))
            {
                ContextType = "text/css; charset=UTF-8";
            }
            using (FileStream stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
            {
                _body = new String2((int)info.Length);
                stream.Read(_body.ToBytes(), 0, (int)info.Length);
            }
            StateOK();
            return _body;
        }

        public void SetCookie(String2 key, String2 value)
        {
            _cookie.Add(key + "=" + value + ";");
        }

        public void SetCookie(String2 key, String2 value, DateTime expire)
        {
            _cookie.Add(key + "=" + value + "; Expires=" + expire.ToString("r"));
        }

        public void SetCookie(String2 key, String2 value, String2 domain, String2 path)
        {
            _cookie.Add(key + "=" + value + "; Domain=" + domain + "; Path=" + path);
        }

        public void SetCookie(String2 key, String2 value, String2 domain, String2 path, DateTime expire)
        {
            _cookie.Add(key + "=" + value + "; Domain=" + domain + "; Path=" + path + "; Expires=" + expire.ToString("r"));
        }

        public void ClearCookie(String key)
        {
            _cookie.Add(key + "=; Expires=" + new DateTime(0).ToString("r") + ";");
        }

        public String2 View()
        {
            String2 buffer = new String2(0);
            buffer += Version + " " + State + " " + StateComment + String2.CRLF;
            foreach (var h in _header)
            {
                buffer += h.Key + ": " + h.Value + String2.CRLF;
            }
            foreach (var c in _cookie)
            {
                buffer += "Set-Cookie:" + c + String2.CRLF;
            }
            buffer += "Content-Length: " + Body.Length + String2.CRLF;
            buffer += "Date: " + DateTime.Now.ToString("r") + String2.CRLF;
            buffer += String2.CRLF;
            buffer += Body;
            buffer += String2.CRLF + String2.CRLF;
            return buffer;
        }

        public void SetSession(String key, Object value)
        {
            String sessionkey;
            if (_request.Cookie.ContainsKey("session-id"))
            {
                sessionkey = _request.Cookie["session-id"].ToString();
            }
            else
            {
                sessionkey = System.Guid.NewGuid().ToString();
                SetCookie("session-id", sessionkey);
            }
            var session = _server.GetSession(sessionkey);
            if (session.ContainsKey(key))
            {
                session.Remove(key);
            }
            session.Add(key, value);
        }
    }
}
