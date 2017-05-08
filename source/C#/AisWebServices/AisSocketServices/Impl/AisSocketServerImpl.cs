using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AIsSocketServices.Impl
{
    class AisSocketServerImpl : Socket, IAisSocketServer
    {
        class AisClient : TcpClient
        {
            private Stream stream;

            public AisClient(Socket sock)
            {
                this.Client = sock;
                stream = GetStream();
            }

            public static implicit operator AisClient(Socket sock)
            {
                return new AisClient(sock);
            }

            public new bool Connected
            {
                get { return base.Connected; }
            }

            public EndPoint RemoteEndPoint
            {
                get { return base.Client.RemoteEndPoint; }
            }

            public byte[] Receive(int length)
            {
                byte[] buffer = new byte[length];
                byte[] ret = new byte[length];
                int recv = 0;
                int seek = 0;
                while ((recv = stream.Read(buffer, 0, length)) > 0)
                {
                    Array.Copy(buffer, 0, ret, seek, recv);
                    seek += recv;
                    if (seek >= length)
                    {
                        break;
                    }
                }
                return ret;
            }

            public void Send(byte[] data)
            {
                stream.Write(data, 0, data.Length);
            }

            public void SetTimeout(int time)
            {
                stream.WriteTimeout = time;
                stream.ReadTimeout = time;
            }

            public new void Close()
            {
                base.Close();
            }
        }

        private IAisList list;

        public int Port { get; set; }
        public int Timeout { get; set; }

        public AisSocketServerImpl(int port)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            Bind(new IPEndPoint(IPAddress.Any, port));
            Listen(20);
            this.Port = port;
        }
        public void Run()
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                while (true)
                {
                    AisClient client = null;
                    try
                    {
                        client = Accept();
                        Acception(client);
                    }
                    finally
                    {
                        client.Close();
                    }
                }
            });
        }
        private void Acception(AisClient client)
        {
            int length = BitConverter.ToInt32(client.Receive(4), 0);
            String key = Encoding.UTF8.GetString(client.Receive(length));
            byte[] data = Encoding.UTF8.GetBytes(list.Pop(key));
            client.Send(BitConverter.GetBytes(data.Length));
            client.Send(data);
        }
        public void SetList(IAisList list)
        {
            this.list = list;
        }

    }
}
