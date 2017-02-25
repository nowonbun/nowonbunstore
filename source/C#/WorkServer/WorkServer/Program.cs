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
                try
                {
                    HandShake header = client.Receive();
                    logger.Debug(header);
                    WorkServer sock = header.ServerBuilder(client);
                    if (sock != null && sock.Initialize(header))
                    {
                        sock.Run();
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            };
            server.ServerStart();
        }

        public static void Main(String[] args)
        {
            new Program();
            Console.ReadLine();
        }
    }
}
