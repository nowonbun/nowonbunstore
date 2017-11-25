using System.Collections.Generic;

namespace WebScraping.WebServer
{
    public class Request
    {

        private Dictionary<String2, String2> _header = new Dictionary<String2, String2>();
        private Dictionary<String2, String2> _query_string = new Dictionary<String2, String2>();

        public Dictionary<String2, String2> Headers { get { return _header; } }
        public String2 Method { get; private set; }
        public String2 Uri { get; private set; }
        public String2 Version { get; private set; }
        public Dictionary<String2, String2> QueryString { get { return _query_string; } }

        public Request(String2 header)
        {
            header = header.Trim();
            String2[] temp_header = header.Split(new String2("\r\n"));
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
                pos = temp_header[i].IndexOf(":");
                if (pos < 0)
                {
                    continue;
                }
                String2 key = temp_header[i].SubString(0, pos).Trim();
                String2 value = temp_header[i].SubString(pos + 1, temp_header[i].Length - (pos + 1)).Trim();
                if (_header.ContainsKey(key))
                {
                    _header.Remove(key);
                }
                _header.Add(key, value);
            }
        }
        public bool IsWebSocket()
        {
            return _header.ContainsKey("Upgrade") && new String2("websocket").Equals(_header["Upgrade"]);
        }
    }
}
