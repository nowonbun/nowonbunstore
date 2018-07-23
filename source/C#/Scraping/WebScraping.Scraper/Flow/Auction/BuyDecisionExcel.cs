using System;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class BuyDecisionExcel
    {
        [ExcelHeader("아이디", 0)]
        private String id = null;

        public String Id
        {
            get { return this.id; }
        }

        [ExcelHeader("구매결정일자", 1)]
        private String buyDate = null;

        public String BuyDate
        {
            get { return this.buyDate; }
        }

        [ExcelHeader("주문번호", 2)]
        private String orderNumber = null;

        public String OrderNumber
        {
            get { return this.orderNumber; }
        }

        [ExcelHeader("상품번호", 3)]
        private String productNumber = null;

        public String ProductNumber
        {
            get { return this.productNumber; }
        }

        [ExcelHeader("정산상태", 4)]
        private String state = null;

        public String State
        {
            get { return this.state; }
        }

        [ExcelHeader("판매금액", 5)]
        private String sellPrice = null;

        public Decimal SellPrice
        {
            get { return Decimal.Parse(this.sellPrice.Replace(",", "")); }
        }

        [ExcelHeader("판매단가", 6)]
        private String sellOriginPrice = null;

        private String SellOriginPrice
        {
            get { return this.sellOriginPrice; }
        }

        [ExcelHeader("구매자명", 7)]
        private String buyerName = null;

        public String BuyerName
        {
            get { return this.buyerName; }
        }

        [ExcelHeader("구매자ID", 8)]
        private String buyerId = null;

        public String BuyerId
        {
            get { return this.buyerId; }
        }

        [ExcelHeader("상품명", 9)]
        private String productName = null;

        public String ProductName
        {
            get { return this.productName; }
        }

        [ExcelHeader("수량", 10)]
        private String productQuantity = null;

        public Decimal ProductQuanity
        {
            get { return Decimal.Parse(this.productQuantity.Replace(",", "")); }
        }

        [ExcelHeader("주문옵션", 11)]
        private String orderOption = null;

        public String OrderOption
        {
            get { return this.orderOption; }
        }

        [ExcelHeader("추가구성", 12)]
        private String addOption = null;

        public String AddOption
        {
            get { return this.addOption; }
        }

        [ExcelHeader("사은품", 13)]
        private String gifts = null;

        public String Gifts
        {
            get { return gifts; }
        }

        [ExcelHeader("수령인명", 14)]
        private String getterName = null;

        public String GetterName
        {
            get { return this.getterName; }
        }

        [ExcelHeader("수령인 휴대폰", 15)]
        private String getterCellphone = null;

        public String GeterCellphone
        {
            get { return this.getterCellphone; }
        }

        [ExcelHeader("수령인 전화번호", 16)]
        private String getterPhone = null;

        public String GetterPhone
        {
            get { return this.getterPhone; }
        }

        [ExcelHeader("배송번호", 17)]
        private String deliveryNumber = null;

        public String DeliveryNumber
        {
            get { return this.deliveryNumber; }
        }

        [ExcelHeader("배송비 금액", 18)]
        private String deliveryPrice = null;

        public Decimal DeliveryPrice
        {
            get { return Decimal.Parse(this.deliveryPrice.Replace(",", "")); }
        }

        [ExcelHeader("발송일자", 19)]
        private String sendDate = null;

        public String SendDate
        {
            get { return this.sendDate; }
        }

        [ExcelHeader("배송완료일자", 20)]
        private String sendCompleteDate = null;

        public String SendCometeDate
        {
            get { return this.sendCompleteDate; }
        }

        [ExcelHeader("택배사명(발송방법)", 21)]
        private String deliveryFirm = null;

        public String DeliveryFirm
        {
            get { return this.deliveryFirm; }
        }

        [ExcelHeader("송장번호", 22)]
        private String deliveryKey = null;

        public String DeliveryKey
        {
            get { return this.deliveryKey; }
        }

        [ExcelHeader("구매자 휴대폰", 23)]
        private String buyerCellPhone = null;

        public String BuyerCellPhone
        {
            get { return this.buyerCellPhone; }
        }

        [ExcelHeader("구매자 전화번호", 24)]
        private String buyerPhoneNumber = null;

        public String BuyerPhoneNumber
        {
            get { return this.buyerPhoneNumber; }
        }

        [ExcelHeader("장바구니번호(결제번호)", 25)]
        private String buyerKey = null;

        public String BuyerKey
        {
            get { return this.buyerKey; }
        }

        [ExcelHeader("주문일자(결제확인전)", 26)]
        private String orderDate = null;

        public String OrderDate
        {
            get { return orderDate; }
        }

        [ExcelHeader("판매자 관리코드", 27)]
        private String sellerCode = null;

        public String SellerCode
        {
            get { return this.sellerCode; }
        }

        [ExcelHeader("판매자 상세관리코드", 28)]
        private String sellerDetailCode = null;

        public String SellerDetailCode
        {
            get { return this.sellerDetailCode; }
        }

        [ExcelHeader("서비스이용료", 29)]
        private String servicePrice = null;

        public String ServicePrice
        {
            get { return this.servicePrice; }
        }

        [ExcelHeader("정산예정금액", 30)]
        private String expectedPrice = null;

        public Decimal ExpectPrice
        {
            get { return Decimal.Parse(this.expectedPrice.Replace(",", "")); }
        }

        [ExcelHeader("주문확인일자", 31)]
        private String orderConfirmDate = null;

        public String OrderConfirmDate
        {
            get { return this.orderConfirmDate; }
        }

        [ExcelHeader("판매자쿠폰할인", 32)]
        private String sellerCouponDiscount = null;

        public String SellerCouponDiscount
        {
            get { return this.sellerCouponDiscount; }
        }

        [ExcelHeader("스마일포인트적림", 33)]
        private String smilePoint = null;

        public String SmilePoint
        {
            get { return this.smilePoint; }
        }

        [ExcelHeader("일시불할인", 34)]
        private String lumpDiscount = null;

        public String LumpDiscount
        {
            get { return this.lumpDiscount; }
        }

        [ExcelHeader("(옥션)복수구매할인", 35)]
        private String doubleBuyDiscount = null;

        public String DoubleBuyDiscount
        {
            get { return this.doubleBuyDiscount; }
        }

        [ExcelHeader("(옥션)우수회원할인", 36)]
        private String vipDiscount = null;

        public String VipDiscount
        {
            get { return this.vipDiscount; }
        }

        [ExcelHeader("결제완료일", 37)]
        private String paymentCompleteDate = null;

        public String PaymentCompeteDate
        {
            get { return this.paymentCompleteDate; }
        }

        [ExcelHeader("정산완료일", 38)]
        private String completeDate = null;

        public String CompleteDate
        {
            get { return this.completeDate; }
        }

        [ExcelHeader("배송구분", 39)]
        private String deliveryType = null;

        public String DeliveryType
        {
            get { return this.deliveryType; }
        }

        [ExcelHeader("주문종류", 40)]
        private String orderType = null;

        public String OrderType
        {
            get { return this.orderType; }
        }

        [ExcelHeader("SKU번호 및 수량", 41)]
        private String skyNumber = null;

        public String SkyNumber
        {
            get { return this.skyNumber; }
        }

        [ExcelHeader("글로벌샵구분", 42)]
        private String globalShopType = null;

        public String GlobalShopType
        {
            get { return this.globalShopType; }
        }

        [ExcelHeader("해외배송여부", 43)]
        private String otherContry = null;

        public String OtherContry
        {
            get { return this.otherContry; }
        }

        [ExcelHeader("제휴사명", 44)]
        private String partnerName = null;

        public String PartnerName
        {
            get { return this.partnerName; }
        }


    }
}
