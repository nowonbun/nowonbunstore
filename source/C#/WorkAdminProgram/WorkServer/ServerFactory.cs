using System;

namespace WorkServer
{
    public class ServerFactory
    {
        private static Server singleton = null;
        public static Server CreateServer(int port, int timeout)
        {
            if (singleton != null)
            {
                throw new Exception("It implement the server already.");
            }
            singleton = new ServerImpl(port, timeout);
            singleton.ServerStart();
            singleton.Timeout = timeout;
            return singleton;
        }
        public static Server GetServer()
        {
            if (singleton == null)
            {
                throw new NotImplementedException();
            }
            return singleton;
        }
    }
}
