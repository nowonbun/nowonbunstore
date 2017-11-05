using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Models
{
    public class DeliveryTableSub : AbstractModel
    {
        private Int64 idx;
        private Int64 deliverykey;
        private int number;
        private Int64 productindex;
        private String productspec;
        private String producttype;
        private Decimal productamount;
        private Decimal productprice;
        private Decimal productvat;
        private String productother;
        private DateTime createdate;
        private String creater;
        private String state;
        private String companycode;

        protected override bool KeySetting(String columnName)
        {
            if (object.Equals("idx", columnName))
            {
                return true;
            }
            return false;
        }

        public Int64 Idx
        {
            get { return idx; }
            set { idx= value; }
        }
        public Int64 DeliveryKey 
        {
            get { return deliverykey; }
            set { deliverykey = value; }
        }
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public Int64 ProductIndex
        {
            get { return productindex; }
            set { productindex = value; }
        }
        public String ProductSpec
        {
            get { return productspec; }
            set { productspec = value; }
        }
        public String ProductType
        {
            get { return producttype; }
            set { producttype = value; }
        }
        public Decimal ProductAmount
        {
            get { return productamount; }
            set { productamount = value; }
        }
        public Decimal ProductPrice
        {
            get { return productprice; }
            set { productprice = value; }
        }
        public Decimal ProductVat
        {
            get { return productvat; }
            set { productvat = value; }
        }
        public String ProductOther
        {
            get { return productother; }
            set { productother = value; }
        }
        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate = value; }
        }
        public String Creater 
        {
            get { return creater; }
            set { creater = value; }
        }
        public String State 
        {
            get { return state; }
            set { state = value; }
        }
        public String CompanyCode
        {
            get { return companycode; }
            set { companycode = value; }
        }
        /*
        public String productName //상품이름
        {
            get { return (String)Get("productName", typeof(String)); }
            set { Set("productName", value); }
        }
        public String productspec_disp
        {
            get { return (String)Get("productspec_disp", typeof(String)); }
            set { Set("productspec_disp", value); }
        }
        */

    }
}