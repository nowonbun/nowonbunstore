using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Library.Log;

namespace WebScraping.WebServer.Other
{
    class WebServerException : System.Exception
    {
        public WebServerException(String message)
            : base(message)
        {
            
        }
    }
}
