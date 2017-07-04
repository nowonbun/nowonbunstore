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
            WebServer.WebServer.Start();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
