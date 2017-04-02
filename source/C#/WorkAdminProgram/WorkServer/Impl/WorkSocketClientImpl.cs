using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using WorkServer;

namespace WorkServer
{
    partial class WorkSocketImpl : IWorkSocketClient
    {
        private static SHA1 SHA = null;
        private IClient client;
        public Exception LastException { get; set; }
        private Action<IWorkSocketClient, byte, String2> receive;
        
        public IClient SocketClient
        {
            get
            {
                return client;
            }
        }

        public WorkSocketImpl(IClient client)
        {
            this.client = client;
            client.SetTimeout(86400000);
        }

        public void Initialize(String2 key)
        {
            try
            {
                SendHandShake(key);
                Receive();
            }
            catch (Exception e)
            {
                LastException = e;
                WorkSocketFactory.GetWorkSocketServer().RemoveSocketClient(this);
            }
        }

        public void SetReceiveEvent(Action<IWorkSocketClient, byte, String2> receive)
        {
            this.receive = receive;
        }

        public void Send(int opcode)
        {
            Send(opcode, null);
        }

        public void Send(int opcode, String2 data)
        {
            try
            {
                if ((opcode == (int)Opcode.MESSAGE) || (opcode == (int)Opcode.BINARY) && data != null)
                {
                    if (data.Length <= 0x80)
                    {
                        client.Send(new byte[] { (byte)(0x80 | 1), (byte)data.Length });
                    }
                    else if (data.Length <= 65535)
                    {
                        client.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7E });
                        client.Send(Util.Reverse(BitConverter.GetBytes((short)data.Length)));
                    }
                    else
                    {
                        client.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7F });
                        client.Send(Util.Reverse(BitConverter.GetBytes((long)data.Length)));
                    }
                    client.Send(data.ToBytes());
                    return;
                }
                else if ((opcode == (int)Opcode.PING) || (opcode == (int)Opcode.PONG))
                {
                    client.Send(new byte[] { (byte)(0x80 | opcode), (byte)0x00 });
                    return;
                }
                throw new Exception("This setting is wrong OPCODE = " + opcode);
            }
            catch (Exception e)
            {
                LastException = e;
                client.Close();
            }
        }

        private void Receive()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
                        String2 data = null;
                        byte opcode = (byte)0;
                        String2 head = Receive(2);
                        if (head.Length < 2)
                        {
                            throw new Exception("header size");
                        }
                        bool fin = (head[0] & 0x80) == 0x80;
                        if (!fin)
                        {
                            throw new Exception("Fin error");
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
                        if (opcode == (int)Opcode.MESSAGE)
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
                            receive(this, opcode, data);
                            continue;
                        }
                        if (opcode == (int)Opcode.BINARY)
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
                            receive(this, opcode, data);
                            continue;
                        }
                        if (opcode == (int)Opcode.EXIT)
                        {
                            return;
                        }
                        if ((opcode == (int)Opcode.PING) || (opcode == (int)Opcode.PONG))
                        {
                            Send(opcode);
                            continue;
                        }
                        throw new Exception("This opcode is wrong. Receive OPCODE - " + opcode);
                    }
                }
                catch (Exception e)
                {
                    LastException = e;
                    client.Close();
                }
                finally
                {
                    WorkSocketFactory.GetWorkSocketServer().RemoveSocketClient(this);
                }
            });
        }

        private String2 Receive(int length)
        {
            if (!client.Connected)
            {
                throw new Exception("Disconnection");
            }
            return client.Receive(length);
        }

        private void SendHandShake(String2 key)
        {
            String2 temp = new String2(Encoding.UTF8);
            temp += "HTTP/1.1 101 Switching Protocols" + String2.CRLF;
            temp += "Upgrade: websocket" + String2.CRLF;
            temp += "Connection: Upgrade" + String2.CRLF;
            temp += "Sec-WebSocket-Accept:" + ComputeHash(key) + String2.CRLF + String2.CRLF;
            client.Send(temp.ToBytes());
        }

        private String2 ComputeHash(String2 key)
        {
            if (SHA == null)
            {
                SHA = SHA1CryptoServiceProvider.Create();
            }
            String buffer = key.Trim().ToString() + Define.GUID;
            byte[] hash = SHA.ComputeHash(Encoding.ASCII.GetBytes(buffer));
            return new String2(Convert.ToBase64String(hash), Encoding.UTF8);
        }
    }
}
