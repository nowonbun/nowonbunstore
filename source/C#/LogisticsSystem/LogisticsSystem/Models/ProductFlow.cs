using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Models
{
    public class ProductFlow : AbstractModel
    {
        private Int64 idx;
        private Int64 productindex;
        private Decimal productamount;
        private Decimal productbuyprice;
        private Decimal productsellprice;
        private DateTime cretedate;
        private String creater;
        private Int64 applytype;
        private String state;
        private String companycode;
        private String etc;

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
        public Decimal ProductAmount
        {
            get { return productamount; }
            set { productamount = value; }
        }
        public Decimal ProductBuyPrice
        {
            get { return productbuyprice; }
            set { productbuyprice = value; }
        }
        public Decimal ProductSellPrice
        {
            get { return productsellprice; }
            set { productsellprice = value; }
        }
        public DateTime CreteDate
        {
            get { return cretedate; }
            set { cretedate = value; }
        }
        public String Creater
        {
            get { return creater; }
            set { creater = value; }
        }
        public Int64 ApplyType
        {
            get { return applytype; }
            set { applytype = value; }
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
        public String ETC
        {
            get { return etc; }
            set { etc = value; }
        }

        public String StateDisp
        {
            get; set;
        }
        public String ProductName
        {
            get; set;
        }

        public void stateView(LanguageType? lType)
        {
            if (Define.ProductFlow.INCOMESTANBY.ToString().Equals(state.ToString()))
            {
                if (lType == LanguageType.Korea) StateDisp = "입고대기";
                else StateDisp = "入庫待ち";
            }
            else if (Define.ProductFlow.INCOMECANCEL.ToString().Equals(state.ToString()))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인취소";
                else StateDisp = "承認取消";
            }
            else if (Define.ProductFlow.INCOMECOMPLATE.ToString().Equals(state))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인";
                else StateDisp = "承認";
            } 
            else if (Define.ProductFlow.OUTCOMESTANBY.ToString().Equals(state.ToString()))
            {
                if (lType == LanguageType.Korea) StateDisp = "출고대기";
                else StateDisp = "出庫待ち";
            }
            else if (Define.ProductFlow.OUTPUTCANCEL.ToString().Equals(state.ToString()))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인취소";
                else StateDisp = "承認取消";
            }
            else if (Define.ProductFlow.OUTPUTCOMPLATE.ToString().Equals(state))
            {
                if (lType == LanguageType.Korea) StateDisp = "승인";
                else StateDisp = "承認";
            }
        }
    }
}