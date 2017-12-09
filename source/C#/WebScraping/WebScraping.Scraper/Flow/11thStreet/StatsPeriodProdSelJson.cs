using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Flow._11thStreet
{
    class StatsPeriodProdSelNode
    {
        public int ordCnCnt { get; set; }
        public int visitCnt { get; set; }
        public int likeCnt { get; set; }
        public int selAmt { get; set; }
        public int rfndAmt { get; set; }
        public int dispPrdCnt { get; set; }
        public int selCntPre { get; set; }
        public String totzDate { get; set; }
        public int ordCnt { get; set; }
        public int stlAmt { get; set; }
        public int ordAmt { get; set; }
        public int selCnt { get; set; }
        public int selAmtPre { get; set; }
        public int mall99 { get; set; }
        public int rfndCnt { get; set; }
        public int stlCnt { get; set; }
        public int ordCnAmt { get; set; }
        public int bcktCnt { get; set; }
        public int ordRt { get; set; }
        public int mall03 { get; set; }
        public int mall01 { get; set; }
    }
    class StatsPeriodProdSelJson
    {
        public String Count { get; set; }
        public List<StatsPeriodProdSelNode> statsPeriodProdSelSummary { get; set; }
        public List<StatsPeriodProdSelNode> statsPeriodProdSelList { get; set; }
    }
}
