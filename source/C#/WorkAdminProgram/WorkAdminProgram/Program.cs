using System;
using System.Windows.Forms;
using System.IO;
using log4net;
using System.Threading;
using WorkWebServer;
using WorkSocketServer;
using System.Text;

namespace WorkServer
{
    class Program
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(Program));
        public static String FILE_STORE_PATH;
        public static String WEB_STORE_PATH;
        public static String CONFIG_PATH;
        public static String WORK_PATH;

        public Program()
        {
            String2.DefaultEncoding = Encoding.UTF8;
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
            WORK_PATH = GetExecutablePath() + Path.DirectorySeparatorChar + Define.WORK_PATH;
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
            Server server = ServerFactory.CreateServer(80, 500);
            WorkWebFactory.CreateWorkWebServer(WEB_STORE_PATH, FILE_STORE_PATH);
            WorkSocketFactory.CreateWorkSocketServer();
            server.Acception += (client) =>
            {
                try
                {
                    HandShake header = client.Receive();
                    logger.Debug(header);
                    String2 type = header.Get(Define.PROTOCOL_CONNECTION);
                    if (type == null)
                    {
                        throw new Exception("header errer");
                    }
                    type = type.ToUpper();
                    if (type.Equals(Define.KEEP_ALIVE))
                    {
                        WorkWebFactory.GetWorkWebServer().RunWebServer(client, header.Header);
                    }
                    else if (type.Equals(Define.UPGRADE))
                    {
                        FileNode file = FileNode.GetFileNode();
                        WorkSocketFactory.GetWorkSocketServer().CreateSocketServer(
                            client,
                            header.Get("Sec-WebSocket-Key"),
                            (socket, opcode, data) =>
                            {
                                WebSocketServer.Run(socket, opcode, data);
                            });
                    }
                    else
                    {
                        logger.Error(type);
                        throw new Exception("header errer");
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    client.Close();
                }
            };
            server.ServerStart();
        }






        public static void Main(String[] args)
        {
            new Thread(_ =>
            {
                new Program();
                while (true)
                {
                    String command = Console.ReadLine();
                    if (String.Equals("EXIT", command.ToUpper()))
                    {
                        return;
                    }
                }
            }).Start();
        }
    }
}
