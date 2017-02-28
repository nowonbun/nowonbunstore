using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using log4net;


namespace WorkServer
{
    class WebSocketServer : WorkServer
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
                String2 temp = new String2(Encoding.UTF8);
                temp += "HTTP/1.1 101 Switching Protocols" + String2.CRLF;
                temp += "Upgrade: websocket" + String2.CRLF;
                temp += "Connection: Upgrade" + String2.CRLF;
                temp += "Sec-WebSocket-Accept:" + StaticFunction.ComputeHash(header.Get("Sec-WebSocket-Key")) + String2.CRLF + String2.CRLF;
                ClientSocket.Send(temp);
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }
        public override void Run()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                FileNode file = FileNode.GetFileNode();
                try
                {
                    String2 data;
                    byte opcode;
                    while (Receive(out opcode, out data))
                    {
                        if (file.Open && opcode != (int)OPCODE.BINARY)
                        {
                            logger.Error("It's error what transfer the file.");
                            file.Init();
                        }
                        if (opcode == (int)OPCODE.MESSAGE)
                        {
                            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.MESSAGE);
                            String chatMessage = ClientSocket.Client.RemoteEndPoint +"-"+data.ToString();
                            builder.SetMessage(chatMessage);
                            String2 message = builder.Build();
                            foreach (WebSocketServer client in clientlist)
                            {
                                client.Send((int)OPCODE.MESSAGE, message);
                            }
                            logger.Info(message);
                            continue;
                        }
                        if (opcode == (int)OPCODE.BINARY)
                        {
                            if (data.Length < 1)
                            {
                                logger.Error("It is being have downloading.but because what the data is nothing is stopped.");
                                continue;
                            }
                            byte type = data[0];
                            if (type == (byte)FileMessageType.FileOpen)
                            {
                                file.Length = BitConverter.ToInt32(data.ToBytes(), 1);
                                String2 filename = data.SubString(5, data.Length - 5);
                                filename.Encode = Encoding.UTF8;
                                logger.Info("filename - " + filename);
                                file.SetStream(new FileStream(Program.FILE_STORE_PATH + filename.Trim().ToString(), FileMode.Create, FileAccess.Write), file.Length);
                                continue;
                            }
                            if (type == (byte)FileMessageType.FileWrite)
                            {
                                if (!file.Open)
                                {
                                    logger.Error("It is being have downloading.but because what file's connection is closed.");
                                    file.Init();
                                    continue;
                                }
                                String2 binary = data.SubString(1, data.Length - 1);
                                binary.WriteStream(file.StreamBuffer);
                                file.Peek += binary.Length;
                                logger.Info(file.Peek);
                                if (file.Peek >= file.Length)
                                {
                                    file.Complete();
                                    Send((int)OPCODE.BINARY, new String2("File upload Success!!", Encoding.UTF8));
                                }
                                continue;
                            }
                            if (type == (byte)FileMessageType.FileSearch || type == (byte)FileMessageType.FileListNotice)
                            {
                                WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.FILELIST);
                                DirectoryInfo info = new DirectoryInfo(Program.FILE_STORE_PATH);
                                FileInfo[] files = info.GetFiles();
                                builder.SetFileList(from f in info.GetFiles() select f.Name);
                                String2 message = builder.Build();
                                if (type == (byte)FileMessageType.FileSearch)
                                {
                                    Send((int)OPCODE.BINARY, message);
                                    if (!clientlist.Contains(this))
                                    {
                                        clientlist.Add(this);
                                    }
                                }
                                else if (type == (byte)FileMessageType.FileListNotice)
                                {
                                    foreach (WebSocketServer client in clientlist)
                                    {
                                        client.Send((int)OPCODE.BINARY, message);
                                    }
                                }
                                continue;
                            }
                            logger.Error("FileMessage type is wrong.");
                            file.Init();
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
                finally
                {
                    file.Init();
                    ClientSocket.Dispose();
                    clientlist.Remove(this);
                }
            });
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
