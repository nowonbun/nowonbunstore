using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using log4net;
using WorkSocketServer;
using Newtonsoft.Json;

namespace WorkServer
{
    class WebSocketServer
    {
        //TODO:This is possible what the problem is occured by with multi thread.
        static FileNode file = FileNode.GetFileNode();
        public static void Run(WorkSocket client, byte opcode, String2 data)
        {
            SendWorkTemp(client, "default", DateTime.Now.ToString("yyyy_MM_dd") + "_業務報告");
            SendFileList(client, FileMessageType.FileSearch);
            SendWorkList(client, WorkType.WorkSearch);

            if (file.Open && opcode != (int)OPCODE.BINARY)
            {
                //logger.Error("It's error what transfer the file.");
                file.Init();
            }
            if (opcode == (int)OPCODE.MESSAGE)
            {
                IDictionary<String, String> messageBuffer = JsonConvert.DeserializeObject<Dictionary<String, String>>(data.ToString());
                if (String.Equals(messageBuffer["TYPE"], "1"))
                {
                    WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.MESSAGE);
                    String chatMessage = client.SocketClient.RemoteEndPoint + "-" + messageBuffer["MESSAGE"];
                    builder.SetMessage(chatMessage);
                    String2 message = builder.Build();
                    Console.WriteLine(message);
                    /*foreach (WebSocketServer client1 in clientlist)
                    {
                        client1.Send((int)OPCODE.MESSAGE, message);
                    }*/
                    //logger.Info(message);
                }
                else if (String.Equals(messageBuffer["TYPE"], "4"))
                {
                    FileInfo info = new FileInfo(Program.WORK_PATH + Path.DirectorySeparatorChar + messageBuffer["WORKTITLE"]);
                    String2 data1 = new String2(messageBuffer["MESSAGE"], Encoding.UTF8);
                    using (FileStream stream = new FileStream(info.FullName, FileMode.Create, FileAccess.Write))
                    {
                        //data1.WriteStream(stream);
                        stream.Write(data1.ToBytes(), 0, data1.Length);
                    }
                    SendWorkList(client, WorkType.WorkListNotice);
                }
                else if (String.Equals(messageBuffer["TYPE"], "5"))
                {
                    String data1 = messageBuffer["MESSAGE"];
                    SendWorkTemp(client, data1.Trim(), data1.Trim());
                }
            }
            if (opcode == (int)OPCODE.BINARY)
            {
                if (data.Length < 1)
                {
                    //logger.Error("It is being have downloading.but because what the data is nothing is stopped.");
                    //continue;
                }
                byte type = data[0];
                if (type == (byte)FileMessageType.FileOpen)
                {
                    file.Length = BitConverter.ToInt32(data.ToBytes(), 1);
                    String2 filename = data.SubString(5, data.Length - 5);
                    filename.Encode = Encoding.UTF8;
                    //logger.Info("filename - " + filename);
                    file.SetStream(new FileStream(Program.FILE_STORE_PATH + filename.Trim().ToString(), FileMode.Create, FileAccess.Write), file.Length);
                    //continue;
                }
                if (type == (byte)FileMessageType.FileWrite)
                {
                    if (!file.Open)
                    {
                        //logger.Error("It is being have downloading.but because what file's connection is closed.");
                        file.Init();
                        //continue;
                    }
                    String2 binary = data.SubString(1, data.Length - 1);
                    file.StreamBuffer.Write(binary.ToBytes(), 0, binary.Length);
                    file.Peek += binary.Length;
                    //logger.Info(file.Peek);
                    if (file.Peek >= file.Length)
                    {
                        file.Complete();
                        client.Send((int)OPCODE.BINARY, new String2("File upload Success!!", Encoding.UTF8));
                    }
                    //continue;
                }
                if (type == (byte)FileMessageType.FileSearch || type == (byte)FileMessageType.FileListNotice)
                {
                    SendFileList(client, (FileMessageType)type);
                    //continue;
                }
                if (type == (byte)WorkType.WorkSearch || type == (byte)WorkType.WorkListNotice)
                {
                    SendWorkList(client, (WorkType)type);
                }
                //logger.Error("FileMessage type is wrong.");
                file.Init();
            }
        }
        private static void SendWorkTemp(WorkSocket client, String file, String title)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.WORKTEMP);
            FileInfo info = new FileInfo(Program.WORK_PATH + Path.DirectorySeparatorChar + file);
            String2 data = new String2((int)info.Length, Encoding.UTF8);
            using (FileStream stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
            {
                stream.Read(data.ToBytes(), 0, data.Length);
                //data = String2.ReadStream(stream, Encoding.UTF8, (int)info.Length);
            }
            builder.SetWorkTitle(title);
            builder.SetMessage(data.ToString());
            String2 message = builder.Build();
            client.Send((int)OPCODE.BINARY, message);
        }
        private static void SendWorkList(WorkSocket client, WorkType type)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.WORKLIST);
            DirectoryInfo info = new DirectoryInfo(Program.WORK_PATH);
            FileInfo[] files = info.GetFiles();
            builder.SetFileList(from f in info.GetFiles() where !String.Equals(f.Name, "default") select f.Name);
            String2 message = builder.Build();
            if (type == WorkType.WorkListNotice)
            {
                /*foreach (WebSocketServer client in clientlist)
                {
                    client.Send((int)OPCODE.BINARY, message);
                }*/
            }
            else if (type == WorkType.WorkSearch)
            {
                client.Send((int)OPCODE.BINARY, message);
            }
        }
        private static void SendFileList(WorkSocket client, FileMessageType type)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.FILELIST);
            DirectoryInfo info = new DirectoryInfo(Program.FILE_STORE_PATH);
            FileInfo[] files = info.GetFiles();
            builder.SetFileList(from f in info.GetFiles() select f.Name);
            String2 message = builder.Build();
            if (type == FileMessageType.FileSearch)
            {
                client.Send((int)OPCODE.BINARY, message);
                /*if (!clientlist.Contains(this))
                {
                    clientlist.Add(this);
                }*/
            }
            else if (type == FileMessageType.FileListNotice)
            {
                /*foreach (WebSocketServer client in clientlist)
                {
                    client.Send((int)OPCODE.BINARY, message);
                }*/
            }
        }
    }
}
