using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkServer
{
    abstract class WorkServer
    {
        private Client client = null;
        public WorkServer(Client client)
        {
            this.client = client;
        }
        protected Client ClientSocket
        {
            get { return this.client; }
        }
        public abstract bool Initialize(HandShake header);
        public abstract void Run();
    }
}
