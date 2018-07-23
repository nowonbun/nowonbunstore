using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WebScraping.Library.Config;
using System.Threading;
using WebScraping.Library.Log;

namespace WebScraping.Scraper.Common
{
    class BrokerSender
    {
        //Adapter pattern
        //private Socket _socket;
        private static BrokerSender singleton;
        private List<Thread> queue = new List<Thread>();
        protected Logger logger { get; private set; }
        public static BrokerSender Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new BrokerSender();
                }
                return singleton;
            }
        }
        private BrokerSender()
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
        }
        public void Send(String msg)
        {
            logger.Debug("▼▽▼▽▼▽ SEND BROKER ▼▽▼▽▼▽");
            logger.Debug(msg);
            logger.Debug("△▲△▲△▲ SEND BROKER △▲△▲△▲");
            if (Debug.IsDebug())
            {
                return;
            }
            Thread thread = new Thread(() =>
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                if (Debug.IsDebug())
                {
                    Console.WriteLine(msg);
                    return;
                }
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), BrokerInfo.GetPort()));
                byte[] data = Encoding.UTF8.GetBytes(msg);
                byte[] size = BitConverter.GetBytes(data.Length);

                socket.Send(size, 4, SocketFlags.None);
                socket.Send(data, data.Length, SocketFlags.None);
                try
                {
                    byte[] ret = new byte[1];
                    socket.Receive(ret, 1, SocketFlags.None);
                    socket.Close();
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            });
            queue.Add(thread);
            thread.Start();
        }
        public static void Abort()
        {
            foreach (var q in Instance.queue)
            {
                while (true)
                {
                    if (!q.IsAlive)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
