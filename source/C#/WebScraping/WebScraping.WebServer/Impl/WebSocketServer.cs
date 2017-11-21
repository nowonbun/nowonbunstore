using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;

namespace WebScraping.WebServer.Impl
{
    public class WebSocketServer : Socket, IWebSocketServer, IDisposable
    {
        private int BUFFER_SIZE = 4096;
        private static String GUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private static SHA1 SHA = SHA1CryptoServiceProvider.Create();
        private List<Socket> clients = new List<Socket>();
        private List<Func<byte[], WebSocketNode>> methods = new List<Func<byte[], WebSocketNode>>();
        private List<Func<byte[], WebSocketNode>> methodAlls = new List<Func<byte[], WebSocketNode>>();

        public WebSocketServer(int port)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
        {
            Bind(new IPEndPoint(IPAddress.Any, port));
            Listen(20);
            ServerStart();
        }
        private void ServerStart()
        {
            StringBuilder err = new StringBuilder();
            err.AppendLine("HTTP/1.1 404 Not Found");
            err.AppendLine("Transfer - Encoding: chunked");
            byte[] error = Encoding.UTF8.GetBytes(err.ToString());
            ThreadPool.QueueUserWorkItem((c) =>
            {
                while (true)
                {
                    Socket client = null;
                    try
                    {
                        client = Accept();
                        byte[] buffer = new byte[BUFFER_SIZE];
                        client.Receive(buffer, BUFFER_SIZE, SocketFlags.None);
                        var header = GetHeader(buffer);
                        if (!(header.ContainsKey("Upgrade") && "websocket".Equals(header["Upgrade"])))
                        {

                            //HTTP/1.1 404 Not Found
                            //Transfer - Encoding: chunked
                            client.Send(error);
                            continue;
                        }
                        byte[] rvheader = Handshake(header["Sec-WebSocket-Key"]);
                        client.Send(rvheader);
                        Receiver(client);
                    }
                    catch (Exception e)
                    {
                        if (client != null)
                        {
                            client.Dispose();
                        }
                        throw e;
                    }
                }
            });
        }
        private byte[] Receive(Socket client, int length)
        {
            if (!client.Connected)
            {
                throw new Exception("Disconnection");
            }
            byte[] buffer = new byte[length];
            client.Receive(buffer, length, SocketFlags.None);
            return buffer;
        }
        private byte[] Reverse(byte[] data)
        {
            byte[] ret = new byte[data.Length];
            Array.Copy(data, ret, ret.Length);
            Array.Reverse(ret);
            return ret;
        }
        private void Receiver(Socket client)
        {
            clients.Add(client);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
                        byte opcode = (byte)0;
                        byte[] head = Receive(client, 2);
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
                            length = BitConverter.ToInt16(Reverse(Receive(client, 2)), 0);
                        }
                        if (length == 0x7F)
                        {
                            length = (int)BitConverter.ToInt64(Reverse(Receive(client, 8)), 0);
                        }
                        byte[] key = mask ? Receive(client, 4) : null;
                        if (opcode == (int)Opcode.MESSAGE)
                        {
                            byte[] buffer = Receive(client, length);
                            if (key != null)
                            {
                                for (int i = 0; i < buffer.Length; i++)
                                {
                                    buffer[i] = (byte)(buffer[i] ^ key[i % 4]);
                                }
                            }
                            methods.ForEach(action =>
                            {
                                WebSocketNode message = action(buffer);
                                this.Send(client, (int)message.OPCode, message.Message);
                            });
                            methodAlls.ForEach(action =>
                            {
                                WebSocketNode message = action(buffer);
                                this.Send((int)message.OPCode, message.Message);
                            });
                            continue;
                        }
                        if (opcode == (int)Opcode.BINARY)
                        {
                            byte[] buffer = Receive(client, length);
                            if (key != null)
                            {
                                for (int i = 0; i < buffer.Length; i++)
                                {
                                    buffer[i] = (byte)(buffer[i] ^ key[i % 4]);
                                }
                            }
                            methods.ForEach(action =>
                            {
                                WebSocketNode message = action(buffer);
                                this.Send(client, (int)message.OPCode, message.Message);
                            });
                            methodAlls.ForEach(action =>
                            {
                                WebSocketNode message = action(buffer);
                                this.Send((int)message.OPCode, message.Message);
                            });
                            continue;
                        }
                        if (opcode == (int)Opcode.EXIT)
                        {
                            return;
                        }
                        if ((opcode == (int)Opcode.PING) || (opcode == (int)Opcode.PONG))
                        {
                            Send(client, opcode, null);
                            continue;
                        }
                        throw new Exception("This opcode is wrong. Receive OPCODE - " + opcode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    client.Close();
                }
                finally
                {
                    clients.Remove(client);
                }
            });
        }
        public void Send(WebSocketNode node)
        {
            Send((int)node.OPCode, node.Message);
        }
        public void Send(int opcode, byte[] data)
        {
            foreach (var client in clients)
            {
                Send(client, opcode, data);
            }
        }
        public void Send(Socket client, int opcode, byte[] data)
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
                        client.Send(Reverse(BitConverter.GetBytes((short)data.Length)));
                    }
                    else
                    {
                        client.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7F });
                        client.Send(Reverse(BitConverter.GetBytes((long)data.Length)));
                    }
                    client.Send(data, SocketFlags.None);
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
                Console.WriteLine(e);
                client.Close();
            }
        }
        private Dictionary<String, String> GetHeader(byte[] data)
        {
            String buffer = Encoding.UTF8.GetString(data).Trim('\0');
            String[] buffer2 = buffer.Split('\r', '\n');
            Dictionary<String, String> ret = new Dictionary<string, string>();
            for (int i = 0; i < buffer2.Length; i++)
            {
                if (buffer2[i].Length < 1)
                {
                    continue;
                }
                int pos = buffer2[i].IndexOf(":");
                if (pos < 1)
                {
                    continue;
                }
                String key = buffer2[i].Substring(0, pos).Trim();
                string value = buffer2[i].Substring(pos + 1).Trim();
                ret.Add(key, value);
            }
            return ret;
        }
        private byte[] Handshake(String key)
        {
            String temp = "";
            temp += "HTTP/1.1 101 Switching Protocols\r\n";
            temp += "Upgrade: websocket\r\n";
            temp += "Connection: Upgrade\r\n";
            temp += "Sec-WebSocket-Accept:" + ComputeHash(key) + "\r\n\r\n";
            return Encoding.UTF8.GetBytes(temp);
        }
        private String ComputeHash(String key)
        {
            String buffer = key.Trim().ToString() + GUID;
            byte[] hash = SHA.ComputeHash(Encoding.ASCII.GetBytes(buffer));
            return Convert.ToBase64String(hash);
        }
        public void Get(Func<byte[], WebSocketNode> method)
        {
            methods.Add(method);
        }
        public void GetAll(Func<byte[], WebSocketNode> method)
        {
            methodAlls.Add(method);
        }
    }
}
