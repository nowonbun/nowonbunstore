using System;
using System.Collections.Generic;
using System.IO;

namespace WebScraping.WebServer
{
    public class Response
    {
        private Dictionary<String2, String2> _header = new Dictionary<String2, String2>();
        private String2 _body;
        public Dictionary<String2, String2> Headers { get { return _header; } }
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

        public Response()
        {
            Version = "HTTP/1.1";
            State = "403";
            StateComment = "Not Found";
            ContextType = "text/html; charset=UTF-8";
            _header.Add("Server", "MyServer");
            _header.Add("Cache-Control", "no-cache");
            Body = "";
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
                throw new Exception("not file");
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
        public void SetHeader(String2 key, String2 value)
        {
            if (this.Headers.ContainsKey(key))
            {
                this.Headers.Remove(key);
            }
            this.Headers.Add(key, value);
        }
    }
}
