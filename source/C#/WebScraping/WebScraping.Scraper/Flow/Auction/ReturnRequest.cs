using System;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class ReturnRequest
    {
        [ExcelHeader("구매자명*", 0)]
        private String buyerName;

        public String BuyerName
        {
            get { return this.buyerName; }
        }

        [ExcelHeader("수령인명*", 0)]
        private String getterName;

        public String GetterName
        {
            get { return this.getterName; }
        }

        [ExcelHeader("아이디*", 0)]
        private String id;

        public String Id
        {
            get { return this.id; }
        }

        [ExcelHeader("주문번호*", 0)]
        private String orderNumber;

        public String OrderNumber
        {
            get { return this.orderNumber; }
        }

        [ExcelHeader("주문일자(결제확인전)*", 0)]
        private String orderDate;

        public String OrderDate
        {
            get { return this.orderDate; }
        }

        [ExcelHeader("반품신청일*", 0)]
        private String returnApp;

        public String ReturnApp
        {
            get { return this.returnApp; }
        }

        [ExcelHeader("수거완료일*", 0)]
        private String returnComplete;

        public String ReturnComplete
        {
            get { return this.returnComplete; }
        }

        [ExcelHeader("환불보류일*", 0)]
        private String refundHoldDate;

        public String RefundHoldDate
        {
            get { return this.refundHoldDate; }
        }

        [ExcelHeader("환불승인일*", 0)]
        private String refundAcceptionDate;

        public String RefundAcceptionDate
        {
            get { return this.refundAcceptionDate; }
        }

        [ExcelHeader("상품번호*", 0)]
        private String productNumber;

        public String ProductNumber
        {
            get { return this.productNumber; }
        }

        [ExcelHeader("상품명*", 0)]
        private String productName;

        public String ProductName
        {
            get { return this.productName; }
        }

        [ExcelHeader("판매금액*", 0)]
        private String sellPrice;

        public String SellPrice
        {
            get { return this.sellPrice; }
        }

        [ExcelHeader("수량*", 0)]
        private String amount;

        public String Amount
        {
            get { return this.amount; }
        }

        [ExcelHeader("필수선택*", 0)]
        private String requiredSelection;

        public String RequiredSelection
        {
            get { return this.requiredSelection; }
        }

        [ExcelHeader("추가구성*", 0)]
        private String addStructure;

        public String AddStructure
        {
            get { return this.addStructure; }
        }

        [ExcelHeader("배송번호*", 0)]
        private String deleveryNumber;

        public String DeleveryNumber
        {
            get { return this.deleveryNumber; }
        }

        [ExcelHeader("배송비*", 0)]
        private String deleveryType;

        public String DeleveryType
        {
            get { return this.deleveryType; }
        }

        [ExcelHeader("배송비금액", 0)]
        private String deleveryFee;

        public String DeleveryFee
        {
            get { return this.deleveryFee; }
        }

        [ExcelHeader("반품접수채널*", 0)]
        private String refundChannel;

        public String RefundChannel
        {
            get { return this.refundChannel; }
        }

        [ExcelHeader("반품사유*", 0)]
        private String refundComment;

        public String RefundComment
        {
            get { return this.refundComment; }
        }

        [ExcelHeader("상세사유*", 0)]
        private String detailComment;

        public String DetailComment
        {
            get { return this.detailComment; }
        }

        [ExcelHeader("상품상태*", 0)]
        private String returnStatus;

        public String ReturnStatus
        {
            get { return this.returnStatus; }
        }

        [ExcelHeader("최초배송비부담*", 0)]
        private String deleveryCharge;

        public String DeleveryCharge
        {
            get { return this.deleveryCharge; }
        }

        [ExcelHeader("반품수거택배사명*", 0)]
        private String returnDeleveryFirm;

        public String ReturnDeleveryFirm
        {
            get { return this.returnDeleveryFirm; }
        }

        [ExcelHeader("반품송장번호*", 0)]
        private String returnDeleveryNumber;

        public String ReturnDeleveryNumber
        {
            get { return this.returnDeleveryNumber; }
        }

        [ExcelHeader("반품배송비지불주체*", 0)]
        private String returnFeeType;

        public String ReturnFeeType
        {
            get { return this.returnFeeType; }
        }

        [ExcelHeader("반품배송비금액*", 0)]
        private String returnDeleveryFee;

        public String ReturnDeleveryFee
        {
            get { return this.returnDeleveryFee; }
        }

        [ExcelHeader("반품추가비금액*", 0)]
        private String returnAddtionalFee;

        public String ReturnAddtionalFee
        {
            get { return this.returnAddtionalFee; }
        }

        [ExcelHeader("반품추가비지불주체*", 0)]
        private String returnAddtionalFeeType;

        public String ReturnAddtionalFeeType
        {
            get { return this.returnAddtionalFeeType; }
        }

        [ExcelHeader("구매자ID", 0)]
        private String sellerId;

        public String SellerId
        {
            get { return this.sellerId; }
        }

        [ExcelHeader("구매자 휴대폰", 0)]
        private String sellerCellphone;

        public String SellerCellphone
        {
            get { return this.sellerCellphone; }
        }

        [ExcelHeader("구매자 전화번호", 0)]
        private String sellerPhone;

        public String SellerPhone
        {
            get { return this.sellerPhone; }
        }

        [ExcelHeader("수령인 휴대폰", 0)]
        private String getterCellPhone;

        public String GetterCellPhone
        {
            get { return this.getterCellPhone; }
        }

        [ExcelHeader("수령인 전화번호", 0)]
        private String getterPhone;

        public String GetterPhone
        {
            get { return this.getterPhone; }
        }

        [ExcelHeader("환불보류해제일*", 0)]
        private String returnHoldReleaseDate;

        public String ReturnHoldReleaseDate
        {
            get { return this.returnHoldReleaseDate; }
        }

        [ExcelHeader("원배송택배사명*", 0)]
        private String originDeleveryFirm;

        public String OriginDeleveryFirm
        {
            get { return this.originDeleveryFirm; }
        }

        [ExcelHeader("원배송송장번호*", 0)]
        private String originDeleveryNumber;

        public String OriginDeleveryNumber
        {
            get { return this.originDeleveryNumber; }
        }

        [ExcelHeader("환불보류사유*", 0)]
        private String returnHoldComment;

        public String ReturnHoldComment
        {
            get { return this.returnHoldComment; }
        }

        [ExcelHeader("장바구니번호(결제번호)*", 0)]
        private String shoppingCurtNumber;

        public String ShoppingCurtNumber
        {
            get { return this.shoppingCurtNumber; }
        }

        [ExcelHeader("판매자관리코드*", 0)]
        private String sellerCode;

        public String SellerCode
        {
            get { return this.sellerCode; }
        }

        [ExcelHeader("판매자상세관리코드*", 0)]
        private String sellerDetailCode;

        public String SellerDetailCode
        {
            get { return this.sellerDetailCode; }
        }
    }
}

