using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkServer;

namespace WorkServer
{
    public class WorkSocketFactory
    {
        private static WorkSocketFactory singleton = null;

        private IList<IWorkSocketClient> clientlist = new List<IWorkSocketClient>();

        public static WorkSocketFactory GetWorkSocketServer()
        {
            if (singleton == null)
            {
                singleton = new WorkSocketFactory();
            }
            return singleton;
        }

        public IWorkSocketClient CreateSocketClient(IClient client)
        {
            IWorkSocketClient server = new WorkSocketImpl(client);
            clientlist.Add(server);
            return server;
        }

        public void RemoveSocketClient(IWorkSocketClient client)
        {
            clientlist.Remove(client);
        }

        public IList<IWorkSocketClient> GetSocketList()
        {
            return clientlist.ToList();
        }
    }
}
