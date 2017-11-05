using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class CompanyInfo : AbstractModel
    {
        private Int64 idx;
        private String companycode;
        private String companyname;
        private String companyaddress;
        private String companypostnumber;
        private String companysecuritynumber;
        private String representative;
        private String representativenumber;
        private String companynumber;
        private String companyfax;
        private String companyemail;
        private String representativeemail;
        private String companyaccountnumber;
        private String companyaccountbank;
        private String companyaccountbankcode;
        private String companyaccountbankcodename;
        private String companyaccountownername;
        private DateTime companyestablishmenidate;
        private DateTime createdate;
        private String creater;
        private String state;
        private String orderaddress;
        private String companyname_en;
        private String companyaddress_en;
        private String representative_en;
        private String orderaddress_en;
        private String companynumbertype;
        private String companyfaxtype;
        private String companyrepresentativenumber;

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
        public String CompanyCode
        {
            get { return companycode; }
            set { companycode = value; }
        }
        public String CompanyName
        {
            get { return companyname; }
            set { companyname = value; }
        }
        public String CompanyAddress
        {
            get { return companyaddress; }
            set { companyaddress = value; }
        }
        public String CompanyPostNumber
        {
            get { return companypostnumber; }
            set { companypostnumber = value; }
        }
        public String CompanySecurityNumber
        {
            get { return companysecuritynumber; }
            set { companysecuritynumber = value; }
        }
        public String Representative
        {
            get { return representative; }
            set { representative = value; }
        }
        public String RepresentativeNumber
        {
            get { return representativenumber; }
            set { representativenumber = value; }
        }
        public String CompanyNumber
        {
            get { return companynumber; }
            set { companynumber = value; }
        }
        public String CompanyFax
        {
            get { return companyfax; }
            set { companyfax = value; }
        }
        public String CompanyEmail
        {
            get { return companyemail; }
            set { companyemail = value; }
        }
        public String RepresentativeEmail
        {
            get { return representativeemail; }
            set { representativeemail = value; }
        }
        public String CompanyAccountNumber
        {
            get { return companyaccountnumber; }
            set { companyaccountnumber = value; }
        }
        public String CompanyAccountBank
        {
            get { return companyaccountbank; }
            set { companyaccountbank = value; }
        }
        public String CompanyAccountBankCode
        {
            get { return companyaccountbankcode; }
            set { companyaccountbankcode = value; }
        }
        public String CompanyAccountBankCodeName
        {
            get { return companyaccountbankcodename; }
            set { companyaccountbankcodename = value; }
        }
        public String CompanyAccountOwnerName
        {
            get { return companyaccountownername; }
            set { companyaccountownername = value; }
        }
        public DateTime CompanyEstablishmeniDate
        {
            get { return companyestablishmenidate; }
            set { companyestablishmenidate = value; }
        }
        public String CompanyEstablishmeniDateString
        {
            get { return companyestablishmenidate.ToString("yyyy-MM-dd"); }
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
        public String OrderAddress
        {
            get { return orderaddress; }
            set { orderaddress = value; }
        }
        public String CompanyName_En
        {
            get { return companyname_en; }
            set { companyname_en = value; }
        }
        public String CompanyAddress_En
        {
            get { return companyaddress_en; }
            set { companyaddress_en = value; }
        }
        public String Representative_En
        {
            get { return representative_en; }
            set { representative_en = value; }
        }
        public String OrderAddress_En
        {
            get { return orderaddress_en; }
            set { orderaddress_en = value; }
        }
        public String CompanyNumberType
        {
            get { return companynumbertype; }
            set { companynumbertype = value; }
        }
        public String CompanyFaxType
        {
            get { return companyfaxtype; }
            set { companyfaxtype = value; }
        }
        public String CompanyRepresentativeNumber
        {
            get { return companyrepresentativenumber; }
            set { companyrepresentativenumber = value; }
        }
        public String CompanyPostNumber1
        {
            get; set;
        }
        public String CompanyPostNumber2
        {
            get; set;
        }
        public String RepresentativeNumber1
        {
            get; set;
        }
        public String RepresentativeNumber2
        {
            get; set;
        }
        public String RepresentativeNumber3
        {
            get; set;
        }
        public String CompanyNumber1
        {
            get; set;
        }
        public String CompanyNumber2
        {
            get; set;
        }
        public String CompanyNumber3
        {
            get; set;
        }
        public String CompanyFax1
        {
            get; set;
        }
        public String CompanyFax2
        {
            get; set;
        }
        public String CompanyFax3
        {
            get; set;
        }
        public void NumberSplit()
        {
            try
            {
                String[] buffer = companypostnumber.Split('-');
                CompanyPostNumber1 = buffer[0];
                CompanyPostNumber2 = buffer[1];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("우편번호 분할시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }
            try
            {
                String[] buffer = companynumber.Split('-');
                CompanyNumber1 = buffer[0];
                CompanyNumber2 = buffer[1];
                CompanyNumber3 = buffer[2];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("회사전화번호 분할시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }
            try
            {
                String[] buffer = companyfax.Split('-');
                CompanyFax1 = buffer[0];
                CompanyFax2 = buffer[1];
                CompanyFax3 = buffer[1];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("팩스번호 분할시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }
            try
            {
                String[] buffer = representativenumber.Split('-');
                RepresentativeNumber1 = buffer[0];
                RepresentativeNumber2 = buffer[1];
                RepresentativeNumber3 = buffer[1];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("대표자전화번호 분할시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }
        }
        public void NumberJoin()
        {
            companypostnumber = CompanyPostNumber1 + "-" + CompanyPostNumber2;
            companynumber = CompanyNumber1 + "-" + CompanyNumber2 + "-" + CompanyNumber3;
            companyfax = CompanyFax1 + "-" + CompanyFax2 + "-" + CompanyFax3;
            representativenumber = RepresentativeNumber1 + "-" + RepresentativeNumber2 + "-" + RepresentativeNumber3;
        }
    }
}