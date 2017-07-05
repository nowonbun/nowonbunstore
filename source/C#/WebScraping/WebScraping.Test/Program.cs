using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.WebServer;

namespace WebScraping.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer.WebServer.Start(80, (e) =>
            {
                String cmd = e.Header[1];
                Console.WriteLine(cmd);
            });

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
