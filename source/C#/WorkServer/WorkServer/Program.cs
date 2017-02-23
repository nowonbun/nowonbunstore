using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using log4net;
using log4net.Appender;

namespace WorkServer
{
    class Program
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(Program));
        public static String FILE_STORE_PATH;
        public static String WEB_STORE_PATH;
        public static String CONFIG_PATH;

        public Program()
        {
            InitPath();
            InitLog();
            logger.Info("The server make to start it.");
            SocketServer();
        }
        public void InitPath()
        {
            FILE_STORE_PATH = GetExecutablePath() + Path.DirectorySeparatorChar + Define.FILE_STORE_PATH + Path.DirectorySeparatorChar;
            WEB_STORE_PATH = GetExecutablePath() + Path.DirectorySeparatorChar + Define.WEB_STORE_PATH;
            CONFIG_PATH = GetExecutablePath() + Path.DirectorySeparatorChar + Define.CONFIG_PATH;
        }
        public static String GetExecutablePath()
        {
            return Application.StartupPath;
        }
        public void InitLog()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(CONFIG_PATH + Path.DirectorySeparatorChar + Define.LOG4NET));
        }
        public void SocketServer()
        {
            Server server = new Server(80);
            server.Acception += (client) =>
            {
                if (!Swiching(client))
                {
                    client.Dispose();
                }
            };
            server.ServerStart();
        }
        private bool Swiching(Client client)
        {
            try
            {
                client.GetStream().ReadTimeout = 500;
                HandShake header = client.Receive();
                logger.Debug(header);
                String2 type = header.Get(Define.PROTOCOL_CONNECTION);
                if (type == null)
                {
                    return false;
                }
                WorkServer sock = null;
                if (type.Equals(Define.KEEP_ALIVE))
                {
                    sock = new WebServer(client);
                }
                else if (type.Equals(Define.UPGRADE))
                {
                    client.GetStream().ReadTimeout = 86400000;
                    sock = new WebSocketServer(client);
                }
                if (sock != null && sock.Initialize(header))
                {
                    sock.Run();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }
        public static void Main(String[] args)
        {
            new Program();
            Console.ReadLine();
        }
    }
}
