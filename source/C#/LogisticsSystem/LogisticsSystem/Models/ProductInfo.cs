using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class ProductInfo : AbstractModel
    {
        private Int64 idx;
        private String productcode;
        private String productname;
        private String producttype;
        private String productspec;
        private int producttax;
        private String productacquirer;
        private String productmanufacturer;
        private Decimal productcost;
        private Decimal productcostnottax;
        private Decimal productcosttax;
        private Decimal productfactoryprice;
        private Decimal productfactorypricenottax;
        private Decimal productfactorypricetax;
        private Decimal productretailprice;
        private Decimal productretailpricenottax;
        private Decimal productretailpricetax;
        private Decimal productprice;
        private Decimal productpricenottax;
        private Decimal productpricetax;
        private String barcode;
        private String qrcode;
        private String other;
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
            set { idx = value; }
        }
        public String ProductCode
        {
            get { return productcode; }
            set { productcode = value; }
        }
        public String ProductName
        {
            get { return productname; }
            set { productname = value; }
        }
        public String ProductType
        {
            get { return producttype; }
            set { producttype = value; }
        }
        public String ProductSpec
        {
            get { return productspec; }
            set { productspec = value; }
        }
        public int ProductTax
        {
            get { return producttax; }
            set { producttax = value; }
        }
        public String ProductAcquirer
        {
            get { return productacquirer; }
            set { productacquirer = value; }
        }
        public String ProductManufacturer
        {
            get { return productmanufacturer; }
            set { productmanufacturer = value; }
        }
        public Decimal ProductCost
        {
            get { return productcost; }
            set { productcost = value; }
        }
        public Decimal ProductCostNotTax
        {
            get { return productcostnottax; }
            set { productcostnottax = value; }
        }
        public Decimal ProductCostTax
        {
            get { return productcosttax; }
            set { productcosttax = value; }
        }
        public Decimal ProductFactoryPrice
        {
            get { return productfactoryprice; }
            set { productfactoryprice = value; }
        }
        public Decimal ProductFactoryPriceNotTax
        {
            get { return productfactorypricenottax; }
            set { productfactorypricenottax = value; }
        }
        public Decimal ProductFactoryPriceTax
        {
            get { return productfactorypricetax; }
            set { productfactorypricetax = value; }
        }
        public Decimal ProductRetailPrice
        {
            get { return productretailprice; }
            set { productretailprice = value; }
        }
        public Decimal ProductRetailPriceNotTax
        {
            get { return productretailpricenottax; }
            set { productretailpricenottax = value; }
        }
        public Decimal ProductRetailPriceTax
        {
            get { return productretailpricetax; }
            set { productretailpricetax = value; }
        }
        public Decimal ProductPrice
        {
            get { return productprice; }
            set { productprice = value; }
        }
        public Decimal ProductPriceNotTax
        {
            get { return productpricenottax; }
            set { productpricenottax = value; }
        }
        public Decimal ProductPriceTax
        {
            get { return productpricetax; }
            set { productpricetax = value; }
        }
        public String BarCode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        public String QRcode
        {
            get { return qrcode; }
            set { qrcode = value; }
        }
        public String Other
        {
            get { return other; }
            set { other = value; }
        }
        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate = value; }
        }
        public String CreateDateString
        {
            get { return createdate.ToString("yyyy-MM-dd"); }
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


        public String ProjectDispSpec
        {
            get; set;
        }
    }
}