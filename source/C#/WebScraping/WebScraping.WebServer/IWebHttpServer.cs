using System;
using System.Net;

namespace WebScraping.WebServer
{
    public interface IWebHttpServer : IDisposable
    {
        void Get(string rawUrl, Func<byte[]> context);
        void Get(string rawUrl, Func<HttpListenerResponse, byte[]> context);
        void Get(string rawUrl, Func<HttpListenerRequest, byte[]> context);
        void Get(string rawUrl, Func<HttpListenerContext, byte[]> context);
        void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, byte[]> context);
        void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, HttpListenerContext, byte[]> context);
        void Get(string rawUrl, Func<String> context);
        void Get(string rawUrl, Func<HttpListenerResponse, String> context);
        void Get(string rawUrl, Func<HttpListenerRequest, String> context);
        void Get(string rawUrl, Func<HttpListenerContext, String> context);
        void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, String> context);
        void Get(string rawUrl, Func<HttpListenerRequest, HttpListenerResponse, HttpListenerContext, String> context);
    }
}
