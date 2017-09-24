using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Library.Log;
using WebScraping.Library.Log;

namespace WebScraping.WebServer.Other
{
    class WebServerException : System.Exception
    {
        public WebServerException(String message, Exception e) :
            base(message, e)
        {
            LoggerBuilder.Init().Set(this.GetType()).Info("Message - " + message);
            LoggerBuilder.Init().Set(this.GetType()).Info("Exception - " + e);
        }
        public WebServerException(String message)
            : base(message)
        {
            LoggerBuilder.Init().Set(this.GetType()).Info("Message - " + message);
        }
    }
}
