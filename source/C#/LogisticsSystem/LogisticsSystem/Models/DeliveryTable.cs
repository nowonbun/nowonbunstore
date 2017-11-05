using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Models
{
    public class DeliveryTable : AbstractModel
    {
        private Int64 idx;
        private String ordercompany;
        private String orderaddress;
        private DateTime ordersavedate;
        private String inordercompany;
        private String inorderrepresentative;
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
        public String OrderCompany
        {
            get { return ordercompany; }
            set { ordercompany = value; }
        }
        public String OrderAddress
        {
            get { return orderaddress; }
            set { orderaddress = value; }
        }
        public DateTime OrderSavedate
        {
            get { return ordersavedate; }
            set { ordersavedate = value; }
        }
        public String OrderSavedateString
        {
            get { return ordersavedate.ToString("yyyy-MM-dd"); }
        }
        public String InorderCompany
        {
            get { return inordercompany; }
            set { inordercompany = value; }
        }
        public String InorderRepresentative
        {
            get { return inorderrepresentative; }
            set { inorderrepresentative = value; }
        }
        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate= value; }
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

    }
}