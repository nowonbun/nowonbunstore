using System;
using System.Text;
namespace WorkServer
{
    static class Program
    {
        public static String FILE_STORE_PATH = "d:\\work\\file\\";
        public static String WEB_STORE_PATH = "d:\\work\\web";
        public static void Main(String[] args)
        {
            Server server = new Server(80);
            server.Acception += (client) =>
            {
                try
                {
                    client.GetStream().ReadTimeout = 500;
                    HandShake header = client.Receive();
                    Console.WriteLine(header);
                    String2 type = header.Get("Connection");
                    if (type == null)
                    {
                        client.Dispose();
                    }
                    WorkServer sock = null;
                    if (type.ToUpper().Equals("KEEP-ALIVE"))
                    {
                        sock = new WebServer(client);
                    }
                    else if (type.ToUpper().Equals("UPGRADE"))
                    {
                        client.GetStream().ReadTimeout = 86400000;
                        sock = new WebSocketServer(client);
                    }
                    if (sock == null)
                    {
                        client.Dispose();
                        return;
                    }
                    if (sock.Initialize(header))
                    {
                        sock.Run();
                    }
                    else
                    {
                        client.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    client.Dispose();
                }
            };
            server.ServerStart();
            Console.ReadLine();
        }
    }
}
