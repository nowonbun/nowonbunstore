using System;
using System.Collections.Generic;
using WebScraping.Scraper.Common;
using WebScraping.Library.Excel;

namespace WebScraping.Scraper.Flow.Interpark
{
    class ReportEntrExcel
    {
        [ExcelHeader("일자", 0)]
        private String data = null;
        public String Data
        {
            get { return this.data; }
        }

        [ExcelHeader("판매중 상품수", 0)]
        private String productCount = null;
        public String ProductCount
        {
            get { return this.productCount; }
        }
        [ExcelHeader("판매된 상품종수", 0)]
        private String productTypeCount = null;
        public String ProductTypeCount
        {
            get { return this.productTypeCount; }
        }
        [ExcelHeader("주문건수", 0)]
        private String orderCount = null;
        public String OrderCount
        {
            get { return this.orderCount; }
        }
        [ExcelHeader("주문수량", 0)]
        private String orderAmount = null;
        public String OrderAmount
        {
            get { return this.orderAmount; }
        }
        [ExcelHeader("주문금액", 0)]
        private String orderPrice = null;
        public String OrderPrice
        {
            get { return this.orderPrice; }
        }
        [ExcelHeader("취소건수", 0)]
        private String cancelCount = null;
        public String CancelCount
        {
            get { return this.cancelCount; }
        }
        [ExcelHeader("취소수량", 0)]
        private String cancelAmount = null;
        public String CancelAmount
        {
            get { return this.cancelAmount; }
        }
        [ExcelHeader("취소금액", 0)]
        private String cancelPrice = null;
        public String CancelPrice
        {
            get { return this.cancelPrice; }
        }
        [ExcelHeader("반품건수", 0)]
        private String returnCount = null;
        public String ReturnCount
        {
            get { return this.returnCount; }
        }
        [ExcelHeader("반품수량", 0)]
        private String returnAmount = null;
        public String ReturnAmount
        {
            get { return this.returnAmount; }
        }
        [ExcelHeader("반품금액", 0)]
        private String returnPrice = null;
        public String ReturnPrice
        {
            get { return this.returnPrice; }
        }
    }
}
