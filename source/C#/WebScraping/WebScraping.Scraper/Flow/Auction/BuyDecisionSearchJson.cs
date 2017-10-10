using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Flow.Auction
{
    class BuyDecisionSearchJson
    {
        public String Total { get; set; }
        public List<BuyDecisionSearchDataJson> Data { get; set; }
        public String Success { get; set; }
        public String Message { get; set; }
    }
}
