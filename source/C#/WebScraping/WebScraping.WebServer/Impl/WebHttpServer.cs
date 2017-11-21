using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.IO;
using WebScraping.Library.Log;

namespace WebScraping.WebServer.Impl
{
    class WebHttpServer : IWebHttpServer, IDisposable
    {
        private HttpListener _listener = new HttpListener();
        private Logger _logger = null;
        private Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, HttpListenerContext, object>> _method
                            = new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, HttpListenerContext, object>>();

        public WebHttpServer(int port)
        {
            _logger = LoggerBuilder.Init().Set(this.GetType());
            _listener.Prefixes.Add(String.Format("http://*:{0}/", port));
            _listener.Start();
            ThreadPool.QueueUserWorkItem(Listening);
        }
        private void Listening(object c)
        {
            while (_listener.IsListening)
            {
                ThreadPool.QueueUserWorkItem(Request, _listener.GetContext());
            }
        }
        private async void WebSocket(HttpListenerContext context)
        {
            var wsc = await context.AcceptWebSocketAsync(null);
            var ws = wsc.WebSocket;
        }
        private void Request(object obj)
        {
            HttpListenerContext context = obj as HttpListenerContext;
            _logger.Info(context.Request.RawUrl);
            using (var stream = context.Response.OutputStream)
            {
                if (!_method.ContainsKey(context.Request.RawUrl))
                {
                    _logger.Error("Not method exists!");
                    context.Response.StatusCode = 404;
                    return;
                }
                var temp = _method[context.Request.RawUrl](context.Request, context.Response, context);
                byte[] buf;
                if (temp is String)
                {
                    buf = ReadFile(temp as String);
                }
                else
                {
                    buf = temp as byte[];
                }

                context.Response.ContentLength64 = buf.Length;
                stream.Write(buf, 0, buf.Length);
            }
        }
        public void Get(string rawUrl, Func<byte[]> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context();
            });
        }
        public void Get(string rawUrl, Func<HttpListenerResponse, byte[]> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(res);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerRequest, byte[]> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(req);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerContext, byte[]> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(con);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, byte[]> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(req, res);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, HttpListenerContext, byte[]> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, context);
        }
        public void Get(string rawUrl, Func<String> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context();
            });
        }
        public void Get(string rawUrl, Func<HttpListenerResponse, String> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(res);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerRequest, String> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(req);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerContext, String> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(con);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, String> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, (req, res, con) =>
            {
                return context(req, res);
            });
        }
        public void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, HttpListenerContext, String> context)
        {
            if (_method.ContainsKey(rawUrl))
            {
                _logger.Error("method exists!");
                _method.Remove(rawUrl);
            }
            _method.Add(rawUrl, context);
        }
        private byte[] ReadFile(String filepath)
        {
            FileInfo info = new FileInfo(filepath);
            if (!info.Exists)
            {
                _logger.Error("not file");
                throw new Exception("not file");
            }
            using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[info.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public void Dispose()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
