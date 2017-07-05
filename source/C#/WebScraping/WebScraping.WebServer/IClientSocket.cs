using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.WebServer
{
    public interface IClientSocket
    {
        String[] Header{ get; }
    }
}
