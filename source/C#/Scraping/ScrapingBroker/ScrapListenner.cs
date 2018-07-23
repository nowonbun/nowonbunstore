using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using WebScraping.Library.Config;
using WebScraping.Library.Log;

namespace WebScraping.Broker
{
    class ScrapListenner : Socket
    {
        private Logger logger = null;
        private static ScrapListenner singleton = null;
        private List<ScrapExecutor> queue = new List<ScrapExecutor>();
        public static ScrapListenner Instance()
        {
            if (singleton == null)
            {
                singleton = new ScrapListenner();
            }
            return singleton;
        }

        private ScrapListenner() : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            logger = LoggerBuilder.Init().Set(GetType()).Info("Listen Start!");
            Bind(new IPEndPoint(IPAddress.Any, BrokerInfo.GetPort()));
            Listen(20);
            ThreadPool.QueueUserWorkItem((c) =>
            {
                while (true)
                {
                    Socket client = Accept();
                    ThreadPool.QueueUserWorkItem((b) =>
                    {
                        Socket a = (Socket)b;
                        byte[] buffer = new byte[4];
                        a.Receive(buffer, 4, SocketFlags.None);
                        int size = BitConverter.ToInt32(buffer, 0);
                        buffer = new byte[size];
                        a.Receive(buffer, size, SocketFlags.None);
                        a.Send(new byte[] { 9 });
                        a.Close();

                        String msg = Encoding.UTF8.GetString(buffer);
                        logger.Info(" [WEB LOG] Send messate - " + msg);

                        Result result = Result.Build(msg);
                        //TODO: this function is that send just to get the message from the scraper to the server.
                        //the structure is key, ResultType, ScrapType, Index, Separation, Data.
                        ServerConnector.Instance().Send(buffer);
                    }, client);
                }
            });
        }

        public void SetExecuter(ScrapExecutor process)
        {
            queue.Add(process);
            process.Exited += (s, e) =>
            {
                //TODO : not call
                ScrapExecutor b = (ScrapExecutor)s;
                //TODO : ??
                //ServerConnector.Instance().Send(b.ResponseCode.ToString());
                ControlFactory.GetForm<Main>().RemoveGrid(b.Parameter);
                queue.Remove(b);
            };
        }
    }
}
