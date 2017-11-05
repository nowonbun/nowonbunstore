using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ScrappingHttpCore
{
    /// <summary>
    /// 웹 서버 클래스
    /// </summary>
    public class HttpServer : Socket
    {
        public HttpServer(int port = 10000)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            this.Bind(ipep);
            this.Listen(20);
        }
        /// <summary>
        /// 서버 개시
        /// </summary>
        /// <param name="listener"></param>
        public void Start(Func<IHeader, String> listener)
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                HttpAccept(listener);
            });
        }
        /// <summary>
        /// 접속시 발생되는 함수
        /// </summary>
        /// <param name="listener"></param>
        private void HttpAccept(Func<IHeader, String> listener)
        {
            while (true)
            {
                ThreadPool.QueueUserWorkItem((c) =>
                {
                    using (Socket client = (Socket)c)
                    {
                        using (var stream = new NetworkStream(client))
                        {
                            RequestHeader request = new RequestHeader(stream);
                            ResponseHeader response = new ResponseHeader(listener.Invoke(request));
                            response.WriteSend(stream);
                        }
                    }
                }, this.Accept());
            }
        }
    }
}
