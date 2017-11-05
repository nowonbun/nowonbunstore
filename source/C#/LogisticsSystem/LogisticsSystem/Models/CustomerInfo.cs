using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class CustomerInfo : AbstractModel
    {
        private Int64 idx;
        private String customercode;
        private String customertype;
        private String customername;
        private String customerrepresetitive;
        private String customersecuritynumber;
        private String customernumber;
        private String customerfax;
        private String customerpostnumber;
        private String customeraddress;
        private String customeremail;
        private String customertaxviewrepresentative;
        private String customertaxvieweraddress;
        private String customertaxviewerpostnumber;
        private String customerpaymentmethod;
        private String customeraccountbank;
        private String customeraccountbankcode;
        private String customeraccountbankcodename;
        private String customeraccountownername;
        private String customeraccountnumber;
        private String customertaxtype;
        private int customertax;
        private int customergrade;
        private String customerrepressent;
        private String customerrepressentnumber;
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
        public String CustomerCode
        {
            get { return customercode; }
            set { customercode = value; }
        }
        public String CustomerType
        {
            get { return customertype; }
            set { customertype = value; }
        }
        public String CustomerName
        {
            get { return customername; }
            set { customername = value; }
        }
        public String CustomerRepresetitive
        {
            get { return customerrepresetitive; }
            set { customerrepresetitive = value; }
        }
        public String CustomerSecurityNumber
        {
            get { return customersecuritynumber; }
            set { customersecuritynumber = value; }
        }
        public String CustomerNumber
        {
            get { return customernumber; }
            set { customernumber = value; }
        }
        public String CustomerFax
        {
            get { return customerfax; }
            set { customerfax = value; }
        }
        public String CustomerPostNumber
        {
            get { return customerpostnumber; }
            set { customerpostnumber = value; }
        }
        public String CustomerAddress
        {
            get { return customeraddress; }
            set { customeraddress = value; }
        }
        public String CustomerEmail
        {
            get { return customeremail; }
            set { customeremail = value; }
        }
        public String CustomerTaxViewRepresentative
        {
            get { return customertaxviewrepresentative; }
            set { customertaxviewrepresentative = value; }
        }
        public String CustomerTaxViewerAddress
        {
            get { return customertaxvieweraddress; }
            set { customertaxvieweraddress = value; }
        }
        public String CustomerTaxViewerPostNumber
        {
            get { return customertaxviewerpostnumber; }
            set { customertaxviewerpostnumber = value; }
        }
        public String CustomerPaymentMethod
        {
            get { return customerpaymentmethod; }
            set { customerpaymentmethod = value; }
        }
        public String CustomerAccountbank
        {
            get { return customeraccountbank; }
            set { customeraccountbank = value; }
        }
        public String CustomerAccountbankcode
        {
            get { return customeraccountbankcode; }
            set { customeraccountbankcode = value; }
        }
        public String CustomerAccountbankcodename
        {
            get { return customeraccountownername; }
            set { customeraccountownername = value; }
        }
        public String CustomerAccountOwnerName
        {
            get { return customeraccountownername; }
            set { customeraccountownername = value; }
        }
        public String CustomerAccountNumber
        {
            get { return customeraccountnumber; }
            set { customeraccountnumber = value; }
        }
        public String CustomerTaxType
        {
            get { return customertaxtype; }
            set { customertaxtype = value; }
        }
        public int CustomerTax
        {
            get { return customertax; }
            set { customertax = value; }
        }
        public int CustomerGrade
        {
            get { return customergrade; }
            set { customergrade = value; }
        }
        public String CustomerRepressent
        {
            get { return customerrepressent; }
            set { customerrepressent = value; }
        }
        public String CustomerRepressentNumber
        {
            get { return customerrepressentnumber; }
            set { customerrepressentnumber = value; }
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

        public String CustomerPostNumber1
        {
            get; set;
        }
        public String CustomerPostNumber2
        {
            get; set;
        }
        public String CustomerTaxViewerPostNumber1
        {
            get; set;
        }
        public String CustomerTaxViewerPostNumber2
        {
            get; set;
        }

        public void ConvertPostNumber()
        {
            customerpostnumber = CustomerPostNumber1 + "-" + CustomerPostNumber2;
            customertaxviewerpostnumber = CustomerTaxViewerPostNumber1 + "-" + CustomerTaxViewerPostNumber2;
        }
        
    }
}