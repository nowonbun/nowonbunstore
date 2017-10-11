using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Flow.Auction
{
    class BuyDescisionNode
    {
        public DateTime YearMonth { get; set; }
        public Decimal BuyDecisionDate { get; set; }
        public Decimal DeliveryFee { get; set; }
        public Decimal OrderAmnt { get; set; }
        public Decimal SttlExpectedAmnt { get; set; }
        public Decimal SellPrice { get; set; }
    }
}
