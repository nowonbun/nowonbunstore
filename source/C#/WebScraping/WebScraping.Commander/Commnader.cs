using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Commander
{
    public class Commnader
    {
        private static Commnader singleton = null;
        private Commnader() { }
        public static void Start()
        {
            if (singleton != null)
            {
                throw new Exception("already");
            }
            singleton = new Commnader();
            singleton.Run();
        }

        public static void End()
        {
            if (singleton == null)
            {
                throw new Exception("not ready");
            }
            singleton.Close();
        }
        private void Run()
        {
            WebServer.WebServer.Start(19999, (e) =>
            {
                String cmd = e.Header[1];
                if ("/ControllView".Equals(cmd))
                {
                    Console.WriteLine(cmd);
                    e.Response = "Hello world";
                }
            });
        }

        private void Close()
        {

        }
    }
}
