using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrappingCore
{
    class ScrappingException : Exception
    {
        public ScrappingException(string message)
            : base(message)
        {
            
        }
    }
}
