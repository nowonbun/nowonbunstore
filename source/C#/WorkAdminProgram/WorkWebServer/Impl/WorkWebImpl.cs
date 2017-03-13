using System;
using System.Text;
using System.IO;
using WorkServer;
using System.Collections.Generic;

namespace WorkWebServer
{
    partial class WorkWebServerImpl : WorkWeb
    {
        private Client client;
        private String path = "";
        private String webpath;
        private String filepath;
        private IDictionary<String, String> Option = new Dictionary<String, String>();

        public WorkWebServerImpl(Client client, String webpath, String filepath)
        {
            this.client = client;
            this.webpath = webpath;
            this.filepath = filepath;
        }

        public void Initialize(String2 header)
        {
            try
            {
                String2[] buffer = header.Split(" ");
                if (buffer.Length != 3)
                {
                    throw new FormatException();
                }
                this.path = buffer[1].ToString().Trim();
            }
            catch (Exception e)
            {
                throw new FormatException(null, e);
            }
        }

        public void Run()
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (path.IndexOf(Define.DOWNROAD_PATH) != -1)
                {
                    if (!FileUpload())
                    {
                        WebError();
                    }
                }
                else if (!WebUpload(path))
                {
                    WebError();
                }
            }
            client.Close();
        }

        private void WebError()
        {
            try
            {
                HeaderOption builder = HeaderOption.Create();
                builder.AddOption("Content-Type", "text/html");
                client.Send(Build(404, builder).ToBytes());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private byte[] GetFile(String filepath, int filesize)
        {
            byte[] data = new byte[filesize];
            using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(data, 0, filesize);
            }
            return data;
        }
    }
}
