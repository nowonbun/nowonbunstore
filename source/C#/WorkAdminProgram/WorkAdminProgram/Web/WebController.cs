using System;
using WorkServer;
using System.IO;

namespace WorkAdminProgram
{
    class WebController
    {
        public static WebController NewInstance(IWorkWebClient webclient)
        {
            return new WebController(webclient);
        }
        private IWorkWebClient webclient;
        private WebController(IWorkWebClient webclient)
        {
            this.webclient = webclient;
            String path = webclient.Path;
            if (object.Equals(path, Define.DOWNROAD_PATH))
            {
                String filepath = Program.FILE_STORE_PATH + "\\" + webclient.Parameter;
                FileInfo info = new FileInfo(filepath);
                if (info.Exists)
                {
                    webclient.Run(ResponeCode.CODE200, new FileInfo(filepath), true);
                }
                else
                {
                    webclient.Run(ResponeCode.CODE404, null, false);
                }
            }
            else
            {
                String webpath;
                if (object.Equals("/", webclient.Path))
                {
                    webpath = Program.WEB_STORE_PATH + "/index.html";
                }
                else
                {
                    webpath = Program.WEB_STORE_PATH + webclient.Path;
                }
                FileInfo info = new FileInfo(webpath);
                if (info.Exists)
                {
                    webclient.Run(ResponeCode.CODE200, new FileInfo(webpath), false);
                }
                else
                {
                    webclient.Run(ResponeCode.CODE404, null, false);
                }
            }
        }
    }
}
