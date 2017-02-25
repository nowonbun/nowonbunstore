using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;

namespace WorkServer
{
    class WebServer : WorkServer
    {
        private static ILog logger = LogManager.GetLogger(typeof(WebServer));
        private String path = "";
        public WebServer(Client client) : base(client) 
        {
            client.SetTimeout(1000);
        }

        public override bool Initialize(HandShake header)
        {
            try
            {
                String2[] buffer = header.Header.Split(" ");
                if (buffer.Length != 3)
                {
                    return false;
                }
                this.path = buffer[1].ToString().Trim();
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
            if (!String.IsNullOrEmpty(path))
            {
                if (path.IndexOf("/download") != -1)
                {
                    if (!FileUpload())
                    {
                        WebError();
                    }
                }
                else if (!WebUpload())
                {
                    WebError();
                }
            }
            ClientSocket.Dispose();
        }
        private void WebError()
        {
            try
            {
                String2 temp = new String2(Encoding.UTF8);
                temp += "HTTP/1.1 404 NG" + String2.CRLF;
                temp += "Content-Type: text/html" + String2.CRLF;
                temp += "Keep-Alive: timeout=15, max=93" + String2.CRLF;
                temp += "Connection: Keep-Alive" + String2.CRLF + String2.CRLF;
                //logger.Debug(temp);
                ClientSocket.Send(temp);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return;
            }
        }
        private bool WebUpload()
        {
            try
            {
                if (string.Equals(Define.WEB_SEPARATOR, path))
                {
                    path = Path.DirectorySeparatorChar + ConfigReader.GetIni(Define.WEB_SESSION, Define.WEB_INDEX_SESSION);
                }
                path = Program.WEB_STORE_PATH + path;
                FileInfo fileinfo = new FileInfo(path);
                if (!fileinfo.Exists)
                {
                    return false;
                }
                using (FileStream stream = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    String2 temp = new String2(Encoding.UTF8);
                    temp += "HTTP/1.1 200 OK" + String2.CRLF;
                    temp += "Content-Type: text/html" + String2.CRLF;
                    temp += "Keep-Alive: timeout=15, max=93" + String2.CRLF;
                    temp += "Connection: Keep-Alive" + String2.CRLF + String2.CRLF;
                    //logger.Debug(temp);
                    ClientSocket.Send(temp);
                    String2 body = String2.ReadStream(stream, Encoding.Default, (int)fileinfo.Length);
                    //logger.Debug(body);
                    ClientSocket.Send(body);
                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }
        private bool FileUpload()
        {
            try
            {
                String filename = path.Substring(path.IndexOf("?") + 1);
                filename = filename.Trim();
                filename = filename.Replace("%20", " ");
                String file = Program.FILE_STORE_PATH + filename;
                FileInfo fileinfo = new FileInfo(file);
                if (!fileinfo.Exists)
                {
                    return false;
                }
                using (FileStream stream = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    String2 temp = new String2(Encoding.UTF8);
                    temp += "HTTP/1.1 200 OK" + String2.CRLF;
                    temp += "Content-Type: multipart/formed-data" + String2.CRLF;
                    temp += "Content-Disposition: attachment; filename=" + fileinfo.Name + String2.CRLF;
                    temp += "Length: " + fileinfo.Length + String2.CRLF;
                    temp += "Keep-Alive: timeout=15, max=93" + String2.CRLF;
                    temp += "Connection: Keep-Alive" + String2.CRLF + String2.CRLF;
                    //logger.Debug(temp);
                    ClientSocket.Send(temp);
                    String2 body = String2.ReadStream(stream, Encoding.Default, (int)fileinfo.Length);
                    //logger.Debug(body);
                    ClientSocket.Send(body);
                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }
    }
}
