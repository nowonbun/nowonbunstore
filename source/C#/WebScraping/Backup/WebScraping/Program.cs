using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            String asdf = "TEST[1123]";
            Console.WriteLine(GetSplitKey(asdf));
            Console.ReadKey();

        }
        private static int GetSplitKey(String key)
        {
            int spos = key.IndexOf("[")+1;
            int epos = key.LastIndexOf("]");
            if (spos < 0 || epos < 0)
            {
                throw new Exception("xPath 가 맞지 않습니다.");
            }
            try
            {
                return Convert.ToInt32(key.Substring(spos, epos - spos));
            }
            catch (FormatException)
            {
                throw new FormatException("xPath 가 맞지 않습니다.");
            }
        }
    }
}
