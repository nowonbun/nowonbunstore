using System;
using System.IO;

namespace WorkWebServer
{
    partial class WorkWebServerImpl : WorkWeb
    {
        private bool FileUpload()
        {
            try
            {
                String filename = path.Substring(path.IndexOf("?") + 1);
                filename = filename.Trim();
                filename = filename.Replace("%20", " ");
                //String file = Program.FILE_STORE_PATH + filename;
                String file = this.filepath + filename;
                FileInfo fileinfo = new FileInfo(file);
                if (!fileinfo.Exists)
                {
                    return false;
                }

                byte[] data = GetFile(fileinfo.FullName, (int)fileinfo.Length);

                HeaderOption builder = HeaderOption.Create();
                builder.AddOption("Content-Type", "multipart/formed-data");
                builder.AddOption("Content-Disposition", "attachment; filename=" + fileinfo.Name);
                builder.AddOption("Length", fileinfo.Length.ToString());
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
