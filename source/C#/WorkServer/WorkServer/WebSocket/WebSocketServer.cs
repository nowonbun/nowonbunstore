using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using log4net;
using Newtonsoft.Json;

namespace WorkServer
{
    partial class WebSocketServer : WorkServer
    {
        private static ILog logger = LogManager.GetLogger(typeof(WebSocketServer));
        private static IList<WebSocketServer> clientlist = new List<WebSocketServer>();
        public static IList<WebSocketServer> ClientList
        {
            get { return clientlist; }
        }
        public WebSocketServer(Client client)
            : base(client)
        {
            client.SetTimeout(86400000);
        }
        public override bool Initialize(HandShake header)
        {
            try
            {
                SendHandShake(header.Get("Sec-WebSocket-Key"));
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }

        public void Send(int opcode)
        {
            Send(opcode, null);
        }
        public void Send(int opcode, String2 data)
        {
            Send(ClientSocket, opcode, data);
        }
        public void Send(Client sock, int opcode, String2 data)
        {
            if ((opcode == (int)OPCODE.MESSAGE) || (opcode == (int)OPCODE.BINARY) && data != null)
            {
                if (data.Length <= 0x80)
                {
                    sock.Send(new byte[] { (byte)(0x80 | 1), (byte)data.Length });
                }
                else if (data.Length <= 65535)
                {
                    sock.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7E });
                    sock.Send(new String2(BitConverter.GetBytes((short)data.Length)).Reverse());
                }
                else
                {
                    sock.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7F });
                    sock.Send(new String2(BitConverter.GetBytes((long)data.Length)).Reverse());
                }
                sock.Send(data);
                return;
            }
            else if ((opcode == (int)OPCODE.PING) || (opcode == (int)OPCODE.PONG))
            {
                sock.Send(new byte[] { (byte)(0x80 | opcode), (byte)0x00 });
                return;
            }
            logger.Error("This setting is wrong OPCODE = " + opcode);
        }
        public bool Receive(out byte opcode, out String2 data)
        {
            while (true)
            {
                data = null;
                opcode = (byte)0;
                String2 head = Receive(2);
                if (head.Length < 2)
                {
                    return false;
                }
                bool fin = (head[0] & 0x80) == 0x80;
                if (!fin)
                {
                    logger.Error("Fin error");
                    return false;
                }
                opcode = (byte)(head[0] & 0x0f);
                bool mask = (head[1] & 0x80) == 0x80;
                int length = head[1] & 0x7F;
                if (length == 0x7E)
                {
                    length = BitConverter.ToInt16(Receive(2).Reverse().ToBytes(), 0);
                }
                if (length == 0x7F)
                {
                    length = (int)BitConverter.ToInt64(Receive(8).Reverse().ToBytes(), 0);
                }
                String2 key = mask ? Receive(4) : null;
                if (opcode == (int)OPCODE.MESSAGE)
                {
                    byte[] buffer = Receive(length).ToBytes();
                    if (key != null)
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = (byte)(buffer[i] ^ key[i % 4]);
                        }
                    }
                    data = new String2(buffer, Encoding.UTF8);
                    return true;
                }
                if (opcode == (int)OPCODE.BINARY)
                {
                    byte[] buffer = Receive(length).ToBytes();
                    if (key != null)
                    {
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = (byte)(buffer[i] ^ key[i % 4]);
                        }
                    }
                    data = buffer;
                    return true;
                }
                if (opcode == (int)OPCODE.EXIT)
                {
                    return false;
                }
                if ((opcode == (int)OPCODE.PING) || (opcode == (int)OPCODE.PONG))
                {
                    Send(opcode);
                    continue;
                }
                logger.Error("This opcode is wrong. Receive OPCODE - " + opcode);
                return false;
            }
        }
        private String2 Receive(int length)
        {
            if (!ClientSocket.Connected)
            {
                throw new Exception("Disconnection");
            }
            return ClientSocket.Receive(length);
        }
    }
}
