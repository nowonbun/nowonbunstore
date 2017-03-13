using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using WorkServer;

namespace WorkSocketServer
{
    partial class WorkSocketImpl : WorkSocket
    {
        private Client client;
        private Action<WorkSocket, byte, String2> receive;

        public Client SocketClient
        {
            get
            {
                return client;
            }
        }

        public WorkSocketImpl(Client client, Action<WorkSocket, byte, String2> receive)
        {
            this.client = client;
            this.receive = receive;
            client.SetTimeout(86400000);
        }

        public void Initialize(String2 key)
        {
            SendHandShake(key);
            Receive();
        }

        public void Send(int opcode)
        {
            Send(opcode, null);
        }

        public void Send(int opcode, String2 data)
        {
            if ((opcode == (int)OPCODE.MESSAGE) || (opcode == (int)OPCODE.BINARY) && data != null)
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
            else if ((opcode == (int)OPCODE.PING) || (opcode == (int)OPCODE.PONG))
            {
                client.Send(new byte[] { (byte)(0x80 | opcode), (byte)0x00 });
                return;
            }
            throw new Exception("This setting is wrong OPCODE = " + opcode);
        }

        private void Receive()
        {
            ThreadPool.QueueUserWorkItem(_ =>
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
                        receive(this, opcode, data);
                        continue;
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
                        receive(this, opcode, data);
                        continue;
                    }
                    if (opcode == (int)OPCODE.EXIT)
                    {
                        return;
                    }
                    if ((opcode == (int)OPCODE.PING) || (opcode == (int)OPCODE.PONG))
                    {
                        Send(opcode);
                        continue;
                    }
                    throw new Exception("This opcode is wrong. Receive OPCODE - " + opcode);
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
    }
}
