using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrappingMapParser
{
    static class LOG
    {
        public static void Debug(object message){
            Console.WriteLine(message.ToString());
        }
        public static void Error(object message)
        {
            Console.WriteLine(message.ToString());
        }
        public static void Info(object message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}
