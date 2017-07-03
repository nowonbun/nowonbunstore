using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snipper;

namespace Snipper.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            RawSocket rs =  new RawSocket();
            rs.Start();

            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
    }
}
