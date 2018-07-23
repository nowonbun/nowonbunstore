using System;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.Net.Sockets;
using PTM.Httpd.Util;

namespace PTM.Httpd.Impl
{
    class WebSocket
    {
        private static String GUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private static SHA1 SHA = SHA1CryptoServiceProvider.Create();
        private Socket _socket;
        private Request _header;
        private Server _server;
        private Func<String2, WebSocketNode> _method;

        public WebSocket(Socket socket, Server server, Request req, Func<String2, WebSocketNode> method)
        {
            this._socket = socket;
            this._header = req;
            this._method = method;
            this._server = server;
            String2 rvheader = Handshake(req.Header["Sec-WebSocket-Key"]);
            socket.Send(rvheader.ToBytes(), rvheader.Length, SocketFlags.None);
            Listen();
        }
        private void Listen()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
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
                            length = BitConverter.ToInt16(Reverse(Receive(2).ToBytes()), 0);
                        }
                        if (length == 0x7F)
                        {
                            length = (int)BitConverter.ToInt64(Reverse(Receive(8).ToBytes()), 0);
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
                            if (this._method != null)
                            {
                                WebSocketNode node = this._method(buffer);
                                if (node.IsBroadCast)
                                {
                                    _server.Send((int)node.OPCode, node.Message);
                                }
                                else
                                {
                                    Send((int)node.OPCode, node.Message);
                                }
                            }
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
                            if (this._method != null)
                            {
                                WebSocketNode node = this._method(buffer);
                                if (node.IsBroadCast)
                                {
                                    _server.Send((int)node.OPCode, node.Message);
                                }
                                else
                                {
                                    Send((int)node.OPCode, node.Message);
                                }
                            }
                            continue;
                        }
                        if (opcode == (int)Opcode.EXIT)
                        {
                            return;
                        }
                        if ((opcode == (int)Opcode.PING) || (opcode == (int)Opcode.PONG))
                        {
                            Send(opcode, null);
                            continue;
                        }
                        throw new Exception("This opcode is wrong. Receive OPCODE - " + opcode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _socket.Close();
                    _server.RemoveWebSocket(this);
                }
            });
        }
        public void Send(int opcode, String2 data)
        {
            try
            {
                if ((opcode == (int)Opcode.MESSAGE) || (opcode == (int)Opcode.BINARY) && data != null)
                {
                    if (data.Length <= 0x80)
                    {
                        _socket.Send(new byte[] { (byte)(0x80 | 1), (byte)data.Length });
                    }
                    else if (data.Length <= 65535)
                    {
                        _socket.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7E });
                        _socket.Send(Reverse(BitConverter.GetBytes((short)data.Length)));
                    }
                    else
                    {
                        _socket.Send(new byte[] { (byte)(0x80 | 1), (byte)0x7F });
                        _socket.Send(Reverse(BitConverter.GetBytes((long)data.Length)));
                    }
                    _socket.Send(data.ToBytes(), data.Length, SocketFlags.None);
                    return;
                }
                else if ((opcode == (int)Opcode.PING) || (opcode == (int)Opcode.PONG))
                {
                    _socket.Send(new byte[] { (byte)(0x80 | opcode), (byte)0x00 });
                    return;
                }
                throw new Exception("This setting is wrong OPCODE = " + opcode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _socket.Close();
                _server.RemoveWebSocket(this);
            }
        }
        private byte[] Reverse(byte[] data)
        {
            byte[] ret = new byte[data.Length];
            Array.Copy(data, ret, ret.Length);
            Array.Reverse(ret);
            return ret;
        }
        private String2 Receive(int length)
        {
            if (!_socket.Connected)
            {
                throw new Exception("Disconnection");
            }
            byte[] buffer = new byte[length];
            _socket.Receive(buffer, length, SocketFlags.None);
            return buffer;
        }
        private String2 Handshake(String2 key)
        {
            String2 temp = new String2(0);
            temp += "HTTP/1.1 101 Switching Protocols" + String2.CRLF;
            temp += "Upgrade: websocket" + String2.CRLF;
            temp += "Connection: Upgrade" + String2.CRLF;
            temp += "Sec-WebSocket-Accept:" + ComputeHash(key) + String2.CRLF + String2.CRLF;
            return temp;
        }
        private String ComputeHash(String2 key)
        {
            String buffer = key.Trim().ToString() + GUID;
            byte[] hash = SHA.ComputeHash(Encoding.ASCII.GetBytes(buffer));
            return Convert.ToBase64String(hash);
        }
    }
}
