using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WebScraping.Library.Config;
using WebScraping.Library.Log;

namespace WebScraping.Broker
{
    class ServerConnector
    {
        private Logger logger = null;
        private static ServerConnector singleton = null;
        public event Action<String> OnReceive;
        private Socket _socket;
        public static ServerConnector Instance()
        {
            if (singleton == null)
            {
                singleton = new ServerConnector();
            }
            return singleton;
        }

        private ServerConnector()
        {
            logger = LoggerBuilder.Init().Set(GetType()).Info("Connector Start!");
            QueueThread.Push(() =>
            {
                while (true)
                {
                    try
                    {
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ServerInfo.GetIp()), ServerInfo.GetPort());
                        _socket.Connect(ipep);
                        ControlFactory.GetForm<Main>().SetConnection(true);
                        while (true)
                        {
                            byte[] size = new byte[4];
                            _socket.Receive(size, 4, SocketFlags.None);
                            byte[] data = new byte[BitConverter.ToInt32(size, 0)];
                            _socket.Receive(data, data.Length, SocketFlags.None);
                            String buf = Encoding.UTF8.GetString(data);
                            if (OnReceive != null)
                            {
                                logger.Info(" [NODE LOG] The data is recieved from node- " + buf);
                                OnReceive(buf);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ControlFactory.GetForm<Main>().SetConnection(false);
                        logger.Error(" [SCRAP LOG] - " + e.ToString());
                    }
                }
            });
        }

        public void Send(byte[] msg)
        {
            try
            {
                byte[] size = BitConverter.GetBytes(msg.Length);
                _socket.Send(size, 4, SocketFlags.None);
                _socket.Send(msg, msg.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                logger.Error(" [SCRAP LOG] Send error - " + e.ToString());
                logger.Error(" [SCRAP LOG] Send error msseage - " + Encoding.UTF8.GetString(msg));
            }
        }

        public void Send(String msg)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                byte[] size = BitConverter.GetBytes(data.Length);

                _socket.Send(size, 4, SocketFlags.None);
                _socket.Send(data, data.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                logger.Error(" [SCRAP LOG] Send error - " + e.ToString());
                logger.Error(" [SCRAP LOG] Send error msseage - " + msg);
            }
        }
    }
}
