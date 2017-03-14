using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using WorkServer;

namespace WorkAdminProgram
{
    class SocketController
    {
        private IWorkSocketClient webclient;
        private FileNode file = FileNode.GetFileNode();
        public static SocketController NewInstance(IWorkSocketClient webclient)
        {
            return new SocketController(webclient);
        }
        public Action<IWorkSocketClient, byte, String2> SetReceive()
        {
            return (client, opcode, data) =>
            {
                if (file.Open && opcode != (int)Opcode.BINARY)
                {
                    file.Init();
                }
                if (opcode == (int)Opcode.MESSAGE)
                {
                    MessageNode message = MessageDirector.Instance().GetNodeFromJson(data);
                    if (message.MessageType == MessageType.MESSAGE)
                    {
                        MessageNode sendMessage = MessageDirector.Instance().CreateNode();
                        sendMessage.MessageType = MessageType.MESSAGE;
                        sendMessage.Message = client.SocketClient.RemoteEndPoint + "-" + message.Message;
                        String2 json = MessageDirector.Instance().GetJsonFromNode(sendMessage);
                        SendBroadcast(MessageType.MESSAGE, json);
                        Console.WriteLine(sendMessage.Message);
                    }
                    else if (message.MessageType == MessageType.WORKTEMP)
                    {
                        FileInfo info = new FileInfo(Program.WORK_PATH + Path.DirectorySeparatorChar + message.WorkTitle);
                        String2 buffer = new String2(message.Message, Encoding.UTF8);
                        using (FileStream stream = new FileStream(info.FullName, FileMode.Create, FileAccess.Write))
                        {
                            stream.Write(buffer.ToBytes(), 0, buffer.Length);
                        }
                        SendWorkList(client, WorkType.WorkListNotice);
                    }
                    else if (message.MessageType == MessageType.WORKNOTICE)
                    {
                        String buffer = message.Message;
                        SendWorkTemp(client, buffer.Trim(), buffer.Trim());
                    }
                }
                if (opcode == (int)Opcode.BINARY)
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
                        return;
                    }
                    if (type == (byte)FileMessageType.FileWrite)
                    {
                        if (!file.Open)
                        {
                            //logger.Error("It is being have downloading.but because what file's connection is closed.");
                            file.Init();
                            return;
                        }
                        String2 binary = data.SubString(1, data.Length - 1);
                        file.StreamBuffer.Write(binary.ToBytes(), 0, binary.Length);
                        file.Peek += binary.Length;
                        //logger.Info(file.Peek);
                        if (file.Peek >= file.Length)
                        {
                            file.Complete();
                            client.Send((int)Opcode.BINARY, new String2("File upload Success!!", Encoding.UTF8));
                        }
                        return;
                    }
                    if (type == (byte)FileMessageType.FileSearch || type == (byte)FileMessageType.FileListNotice)
                    {
                        SendFileList(client, (FileMessageType)type);
                        return;
                    }
                    if (type == (byte)WorkType.WorkSearch || type == (byte)WorkType.WorkListNotice)
                    {
                        SendWorkList(client, (WorkType)type);
                    }
                    //logger.Error("FileMessage type is wrong.");
                    file.Init();
                }
            };
        }

        public void SendBroadcast(MessageType messageType, String2 data)
        {
            foreach (IWorkSocketClient client in WorkSocketFactory.GetWorkSocketServer().GetSocketList())
            {
                client.Send((int)messageType, data);
            }
        }

        private SocketController(IWorkSocketClient webclient)
        {
            this.webclient = webclient;
        }

        private void SendWorkTemp(IWorkSocketClient client, String file, String title)
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
            client.Send((int)Opcode.BINARY, message);
        }
        private void SendWorkList(IWorkSocketClient client, WorkType type)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.WORKLIST);
            DirectoryInfo info = new DirectoryInfo(Program.WORK_PATH);
            FileInfo[] files = info.GetFiles();
            builder.SetFileList(from f in info.GetFiles() where !String.Equals(f.Name, "default") select f.Name);
            String2 message = builder.Build();
            if (type == WorkType.WorkListNotice)
            {
                foreach (IWorkSocketClient c in WorkSocketFactory.GetWorkSocketServer().GetSocketList())
                {
                    c.Send((int)Opcode.BINARY, message);
                }
            }
            else if (type == WorkType.WorkSearch)
            {
                client.Send((int)Opcode.BINARY, message);
            }
        }
        private void SendFileList(IWorkSocketClient client, FileMessageType type)
        {
            WebSocketMessageBuilder builder = WebSocketMessageBuilder.GetMessage(MessageType.FILELIST);
            DirectoryInfo info = new DirectoryInfo(Program.FILE_STORE_PATH);
            FileInfo[] files = info.GetFiles();
            builder.SetFileList(from f in info.GetFiles() select f.Name);
            String2 message = builder.Build();
            if (type == FileMessageType.FileSearch)
            {
                client.Send((int)Opcode.BINARY, message);
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
