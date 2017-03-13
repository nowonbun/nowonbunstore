using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWebServer
{
    public interface WorkWeb
    {
        void Initialize(String2 header);

        void Run();
    }
}
