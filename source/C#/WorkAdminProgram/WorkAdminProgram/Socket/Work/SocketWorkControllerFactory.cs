using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkAdminProgram
{
    public class SocketWorkControllerFactory
    {
        private static SocketWorkControllerFactory singleton = null;
        public static SocketWorkControllerFactory Instance()
        {
            if(singleton == null)
            {
                singleton = new SocketWorkControllerFactory();
            }
            return singleton;
        }
        private SocketWorkControllerFactory() { }

        public ISocketWorkController CreateMessageController()
        {
            return new MessageController();
        }
    }
}
