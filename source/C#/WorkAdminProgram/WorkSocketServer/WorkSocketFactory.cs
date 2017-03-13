using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkServer;

namespace WorkSocketServer
{
    public class WorkSocketFactory
    {
        private static WorkSocketFactory singleton = null;
        private IList<WorkSocket> clientlist = new List<WorkSocket>();
        public static WorkSocketFactory CreateWorkSocketServer()
        {
            if (singleton != null)
            {
                throw new NotImplementedException();
            }
            singleton = new WorkSocketFactory();
            return singleton;
        }
        public static WorkSocketFactory GetWorkSocketServer()
        {
            if (singleton == null)
            {
                throw new NotImplementedException();
            }
            return singleton;
        }

        public WorkSocket CreateSocketServer(Client client,
                                             String2 key,
                                             Action<WorkSocket, byte, String2> receive)
        {
            WorkSocket server = new WorkSocketImpl(client, receive);
            server.Initialize(key);
            clientlist.Add(server);
            return server;
        }

        public void RemoveSocketServer(WorkSocket client)
        {
            clientlist.Remove(client);
        }
    }
}
