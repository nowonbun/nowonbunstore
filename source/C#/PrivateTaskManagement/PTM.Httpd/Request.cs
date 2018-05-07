using System;
using PTM.Httpd.Util;
using System.Collections.Generic;

namespace PTM.Httpd
{
    public class Request
    {
        private String2 _request;
        private Dictionary<String2, String2> _header = new Dictionary<String2, String2>();
        private Dictionary<String2, String2> _query_string = new Dictionary<String2, String2>();
        private Dictionary<String2, String2> _post_string = new Dictionary<String2, String2>();
        private Dictionary<String2, String2> _cookie = new Dictionary<String2, String2>();
        private IServer _server;

        public Dictionary<String2, String2> Header { get { return _header; } }
        public String2 Method { get; private set; }
        public String2 Uri { get; private set; }
        public String2 Version { get; private set; }
        public Dictionary<String2, String2> QueryString { get { return _query_string; } }
        public Dictionary<String2, String2> PostString { get { return _post_string; } }
        public Dictionary<String2, String2> Cookie { get { return _cookie; } }
        public String2 Host { get; private set; }

        public Request(IServer server, String2 request)
        {
            _server = server;
            request = request.Trim();
            _request = request;
            String2[] temp_header = request.Split(new String2("\r\n"));
            String2 state = temp_header[0];
            String2[] temp_state = state.Split(" ");
            Method = temp_state[0].Trim();
            Uri = temp_state[1].Trim();
            int pos = Uri.IndexOf("?");
            if (pos > -1)
            {
                String2 temp_uri = Uri.SubString(pos + 1, Uri.Length - (pos + 1)).Trim();
                Uri = Uri.SubString(0, pos).Trim();
                String2[] temp_query_string = temp_uri.Split("&");
                foreach (var t in temp_query_string)
                {
                    pos = t.IndexOf("=");
                    if (pos < 0)
                    {
                        continue;
                    }
                    String2 key = t.SubString(0, pos).Trim();
                    String2 value = t.SubString(pos + 1, t.Length - (pos + 1)).Trim();
                    if (_query_string.ContainsKey(key))
                    {
                        _query_string.Remove(key);
                    }
                    _query_string.Add(key, value);
                }
            }
            Version = temp_state[2].Trim();
            for (int i = 1; i < temp_header.Length; i++)
            {
                String2[] temp = temp_header[i].Separate(":");
                if (temp == null)
                {
                    var post_value = temp_header[i].Split("&");
                    foreach (var val in post_value)
                    {
                        temp = val.Separate("=");
                        if (temp == null)
                        {
                            continue;
                        }
                        if (_post_string.ContainsKey(temp[0]))
                        {
                            _post_string.Remove(temp[0]);
                        }
                        _post_string.Add(temp[0], temp[1]);
                    }
                    continue;
                }
                if (_header.ContainsKey(temp[0]))
                {
                    _header.Remove(temp[0]);
                }
                _header.Add(temp[0], temp[1]);
            }
            if (_header.ContainsKey("Host"))
            {
                this.Host = _header["Host"];
            }
            else
            {
                this.Host = new String2(0);
            }
            if (_header.ContainsKey("Cookie"))
            {
                String2 temp = _header["Cookie"];
                String2[] cks = temp.Split(";");
                foreach (var c in cks)
                {
                    var t = c.Separate("=");
                    if (_cookie.ContainsKey(t[0]))
                    {
                        _cookie.Remove(t[0]);
                    }
                    _cookie.Add(t[0], t[1]);
                }
            }
        }

        public bool IsWebSocket()
        {
            return _header.ContainsKey("Upgrade") && new String2("websocket").Equals(_header["Upgrade"]);
        }

        public String2 View()
        {
            return _request;
        }

        public bool IsPost()
        {
            return Method.Equals("POST");
        }

        public Object GetSession(String key)
        {
            String sessionkey;
            if (Cookie.ContainsKey("session-id"))
            {
                sessionkey = Cookie["session-id"].ToString();
            }
            else
            {
                return null;
            }
            var session = _server.GetSession(sessionkey);
            if (session.ContainsKey(key))
            {
                return session[key];
            }
            return null;
        }
    }
}
