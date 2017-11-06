using System;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class SendingExcel
    {
        [ExcelHeader("아이디", 0)]
        private String id;

        public String Id
        {
            get { return this.id; }
        }

        [ExcelHeader("발송일자", 1)]
        private String sendDate;

        public String SendDate
        {
            get { return this.sendDate; }
        }

        [ExcelHeader("주문번호", 2)]
        private String orderNumber;

        public String OrderNumber
        {
            get { return this.orderNumber; }
        }

        [ExcelHeader("상품번호", 3)]
        private String productNumber;

        public String ProductNumber
        {
            get { return this.productNumber; }
        }

        [ExcelHeader("배송상태", 4)]
        private String deliveryState;

        public String DeliveryState
        {
            get { return this.deliveryState; }
        }

        [ExcelHeader("판매금액", 5)]
        private String sellPrice;

        public String SellPrice
        {
            get { return this.sellPrice; }
        }

        [ExcelHeader("판매단가", 6)]
        private String sellUnitPrice;

        public String SellUnitPrice
        {
            get { return this.sellUnitPrice; }
        }

        [ExcelHeader("구매자명", 7)]
        private String buyerName;

        public String BuyerName
        {
            get { return this.buyerName; }
        }

        [ExcelHeader("구매자ID", 8)]
        private String buyerId;

        public String BuyerId
        {
            get { return this.buyerId; }
        }

        [ExcelHeader("상품명", 9)]
        private String productName;

        public String ProductName
        {
            get { return this.productName; }
        }

        [ExcelHeader("수량", 10)]
        private String amount;

        public String Amount
        {
            get { return this.amount; }
        }

        [ExcelHeader("주문옵션", 11)]
        private String orderOption;

        public String OrderOption
        {
            get { return this.orderOption; }
        }

        [ExcelHeader("추가구성", 12)]
        private String addtionStructure;

        public String AddtionStructure
        {
            get { return this.addtionStructure; }
        }

        [ExcelHeader("사은품", 13)]
        private String gift;

        public String Gift
        {
            get { return this.gift; }
        }

        [ExcelHeader("수령인명", 14)]
        private String getterName;

        public String GetterName
        {
            get { return this.getterName; }
        }

        [ExcelHeader("수령인 휴대폰", 15)]
        private String getterCellphone;

        public String GetterCellphone
        {
            get { return this.getterCellphone; }
        }

        [ExcelHeader("수령인 전화번호", 16)]
        private String getterPhone;

        public String GetterPhone
        {
            get { return this.getterPhone; }
        }

        [ExcelHeader("우편번호", 17)]
        private String post;

        public String Post
        {
            get { return this.post; }
        }

        [ExcelHeader("주소", 18)]
        private String address;

        public String Address
        {
            get { return this.address; }
        }

        [ExcelHeader("배송시 요구사항", 19)]
        private String deleveryRequest;

        public String DeleveryRequest
        {
            get { return this.deleveryRequest; }
        }

        [ExcelHeader("배송번호", 20)]
        private String deleveryNumber;

        public String DeleveryNumber
        {
            get { return this.deleveryNumber; }
        }

        [ExcelHeader("배송비", 21)]
        private String deleveryType;

        public String DeleveryType
        {
            get { return this.deleveryType; }
        }

        [ExcelHeader("배송비 금액", 22)]
        private String deleveryPrice;

        public String DeleveryPrice
        {
            get { return this.deleveryPrice; }
        }

        [ExcelHeader("택배사명(발송방법)", 23)]
        private String deleveryFirm;

        public String DeleveryFirm
        {
            get { return this.deleveryFirm; }
        }

        [ExcelHeader("송장번호", 24)]
        private String invoiceNumber;

        public String InvoiceNumber
        {
            get { return this.invoiceNumber; }
        }

        [ExcelHeader("구매자 휴대폰", 25)]
        private String buyerCellphone;

        public String BuyerCellphone
        {
            get { return this.buyerCellphone; }
        }

        [ExcelHeader("구매자 전화번호", 26)]
        private String buyerPhone;

        public String BuyerPhone
        {
            get { return this.buyerPhone; }
        }

        [ExcelHeader("장바구니번호(결제번호)", 27)]
        private String shoppingCartNumber;

        public String ShoppingCartNumber
        {
            get { return this.shoppingCartNumber; }
        }

        [ExcelHeader("발송예정일", 28)]
        private String sendPlanDate;

        public String SendPlanDate
        {
            get { return this.sendPlanDate; }
        }

        [ExcelHeader("배송지연사유", 29)]
        private String deliveryDelay;

        public String DeliveryDelay
        {
            get { return this.deliveryDelay; }
        }

        [ExcelHeader("배송완료일자", 30)]
        private String deliveryComplete;

        public String DeliveryComplete
        {
            get { return this.deliveryComplete; }
        }

        [ExcelHeader("상품미수령 신고일자", 31)]
        private String noGetDate;

        public String NoGetDate
        {
            get { return this.noGetDate; }
        }

        [ExcelHeader("상품미수령 신고사유", 32)]
        private String noGetReason;

        public String NoGetReason
        {
            get { return this.noGetReason; }
        }

        [ExcelHeader("상품미수령 상세사유", 33)]
        private String noGetDetailReason;

        public String NoGetDetailReason
        {
            get { return this.noGetDetailReason; }
        }

        [ExcelHeader("상품미수령 철회 요청일자", 34)]
        private String cancelNoGetDetailReason;

        public String CancelNoGetDetailReason
        {
            get { return this.cancelNoGetDetailReason; }
        }

        [ExcelHeader("상품미수령 이의제기일자", 35)]
        private String claimNoGetDetailReason;

        public String ClaimNoGetDetailReason
        {
            get { return this.claimNoGetDetailReason; }
        }

        [ExcelHeader("판매자 관리코드", 36)]
        private String sellerCode;

        public String SellerCode
        {
            get { return this.sellerCode; }
        }

        [ExcelHeader("판매자 상세관리코드", 37)]
        private String sellerDetailCode;

        public String SellerDetailCode
        {
            get { return this.sellerDetailCode; }
        }

        [ExcelHeader("주문확인일자", 38)]
        private String orderCheckDate;

        public String OrderCheckDate
        {
            get { return this.orderCheckDate; }
        }

        [ExcelHeader("서비스이용료", 39)]
        private String serviceFee;

        public String ServiceFee
        {
            get { return this.serviceFee; }
        }

        [ExcelHeader("정산예정금액", 40)]
        private String selltedAmount;

        public String SelltedAmount
        {
            get { return this.selltedAmount; }
        }

        [ExcelHeader("판매자쿠폰할인", 41)]
        private String sellerCoupon;

        public String SellerCoupon
        {
            get { return this.sellerCoupon; }
        }

        [ExcelHeader("스마일포인트적립", 42)]
        private String smilePoint;

        public String SmilePoint
        {
            get { return this.smilePoint; }
        }

        [ExcelHeader("일시불할인", 43)]
        private String lunpDiscount;

        public String LunpDiscount
        {
            get { return this.lunpDiscount; }
        }

        [ExcelHeader("(옥션)복수구매할인", 44)]
        private String multiDiscount;

        public String MultiDiscount
        {
            get { return this.multiDiscount; }
        }

        [ExcelHeader("(옥션)우수회원할인", 45)]
        private String vipDiscount;

        public String VipDiscount
        {
            get { return this.vipDiscount; }
        }

        [ExcelHeader("판매방식", 46)]
        private String sellType;

        public String SellType
        {
            get { return this.sellType; }
        }

        [ExcelHeader("주문일자(결제확인전)", 47)]
        private String orderDate;

        public String OrderDate
        {
            get { return this.orderDate; }
        }

        [ExcelHeader("결제완료일", 48)]
        private String orderComplete;

        public String OrderComplete
        {
            get { return this.orderComplete; }
        }

        [ExcelHeader("재배송택배사명", 49)]
        private String reDeleveryFirm;

        public String ReDeleveryFirm
        {
            get { return this.reDeleveryFirm; }
        }

        [ExcelHeader("재배송지 우편번호", 50)]
        private String rePost;

        public String RePost
        {
            get { return this.rePost; }
        }

        [ExcelHeader("재배송지 주소", 51)]
        private String reAddress;

        public String ReAddress
        {
            get { return this.reAddress; }
        }

        [ExcelHeader("재배송운송장번호", 52)]
        private String reDeleveryNumber;

        public String ReDeleveryNumber
        {
            get { return this.reDeleveryNumber; }
        }

        [ExcelHeader("배송구분", 53)]
        private String deleveryDevision;

        public String DeleveryDevision
        {
            get { return this.deleveryDevision; }
        }

        [ExcelHeader("주문종류", 54)]
        private String orderType;

        public String OrderType
        {
            get { return this.orderType; }
        }

        [ExcelHeader("SKU번호 및 수량", 55)]
        private String skuNumber;

        public String SkuNumber
        {
            get { return this.skuNumber; }
        }

        [ExcelHeader("제휴사명", 56)]
        private String partner;

        public String Partner
        {
            get { return this.partner; }
        }													
    }
}
