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
    public class OrderTable : AbstractModel
    {
        private Int64 idx;
        private String ordernumber;
        private String ordername;
        private String ordersecuritynumber;
        private String orderpostnumber;
        private String orderaddress;
        private String orderphonenumber;
        private String orderfax;
        private String inordername;
        private Decimal ordermoney;
        private DateTime ordersavedate;
        private String ordersaveplace;
        private DateTime orderdate;
        private DateTime paydate;
        private Decimal paymoney;
        private String paycondition;
        private String payother;
        private DateTime createdate;
        private String creater;
        private String ordertype;
        private String state;
        private String companycode;
        private String ordersaveplace_en;
        private String orderaddress_en;
        private String orderphonenumbertype;
        private String orderfaxtype;
        private String ordername_en;

        protected override bool KeySetting(String columnName)
        {
            if (object.Equals("idx", columnName))
            {
                return true;
            }
            return false;
        }

        public long Idx
        {
            get { return idx; }
            set { idx = value; }
        }
        public string OrderNumber
        {
            get { return ordernumber; }
            set { ordernumber = value; }
        }
        public string OrderName
        {
            get { return ordername; }
            set { ordername = value; }
        }
        public string OrdersecurityNumber
        {
            get { return ordersecuritynumber; }
            set { ordersecuritynumber = value; }
        }
        public string OrderPostnumber
        {
            get { return orderpostnumber; }
            set { orderpostnumber = value; }
        }
        public string OrderAddress
        {
            get { return orderaddress; }
            set { orderaddress = value; }
        }
        public string OrderPhoneNumber
        {
            get { return orderphonenumber; }
            set { orderphonenumber = value; }
        }
        public string OrderFax
        {
            get { return orderfax; }
            set { orderfax = value; }
        }
        public string InorderName
        {
            get { return inordername; }
            set { inordername = value; }
        }
        public decimal OrderMoney
        {
            get { return ordermoney; }
            set { ordermoney = value; }
        }
        public DateTime OrderSaveDate
        {
            get { return ordersavedate; }
            set { ordersavedate = value; }
        }
        public string OrderSavePlace
        {
            get { return ordersaveplace; }
            set { ordersaveplace = value; }
        }
        public DateTime OrderDate
        {
            get { return orderdate; }
            set { orderdate = value; }
        }
        public DateTime PayDate
        {
            get { return paydate; }
            set { paydate = value; }
        }
        public decimal PayMoney
        {
            get { return paymoney; }
            set { paymoney = value; }
        }
        public string PayCondition
        {
            get { return paycondition; }
            set { paycondition = value; }
        }
        public string PayOther
        {
            get { return payother; }
            set { payother = value; }
        }
        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate = value; }
        }
        public string Creater
        {
            get { return creater; }
            set { creater = value; }
        }
        public string OrderType
        {
            get { return ordertype; }
            set { ordertype = value; }
        }
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        public string CompanyCode
        {
            get { return companycode; }
            set { companycode = value; }
        }
        public string OrderSavePlace_En
        {
            get { return ordersaveplace_en; }
            set { ordersaveplace_en = value; }
        }
        public string OrderAddress_En
        {
            get { return orderaddress_en; }
            set { orderaddress_en = value; }
        }
        public string OrderPhoneNumbertype
        {
            get { return orderphonenumbertype; }
            set { orderphonenumbertype = value; }
        }
        public string OrderFaxtype
        {
            get { return orderfaxtype; }
            set { orderfaxtype = value; }
        }
        public string OrderName_En
        {
            get { return ordername_en; }
            set { ordername_en = value; }
        }

        public String StateDisp
        {
            get; set;
        }
        public String PrintSetting
        {
            get; set;
        }
        public String InOrderAddress
        {
            get; set;
        }

        public void StateView(LanguageType? lType)
        {
            if (App_Code.Define.STATE_NORMAL.ToString().Equals(state))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인대기";
                else StateDisp = "承認待ち";
            }
            else if (App_Code.Define.STATE_DELETE.ToString().Equals(state))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인취소";
                else StateDisp = "承認取消";
            }
            else if (App_Code.Define.STATE_APPLY.ToString().Equals(state))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인";
                else StateDisp = "承認";
            }
        }

    }
}