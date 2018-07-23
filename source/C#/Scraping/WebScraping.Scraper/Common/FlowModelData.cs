using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using WebScraping.Scraper.Common;
using WebScraping.Scraper.Impl;
using System.IO;

namespace WebScraping.Scraper.Common
{
    public class FlowModelData
    {
        Dictionary<String, dynamic> datapack = new Dictionary<string, dynamic>();
        public Boolean IsNextScrap { get; set; }
        public String NextUrl { get; set; }
        public Boolean IsSkipAction { get; set; }
        public Action<FlowModelData> CallBack { get; set; }
        public GeckoDocument Document { get; set; }
        public Uri Uri { get; set; }

        public FileInfo File { get; set; }
        public Dictionary<String, dynamic> DataPack
        {
            get
            {
                return datapack;
            }
        }

    }
}
