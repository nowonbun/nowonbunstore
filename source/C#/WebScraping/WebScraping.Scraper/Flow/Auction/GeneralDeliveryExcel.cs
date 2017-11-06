﻿using System;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class GeneralDeliveryExcel
    {
        [ExcelHeader("판매자쿠폰할인", 0)]
        private String sellerCoupon;

        public String SellerCoupon
        {
            get { return this.sellerCoupon; }
        }

        [ExcelHeader("아이디", 1)]
        private String id;

        public String Id
        {
            get { return this.id; }
        }

        [ExcelHeader("주문일자", 2)]
        private String orderDate;

        public String OrderDate
        {
            get { return this.orderDate; }
        }

        [ExcelHeader("주문번호", 3)]
        private String orderNumber;

        public String OrderNumber
        {
            get { return this.orderNumber; }
        }

        [ExcelHeader("상품번호", 4)]
        private String orderProductNumber;

        public String OrderProductNumber
        {
            get { return this.orderProductNumber; }
        }

        [ExcelHeader("상품명", 5)]
        private String orderName;

        public String OrderName
        {
            get { return this.orderName; }
        }

        [ExcelHeader("금액", 6)]
        private String price;

        public String Price
        {
            get { return this.price; }
        }

        [ExcelHeader("수량", 7)]
        private String amount;

        public String Amount
        {
            get { return this.amount; }
        }

        [ExcelHeader("구매자ID", 8)]
        private String buyerId;

        public String BuyerId
        {
            get { return this.buyerId; }
        }

        [ExcelHeader("구매자명", 9)]
        private String buyerName;

        public String BuyerName
        {
            get { return this.buyerName; }
        }

        [ExcelHeader("수령인명", 10)]
        private String getterName;

        public String GetterName
        {
            get { return this.getterName; }
        }

        [ExcelHeader("배송번호", 11)]
        private String deliveryName;

        public String DeliveryName
        {
            get { return this.deliveryName; }
        }

        [ExcelHeader("배송비", 12)]
        private String deliveryType;

        public String DeliveryType
        {
            get { return this.deliveryType; }
        }

        [ExcelHeader("배송비 금액", 13)]
        private String deliveryPrice;

        public String DeliveryPrice
        {
            get { return this.deliveryPrice; }
        }

        [ExcelHeader("택배사명", 14)]
        private String deliveryFirm;

        public String DeliveryFirm
        {
            get { return this.deliveryFirm; }
        }

        [ExcelHeader("운송장번호", 15)]
        private String waybayNumber;

        public String WaybayNumber
        {
            get { return this.waybayNumber; }
        }

        [ExcelHeader("필수선택", 16)]
        private String requiredSelection;

        public String RequiredSelection
        {
            get { return this.requiredSelection; }
        }

        [ExcelHeader("추가구성", 17)]
        private String addtionStructure;

        public String AddtionStructure
        {
            get { return this.addtionStructure; }
        }

        [ExcelHeader("휴대폰", 18)]
        private String cellphone;

        public String Cellphone
        {
            get { return this.cellphone; }
        }

        [ExcelHeader("전화번호", 19)]
        private String phoneNumber;

        public String PhoneNumber
        {
            get { return this.phoneNumber; }
        }

        [ExcelHeader("우편번호", 20)]
        private String postNumber;

        public String PostNumber
        {
            get { return this.postNumber; }
        }

        [ExcelHeader("주소", 21)]
        private String address;

        public String Address
        {
            get { return this.address; }
        }

        [ExcelHeader("배송시 요구사항", 22)]
        private String other;

        public String Other
        {
            get { return this.other; }
        }

        [ExcelHeader("장바구니번호(결제번호)", 23)]
        private String shoppingCartNumber;

        public String ShoppingCartNumber
        {
            get { return this.shoppingCartNumber; }
        }

        [ExcelHeader("발송예정일", 24)]
        private String sendPlanDate;

        public String SendPlanDate
        {
            get { return this.sendPlanDate; }
        }

        [ExcelHeader("배송지연사유", 25)]
        private String deliveryDelay;

        public String DeliveryDelay
        {
            get { return this.deliveryDelay; }
        }

        [ExcelHeader("사은품", 26)]
        private String gift;

        public String Gift
        {
            get { return this.gift; }
        }

        [ExcelHeader("구매자 휴대폰", 27)]
        private String buyerCellphone;

        public String BuyerCellphone
        {
            get { return this.buyerCellphone; }
        }

        [ExcelHeader("구매자 전화번호", 28)]
        private String buyerphoneNumber;

        public String BuyerphoneNumber
        {
            get { return this.buyerphoneNumber; }
        }

        [ExcelHeader("판매자 관리코드", 29)]
        private String sellerCode;

        public String SellerCode
        {
            get { return this.sellerCode; }
        }

        [ExcelHeader("판매자 상세관리코드", 30)]
        private String sellerDetailCode;

        public String SellerDetailCode
        {
            get { return this.sellerDetailCode; }
        }

        [ExcelHeader("주문확인일자", 31)]
        private String orderCheckDate;

        public String OrderCheckDate
        {
            get { return this.orderCheckDate; }
        }

        [ExcelHeader("정산예정금액", 32)]
        private String selltedAmount;

        public String SelltedAmount
        {
            get { return this.selltedAmount; }
        }

        [ExcelHeader("판매방식", 33)]
        private String sellType;

        public String SellType
        {
            get { return this.sellType; }
        }

        [ExcelHeader("결제완료일", 34)]
        private String paymentComplete;

        public String PaymentComplete
        {
            get { return this.paymentComplete; }
        }

        [ExcelHeader("주문종류", 35)]
        private String orderType;

        public String OrderType
        {
            get { return this.orderType; }
        }

    }
}
