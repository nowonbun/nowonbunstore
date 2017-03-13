using System;
using System.IO;

namespace WorkWebServer
{
    partial class WorkWebServerImpl : WorkWeb
    {
        private bool WebUpload(String path)
        {
            try
            {
                if (string.Equals(Define.WEB_SEPARATOR, path))
                {
                    //path = Path.DirectorySeparatorChar + ConfigReader.GetIni(Define.WEB_SESSION, Define.WEB_INDEX_SESSION);
                    path = Path.DirectorySeparatorChar + "index.html";
                }
                path = webpath + path;
                FileInfo fileinfo = new FileInfo(path);
                if (!fileinfo.Exists)
                {
                    return false;
                }

                byte[] data = GetFile(fileinfo.FullName, (int)fileinfo.Length);
                HeaderOption builder = HeaderOption.Create();
                builder.AddOption("Content-Type", "text/html");
                client.Send(Build(200, builder).ToBytes());
                client.Send(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
