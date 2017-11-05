using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class Cargo : AbstractModel
    {
        private Int64 idx;
        private Int64 productindex;
        private Decimal productinput;
        private Decimal productoutput;
        private Decimal productmoney;
        private String creater;
        private DateTime createdate;
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
        public Int64 ProductIndex
        {
            get { return productindex; }
            set { productindex = value; }
        }
        public Decimal ProductInput
        {
            get { return productinput; }
            set { productinput = value; }
        }
        public Decimal ProductOutput
        {
            get { return productoutput; }
            set { productoutput = value; }
        }
        public Decimal ProductMoney
        {
            get { return productmoney; }
            set { productmoney = value; }
        }
        public String Creater
        {
            get { return creater; }
            set { creater = value; }
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
        public String ProductType
        {
            get;
            set;
        }
        /*
        public Decimal ProductAmount
        {
            get { return (Decimal)Get("ProductAmount", typeof(Decimal)); }
        }
        public Decimal ProductAvgPrice
        {
            get { return (Decimal)Get("ProductAvgPrice", typeof(Decimal)); }
        }
        public String productname
        {
            get { return (String)Get("productname", typeof(String)); }
        }*/

        public void TypeCheck(LanguageType? lType)
        {
            if (productinput == 0 && productoutput != 0)
            {
                if (Object.Equals(lType, LanguageType.Korea))
                {
                    ProductType = "출고";
                }
                else
                {
                    ProductType = "出庫";
                }
            }
            else if (productinput != 0 && productoutput == 0)
            {
                if (Object.Equals(lType, LanguageType.Korea))
                {
                    ProductType = "입고";
                }
                else
                {
                    ProductType = "入庫";
                }
            }
            else
            {
                ProductType = "";
            }
        }
    }
}