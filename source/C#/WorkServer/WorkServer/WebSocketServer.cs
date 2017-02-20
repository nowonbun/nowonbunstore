using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.IO;

namespace WorkServer
{
    class WebSocketServer : WorkServer
    {
        private List<Client> clientlist = new List<Client>();
        private static String2 GUID = new String2("258EAFA5-E914-47DA-95CA-C5AB0DC85B11", Encoding.UTF8);
        private Client client;
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
        public WebSocketServer(Client client) : base(client) { }

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
                Console.WriteLine(e);
                return false;
            }
        }
        public override void Run()
        {
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
                            Console.WriteLine("data null");
                            continue;
                        }
                        if (file.Open && opcode != 2)
                        {
                            Console.WriteLine("it's error what transfer the file");
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
                                Console.WriteLine("transfer typecode?");
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
                                    Console.WriteLine("transfer error");
                                    file.Init();
                                    continue;
                                }
                                String2 binary = data.SubString(1, data.Length - 1);
                                binary.WriteStream(file.Stream);
                                file.Peek += binary.Length;
                                if (file.Peek >= file.Length)
                                {
                                    file.Complete();
                                    Send(2,new String2("File upload Success!!",Encoding.UTF8));
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
                                }
                                sb.Length = sb.Length - 1;
                                sb.Append("]}");
                                Send(2, new String2(sb.ToString(), Encoding.UTF8));
                                continue;
                            }
                            Console.WriteLine("error");
                            file.Init();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    client.Dispose();
                    clientlist.Remove(client);
                }
            });
        }
        public void Send(int opcode)
        {
            Send(opcode,null);
        }
        public void Send(int opcode, String2 data)
        {
            Send(client,opcode, null);
        }
        public void Send(Client sock, int opcode, String2 data)
        {
            if ((opcode == 1 || opcode == 2) && data != null)
            {

            }
            else if (opcode == 9)
            {

            }
            else if (opcode == 10)
            {

            }
            Console.WriteLine("send OPCDE = "+opcode);
        }
        public bool Receive(out byte opcode, out String2 data)
        {
            while (true)
            {
                data = null;
                opcode = (byte)0;
                return true;
            }
        }
        private String2 GetKey(String2 key)
        {
            byte[] hash = ComputeHash((String)(key + GUID));
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
