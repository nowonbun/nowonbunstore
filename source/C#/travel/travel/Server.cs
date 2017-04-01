using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using log4net;
using log4net.Config;

namespace travel
{
    class Server : Socket
    {
        private ILog logger = LogManager.GetLogger(typeof(Server));
        private bool run = true;

        public Server() : base(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.IP)
        {
            base.Bind(new IPEndPoint(IPAddress.Any, 9999));
            base.Listen(20);
        }
        public void Run()
        {
            if (run)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    while (true)
                    {
                        Receive(Accept());
                    }
                });
                run = false;
            }
            else
            {
                logger.Error("already start");
            }
            
        }
        private void Receive(Socket sock)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.Client = sock;
                    byte[] data = new byte[1024];
                    int idx = 0;
                    client.GetStream().ReadTimeout = 1000;
                    while(true)
                    {
                        data[idx++] = (byte)client.GetStream().ReadByte();
                        if (data.Length <= idx)
                        {
                            byte[] buffer = data;
                            data = new byte[data.Length + 1024];
                            Array.Copy(buffer, 0, data, 0, buffer.Length);
                        }
                        if (idx > 3 && data[idx - 4] == 0x0D && data[idx - 3] == 0x0A && data[idx - 2] == 0x0D && data[idx - 1] == 0x0A)
                        {
                            break;
                        }
                    }
                    String aa = Encoding.Default.GetString(data, 0, idx);
                    logger.Info(aa);
                    String[] header = aa.Split('\n')[0].Split(' ');
                    if (!String.Equals("GET", header[0]))
                    {
                        data = Encoding.Default.GetBytes("Error");
                        client.GetStream().Write(data, 0, data.Length);
                        client.Close();
                        return;
                    }
                    String code = header[1];
                    data = Encoding.Default.GetBytes(code);
                    client.GetStream().Write(data, 0, data.Length);
                    client.Close();
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            });
        }
    }
}
