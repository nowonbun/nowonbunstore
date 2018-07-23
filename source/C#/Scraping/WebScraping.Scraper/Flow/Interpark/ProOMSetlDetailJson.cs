using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Flow.Interpark
{
    class ProOMSetlDetailBsJson
    {
        public int addMileageAmt { get; set; }
        public int addSmoneyAmt { get; set; }
        public int amt1 { get; set; }
        public int amt2 { get; set; }
        public int amt3 { get; set; }
        public int amt4 { get; set; }
        public int amt5 { get; set; }
        public string billDt { get; set; }
        public string billIssueDt { get; set; }
        public string billNo { get; set; }
        public string billPblctDts { get; set; }
        public string billPblctStat { get; set; }
        public string billPblctStatNm { get; set; }
        public string billTp { get; set; }
        public string buyCfmDts { get; set; }
        public int clmSeq { get; set; }
        public string comTp { get; set; }
        public string comTpNm { get; set; }
        public int compAmt { get; set; }
        public string compAmtChargeTpNm { get; set; }
        public string compMthdTpNm { get; set; }
        public string compRsnTpNm { get; set; }
        public string costRespTp { get; set; }
        public int curSetlConfAmt { get; set; }
        public int custCompAmt { get; set; }
        public int deductAmt { get; set; }
        public int delvAmt { get; set; }
        public int delvRtnAmt { get; set; }
        public string docCertStatNm { get; set; }
        public int entrDcCouponAmt { get; set; }
        public string entrNm { get; set; }
        public int entrNo { get; set; }
        public int entrOnusAmt { get; set; }
        public string entrStatNm { get; set; }
        public int etcAmt { get; set; }
        public int expnAmt { get; set; }
        public string expnClCd { get; set; }
        public int fee04_21 { get; set; }
        public int fee21 { get; set; }
        public int fee22 { get; set; }
        public int fee23 { get; set; }
        public int fee24 { get; set; }
        public int fee24_22 { get; set; }
        public int fee28 { get; set; }
        public int fee29 { get; set; }
        public int fee34 { get; set; }
        public int fee35 { get; set; }
        public int feeAmt { get; set; }
        public int feeMinusAmt { get; set; }
        public int feeRevConfNo { get; set; }
        public string gub { get; set; }
        public int inDcCouponAmt { get; set; }
        public int inDcCouponOldAmt { get; set; }
        public int inDcCrmCouponAmt { get; set; }
        public int inpkOnusAmt { get; set; }
        public int ipSaveCouponAmt { get; set; }
        public int ipSaveCrmCouponAmt { get; set; }
        public int ipointPayAmt { get; set; }
        public int ipointSaveAmt { get; set; }
        public string item { get; set; }
        public string memId { get; set; }
        public int mileageAmt { get; set; }
        public int nointrstFeeAmt { get; set; }
        public int notPayAmt { get; set; }
        public int notPrevPayAmt { get; set; }
        public int oldSaleUnitcost { get; set; }
        public int onInterestFee { get; set; }
        public string optFnm { get; set; }
        public string ordClmNo { get; set; }
        public string ordNm { get; set; }
        public int ordSeq { get; set; }
        public string ordclmNo { get; set; }
        public string ordpaymentNm { get; set; }
        public string oriOrdClmNo { get; set; }
        public string parentPrdNm { get; set; }
        public int parentPrdNo { get; set; }
        public string payCnclTp { get; set; }
        public string payConfDt { get; set; }
        public int payNo { get; set; }
        public int payTgtAmt { get; set; }
        public string payTpNm { get; set; }
        public int prdCorrAmt { get; set; }
        public string prdNm { get; set; }
        public int prdNo { get; set; }
        public int prdSaleAmt { get; set; }
        public int preUseUnitcost { get; set; }
        public int qty { get; set; }
        public string rcvrNm { get; set; }
        public int realPayAmt { get; set; }
        public int realPrdSaleAmt { get; set; }
        public int realSaleUnitcost { get; set; }
        public string regDts { get; set; }
        public string revDt { get; set; }
        public string revEndDt { get; set; }
        public string revLogTp { get; set; }
        public string revStrDt { get; set; }
        public string rmk { get; set; }
        public int rsvAmt { get; set; }
        public int rtnprdHdelvAmt { get; set; }
        public int saleFee { get; set; }
        public int saleFeeAmt { get; set; }
        public int saleFeeRt { get; set; }
        public int saleQty { get; set; }
        public string setPrdNm { get; set; }
        public int setPrdNo { get; set; }
        public int setlConfAmt { get; set; }
        public string setlConfDt { get; set; }
        public string setlDt { get; set; }
        public string setlEndDt { get; set; }
        public string setlEntrTp { get; set; }
        public string setlEntrTpNm { get; set; }
        public string setlLogDt { get; set; }
        public string setlLogTpNm { get; set; }
        public string setlModTpNm { get; set; }
        public string setlReprdhdelvamtRsn { get; set; }
        public string setlRespTp { get; set; }
        public string setlStrDt { get; set; }
        public int smoneyAmt { get; set; }
        public string smoneyPayYn { get; set; }
        public int supplyAmt { get; set; }
        public int supplyCtrtSeq { get; set; }
        public int taxAmt { get; set; }
        public string taxTp { get; set; }
        public int totAmt { get; set; }
        public int total { get; set; }
        public int uncollectFixAmt { get; set; }
    }
    class ProOMSetlDetailJson
    {
        public List<ProOMSetlDetailBsJson> bs { get; set; }
        public int total { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
        public int records { get; set; }
    }
}
