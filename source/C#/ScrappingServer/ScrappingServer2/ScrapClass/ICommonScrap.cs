using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrappingCore;


namespace ScrappingServer.ScrapClass
{
    public interface ICommonScrap : IScrappingProcess
    {
        void SetHandler(Action<ScrapState> eventhandler);
    }
}
