using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Scraper.Flow._11thStreet
{
    class SellerRemittanceActionNode
    {
        public String feeAmt { get; set; }
        public String sellerDfrmGblDlvFee { get; set; }
        public String addCstCnt { get; set; }
        public String tourCnFeeCnt { get; set; }
        public String bookingFeeCnt { get; set; }
        public String sellerDfrmDstSalesUFeeCnt { get; set; }
        public String dscCupnCstCnt { get; set; }
        public String clmCst { get; set; }
        public String tourCnFee { get; set; }
        public String clmDlvCst { get; set; }
        public String cnFee { get; set; }
        public String totalCount { get; set; }
        public String sellerDfrmGblCnDlvCstCnt { get; set; }
        public String rtngdDlvCst { get; set; }
        public String optAmt { get; set; }
        public String statClfCd { get; set; }
        public String abrdCnDlvCstCnt { get; set; }
        public String prdSelPrc { get; set; }
        public String rtngdDlvCstCnt { get; set; }
        public String sellerDfrmGblCnDlvCst { get; set; }
        public String preStlDlvCst { get; set; }
        public String sellerDfrmMultiDscCst { get; set; }
        public String cnFeeCnt { get; set; }
        public String abrdCnDlvCst { get; set; }
        public String feeAmtCnt { get; set; }
        public String sellerDfrmGblDlvFeeCnt { get; set; }
        public String dscCupnCst { get; set; }
        public String clmCstCnt { get; set; }
        public String optAmtCnt { get; set; }
        public String sellerDfrmAppDlvAmt { get; set; }
        public String addCst { get; set; }
        public String sellerDfrmDstSalesUFee { get; set; }
        public String bookingFee { get; set; }
        public String prdSelPrcCnt { get; set; }
        public String clmDlvCstCnt { get; set; }
        public String preStlDlvCstCnt { get; set; }
        public String sellerDfrmMultiDscCstCnt { get; set; }
        public String sellerDfrmAppDlvCnt { get; set; }
    }
    class SellerRemittanceActionJson
    {
        public List<SellerRemittanceActionNode> list { get; set; }
    }
}
