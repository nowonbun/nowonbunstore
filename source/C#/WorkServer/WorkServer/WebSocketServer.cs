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
        private List<Client> clientlist = new List<Client>();
        private static String GUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private static SHA1 SHA = null;
        public class FileNode
        {
            public FileStream Stream { get; private set; }
            public int Peek { get; set; }
            public int Length { get; set; }
            public bool Open { get; set; }
            public FileNode()
            {
                Init();
            }
            public void Init()
            {
                if (Stream != null)
                {
                    Stream.Close();
                }
                Stream = null;
                Peek = 0;
                Length = 0;
                Open = false;
            }
            public void Complete()
            {
                if (Stream != null)
                {
                    Stream.Flush();
                }
                Init();
            }
            public void SetStream(FileStream stream, int length)
            {
                this.Stream = stream;
                this.Peek = 0;
                this.Length = length;
                this.Open = true;
            }
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
                temp += "Sec-WebSocket-Accept:" + GetKey(header.Get("Sec-WebSocket-Key")) + String2.CRLF + String2.CRLF;
                ClientSocket.Send(temp);
                clientlist.Add(ClientSocket);
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
            logger.Debug(ClientSocket.Client.LocalEndPoint);
            logger.Debug(ClientSocket.Client.RemoteEndPoint);
            ThreadPool.QueueUserWorkItem((c) =>
            {
                try
                {
                    FileNode file = new FileNode(); ;
                    String2 data;
                    byte opcode;
                    while (Receive(out opcode, out data))
                    {
                        if (data == null)
                        {
                            logger.Error("data null");
                            continue;
                        }
                        if (file.Open && opcode != 2)
                        {
                            logger.Error("it's error what transfer the file");
                            file.Init();
                        }
                        if (opcode == 1)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("{\"type\":\"0\",\"message\":");
                            sb.Append("\"");
                            sb.Append(data);
                            sb.Append("\"");
                            sb.Append("}");
                            foreach (Client sock in clientlist)
                            {

                            }
                            Console.WriteLine(data);
                            continue;
                        }
                        if (opcode == 2)
                        {
                            if (data.Length < 1)
                            {
                                logger.Error("transfer typecode?");
                                continue;
                            }
                            if (data[0] == 1)
                            {
                                file.Length = BitConverter.ToInt32(data.ToBytes(), 1);
                                String2 filename = data.SubString(5, data.Length - 5);
                                file.SetStream(new FileStream(Program.FILE_STORE_PATH + filename.Trim().ToString(), FileMode.Create, FileAccess.Write), file.Length);
                                continue;
                            }
                            if (data[0] == 2)
                            {
                                if (!file.Open)
                                {
                                    logger.Error("transfer error");
                                    file.Init();
                                    continue;
                                }
                                String2 binary = data.SubString(1, data.Length - 1);
                                binary.WriteStream(file.Stream);
                                file.Peek += binary.Length;
                                if (file.Peek >= file.Length)
                                {
                                    file.Complete();
                                    Send(2, new String2("File upload Success!!", Encoding.UTF8));
                                }
                                continue;
                            }
                            if (data[0] == 7)
                            {
                                DirectoryInfo info = new DirectoryInfo(Program.FILE_STORE_PATH);
                                FileInfo[] files = info.GetFiles();
                                StringBuilder sb = new StringBuilder();
                                sb.Append("{\"type\":\"1\",\"list\":[");
                                foreach (FileInfo f in files)
                                {
                                    sb.Append("\"");
                                    sb.Append(f.Name);
                                    sb.Append("\"");
                                    sb.Append(",");
                                }
                                sb.Length = sb.Length - 1;
                                sb.Append("]}");
                                Send(2, new String2(sb.ToString(), Encoding.UTF8));
                                continue;
                            }
                            logger.Error("error");
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
                    ClientSocket.Dispose();
                    clientlist.Remove(ClientSocket);
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
            if ((opcode == 1 || opcode == 2) && data != null)
            {
                if (data.Length <= 128)
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
            else if (opcode == 9)
            {
                sock.Send(new byte[] { (byte)(0x80 | 9), (byte)0x00 });
                return;
            }
            else if (opcode == 10)
            {
                sock.Send(new byte[] { (byte)(0x80 | 10), (byte)0x00 });
                return;
            }
            logger.Error("send OPCDE = " + opcode);
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
                if (opcode == 1)
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
                if (opcode == 2)
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
                if (opcode == 9)
                {
                    Send(9);
                    continue;
                }
                if (opcode == 10)
                {
                    Send(10);
                    continue;
                }
                logger.Error("Receive OPCODE - " + opcode);
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
        private String GetKey(String2 key)
        {
            byte[] hash = ComputeHash(key.Trim().ToString() + GUID);
            return Convert.ToBase64String(hash);
        }
        private static byte[] ComputeHash(String str)
        {
            if (SHA == null)
            {
                SHA = SHA1CryptoServiceProvider.Create();
            }
            return SHA.ComputeHash(Encoding.ASCII.GetBytes(str));
        }
    }
}
