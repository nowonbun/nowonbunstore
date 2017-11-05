using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;


namespace LogisticsSystem.Models
{
    public class Bill : AbstractModel
    {
        private Int64 idx;
        private String inordercompany;
        private String inorderrepresentative;
        private String inorderpost;
        private String inorderaddress;
        private String ordercompany;
        private String orderpost;
        private String orderaddress;
        private DateTime billdate;
        private Decimal billmoney;
        private Decimal billtax;
        private Decimal billtotal;
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
        public String InorderCompany
        {
            get { return inordercompany; }
            set { inorderaddress = value; }
        }
        public String InorderRepresentative
        {
            get { return inorderrepresentative; }
            set { inorderrepresentative = value; }
        }
        public String InorderPost
        {
            get { return inorderpost; }
            set { inorderpost = value; }
        }
        public String InorderAddress
        {
            get { return inorderaddress; }
            set { inorderaddress = value; }
        }
        public String OrderCompany
        {
            get { return ordercompany; }
            set { ordercompany = value; }
        }
        public String OrderPost
        {
            get { return orderpost; }
            set { orderpost = value; }
        }
        public String OrderAddress
        {
            get { return orderaddress; }
            set { orderaddress = value; }
        }
        public DateTime BillDate
        {
            get { return billdate; }
            set { billdate = value; }
        }
        public String BillDateString
        {
            get { return billdate.ToString("yyyy-MM-dd"); }
        }
        public Decimal BillMoney
        {
            get { return billmoney; }
            set { billmoney = value; }
        }
        public Decimal BillTax
        {
            get { return billtax; }
            set { billtax = value; }
        }
        public Decimal BillTotal
        {
            get { return billtotal; }
            set { billtotal = value; }
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


    }
}