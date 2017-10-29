using System;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Auction
{
    class LacRemitListExcel
    {
        [ExcelHeader("구분", 0)]
        private String type;

        public String Type
        {
            get { return this.type; }
        }

        [ExcelHeader("발송일자", 1)]
        private String sendDate;

        public String SendDate
        {
            get { return this.sendDate; }
        }

        [ExcelHeader("송금일자", 2)]
        private String sendMoneyDate;

        public String SendMoneyDate
        {
            get { return this.sendMoneyDate; }
        }

        [ExcelHeader("구매결정일", 3)]
        private String buyDecisionDate;

        public String BuyDecisionDate
        {
            get { return this.buyDecisionDate; }
        }

        [ExcelHeader("결제번호", 4)]
        private String accountNumber;

        public String AccountNumber
        {
            get { return this.accountNumber; }
        }

        [ExcelHeader("상품번호", 5)]
        private String productNumber;

        public String ProductNumber
        {
            get { return this.productName; }
        }

        [ExcelHeader("주문번호", 6)]
        private String orderNumber;

        public String OrderNumber
        {
            get { return this.orderNumber; }
        }

        [ExcelHeader("상품명", 7)]
        private String productName;

        public String ProductName
        {
            get { return productName; }
        }

        [ExcelHeader("구매자ID", 8)]
        private String buyerID;

        public String BuyerID
        {
            get { return this.buyerID; }
        }

        [ExcelHeader("구매자명", 9)]
        private String buyerName;

        public String BuyerName
        {
            get { return this.buyerName; }
        }

        [ExcelHeader("상품금액", 10)]
        private String productPrice;

        public String ProductPrice
        {
            get { return this.productPrice; }
        }

        [ExcelHeader("선결제배송비", 11)]
        private String deliveryPrice;

        public String DeliveryPrice
        {
            get { return this.deliveryPrice; }
        }

        [ExcelHeader("반품 교환 배송비", 12)]
        private String returnPrice;

        public String ReturnPrice
        {
            get { return this.returnPrice; }
        }

        [ExcelHeader("송금액", 13)]
        private String sendPrice;

        public String SendPrice
        {
            get { return this.sendPrice; }
        }
    }
}
