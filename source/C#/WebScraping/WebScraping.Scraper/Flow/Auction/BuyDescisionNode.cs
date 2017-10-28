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
        public Decimal DeliveryFee { get; set; }
        public Decimal OrderAmnt { get; set; }
        public Decimal SttlExpectedAmnt { get; set; }
        public Decimal SellPrice { get; set; }

        public override string ToString()
        {
            return String.Format("YearMonth:{0}, DeliveryFee:{1}, OrderAmnt:{2}, SttlExpectedAmnt:{3}, SellPrice:{4}",
                YearMonth.ToString("yyyy-MM"), DeliveryFee, OrderAmnt, SttlExpectedAmnt, SellPrice);
        }
    }
}
