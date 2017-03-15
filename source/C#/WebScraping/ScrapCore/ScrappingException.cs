using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapCore
{
    class ScrappingException : Exception
    {
        public ScrappingException(string message)
            : base(message)
        {
            
        }
    }
}
