using System;
using WorkServer;

namespace WorkServer
{
    public class WorkWebFactory
    {
        private static WorkWebFactory singleton = null;

        public static WorkWebFactory GetWorkWebServer()
        {
            if(singleton == null)
            {
                singleton = new WorkWebFactory();
            }
            return singleton;
        }

        private WorkWebFactory() { }

        public IWorkWebClient CreateWebClient(IClient client)
        {
            return new WorkWebClientImpl(client);
        }
    }
}
