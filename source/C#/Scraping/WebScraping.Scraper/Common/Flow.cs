using System;

namespace WebScraping.Scraper.Common
{
    public class Flow
    {
        public String Key;
        public String Url;
        public Action<FlowModelData> Func;
        public String Next;
        public Action<FlowModelData> Callback;
    }
}
