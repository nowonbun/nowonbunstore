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
        public void SendHandShake(String2 key)
        {
            String2 temp = new String2(Encoding.UTF8);
            temp += "HTTP/1.1 101 Switching Protocols" + String2.CRLF;
            temp += "Upgrade: websocket" + String2.CRLF;
            temp += "Connection: Upgrade" + String2.CRLF;
            temp += "Sec-WebSocket-Accept:" + StaticFunction.ComputeHash(key) + String2.CRLF + String2.CRLF;
            ClientSocket.Send(temp);
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
                    SendWorkTemp("default", DateTime.Now.ToString("yyyy_MM_dd") + "_業務報告");
                    SendFileList(FileMessageType.FileSearch);
                    SendWorkList(WorkType.WorkSearch);

                    while (Receive(out opcode, out data))
                    {
                        if (file.Open && opcode != (int)OPCODE.BINARY)
                        {
                            logger.Error("It's error what transfer the file.");
                            file.Init();
                        }
                        if (opcode == (int)OPCODE.MESSAGE)
                        {
                            IDictionary<String, String> messageBuffer = JsonConvert.DeserializeObject<Dictionary<String, String>>(data.ToString());
                            if (String.Equals(messageBuffer["TYPE"], "1"))
                            {
                                WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.MESSAGE);
                                String chatMessage = ClientSocket.Client.RemoteEndPoint + "-" + messageBuffer["MESSAGE"];
                                builder.SetMessage(chatMessage);
                                String2 message = builder.Build();
                                foreach (WebSocketServer client in clientlist)
                                {
                                    client.Send((int)OPCODE.MESSAGE, message);
                                }
                                logger.Info(message);
                            }
                            else if (String.Equals(messageBuffer["TYPE"], "4"))
                            {
                                FileInfo info = new FileInfo(Program.WORK_PATH + Path.DirectorySeparatorChar + messageBuffer["WORKTITLE"]);
                                String2 data1 = new String2(messageBuffer["MESSAGE"], Encoding.UTF8);
                                using (FileStream stream = new FileStream(info.FullName, FileMode.Create, FileAccess.Write))
                                {
                                    data1.WriteStream(stream);
                                }
                                SendWorkList(WorkType.WorkListNotice);
                            }
                            else if (String.Equals(messageBuffer["TYPE"], "5"))
                            {
                                String data1 = messageBuffer["MESSAGE"];
                                SendWorkTemp(data1.Trim(), data1.Trim());
                            }
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
                                SendFileList((FileMessageType)type);
                                continue;
                            }
                            if (type == (byte)WorkType.WorkSearch || type == (byte)WorkType.WorkListNotice)
                            {
                                SendWorkList((WorkType)type);
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
        private void SendWorkTemp(String file, String title)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.WORKTEMP);
            FileInfo info = new FileInfo(Program.WORK_PATH + Path.DirectorySeparatorChar + file);
            String2 data;
            using (FileStream stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
            {
                data = String2.ReadStream(stream, Encoding.UTF8, (int)info.Length);
            }
            builder.SetWorkTitle(title);
            builder.SetMessage(data.ToString());
            String2 message = builder.Build();
            Send((int)OPCODE.BINARY, message);
        }
        private void SendWorkList(WorkType type)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.WORKLIST);
            DirectoryInfo info = new DirectoryInfo(Program.WORK_PATH);
            FileInfo[] files = info.GetFiles();
            builder.SetFileList(from f in info.GetFiles() where !String.Equals(f.Name, "default") select f.Name);
            String2 message = builder.Build();
            if (type == WorkType.WorkListNotice)
            {
                foreach (WebSocketServer client in clientlist)
                {
                    client.Send((int)OPCODE.BINARY, message);
                }
            }
            else if (type == WorkType.WorkSearch)
            {
                Send((int)OPCODE.BINARY, message);
            }
        }
        private void SendFileList(FileMessageType type)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.FILELIST);
            DirectoryInfo info = new DirectoryInfo(Program.FILE_STORE_PATH);
            FileInfo[] files = info.GetFiles();
            builder.SetFileList(from f in info.GetFiles() select f.Name);
            String2 message = builder.Build();
            if (type == FileMessageType.FileSearch)
            {
                Send((int)OPCODE.BINARY, message);
                if (!clientlist.Contains(this))
                {
                    clientlist.Add(this);
                }
            }
            else if (type == FileMessageType.FileListNotice)
            {
                foreach (WebSocketServer client in clientlist)
                {
                    client.Send((int)OPCODE.BINARY, message);
                }
            }
        }
    }
}
