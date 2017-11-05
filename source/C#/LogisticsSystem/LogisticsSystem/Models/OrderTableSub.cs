using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Models
{
    /// <summary>
    /// 발주테이블
    /// </summary>
    public class OrderTableSub : AbstractModel
    {
        private Int64 idx;
        private Int64 orderkey;
        private int number;
        private Int64 productindex;
        private String productspec;
        private String producttype;
        private Decimal productamount;
        private Decimal productprice;
        private Decimal productmoney;
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
        public Int64 OrderKey
        {
            get { return orderkey; }
            set { orderkey = value; }
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
        public Decimal ProductMoney 
        {
            get { return productmoney; }
            set { productmoney = value; }
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

        public String ProductSpecDisp
        {
            get; set;
        }
        public String ProductName
        {
            get; set;
        }
    }
}