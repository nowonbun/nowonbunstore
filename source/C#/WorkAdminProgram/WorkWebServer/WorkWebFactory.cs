using System;
using WorkServer;

namespace WorkWebServer
{
    public class WorkWebFactory
    {
        private String webpath;
        private String filepath;
        private static WorkWebFactory singleton = null;

        public static WorkWebFactory CreateWorkWebServer(String webpath, String filepath)
        {
            if (singleton != null)
            {
                throw new NotImplementedException();
            }
            singleton = new WorkWebFactory(webpath, filepath);
            return singleton;
        }

        public static WorkWebFactory GetWorkWebServer()
        {
            if(singleton == null)
            {
                throw new NotImplementedException();
            }
            return singleton;
        }

        public WorkWebFactory(String webpath,String filepath)
        {
            this.webpath = webpath;
            this.filepath = filepath;
        }

        public WorkWeb CreateWebServer(Client client)
        {
            WorkWeb server = new WorkWebServerImpl(client, webpath, filepath);
            return server;
        }

        public WorkWeb RunWebServer(Client client, String2 header)
        {
            WorkWeb server = new WorkWebServerImpl(client, webpath, filepath);
            server.Initialize(header);
            server.Run();
            return server;
        }
    }
}
