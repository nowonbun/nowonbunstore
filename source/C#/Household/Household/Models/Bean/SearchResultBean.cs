using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Bean
{
    public class SearchResultBean : AbstractBean
    {
        public class Node : AbstractBean
        {
            private int idx;
            private String cd;
            private String tp;
            private DateTime date;
            private String day;
            private String type;
            private String category;
            private String content;
            private String priceStr;
            private Decimal price;
            private String pdt;

            public int Idx
            {
                get { return idx; }
                set { idx = value; }
            }

            public String Cd
            {
                get { return cd; }
                set { cd = value; }
            }

            public String Tp
            {
                get { return tp; }
                set { tp = value; }
            }

            public DateTime Date
            {
                get { return date; }
                set { date = value; }
            }

            public String Day
            {
                get { return day; }
                set { day = value; }
            }

            public String Category
            {
                get { return category; }
                set { category = value; }
            }

            public String Type
            {
                get { return type; }
                set { type = value; }
            }

            public String Content
            {
                get { return content; }
                set { content = value; }
            }

            public String Price
            {
                get { return priceStr; }
            }
            public Decimal PriceNum
            {
                get { return price; }
                set
                {
                    price = value;
                    priceStr = value.ToString("#,##0").Replace("-", "");
                }
            }
            public String Pdt
            {
                get { return pdt; }
                set { pdt = value; }
            }
        }
        private Decimal incomeAmount;
        private Decimal expendAmount;
        private Decimal totalAmount;
        private Decimal accountAmount;
        private Decimal creditAmount;
        private String totalAmountStr;
        private String accountAmountStr;
        private String creditAmountStr;
        private String incomeAmountStr;
        private String expendAmountStr;

        private IList<Node> totalList = new List<Node>();
        private IList<Node> accountList = new List<Node>();
        private IList<Node> creditList = new List<Node>();

        public Decimal IncomeAmountNum
        {
            get { return incomeAmount; }
            set
            {
                incomeAmount = value;
                incomeAmountStr = value.ToString("#,##0").Replace("-", "");
            }
        }

        public String IncomeAmount
        {
            get { return incomeAmountStr; }
        }

        public Decimal ExpendAmountNum
        {
            get { return expendAmount; }
            set
            {
                expendAmount = value;
                expendAmountStr = value.ToString("#,##0").Replace("-", "");
            }
        }

        public String ExpendAmount
        {
            get { return expendAmountStr; }
        }

        public Decimal TotalAmountNum
        {
            get { return totalAmount; }
            set
            {
                totalAmount = value;
                totalAmountStr = value.ToString("#,##0").Replace("-", "");
            }
        }
        public String TotalAmount
        {
            get { return totalAmountStr; }
        }

        public Decimal AccountAmountNum
        {
            get { return accountAmount; }
            set
            {
                accountAmount = value;
                accountAmountStr = value.ToString("#,##0").Replace("-", "");
            }
        }

        public String AccountAmount
        {
            get { return accountAmountStr; }
        }

        public Decimal CreditAmountNum
        {
            get { return creditAmount; }
            set
            {
                creditAmount = value;
                creditAmountStr = value.ToString("#,##0").Replace("-", "");
            }
        }

        public String CreditAmount
        {
            get { return creditAmountStr; }
        }

        public void AddTotal(Node node)
        {
            totalList.Add(node);
        }
        public void AddAccount(Node node)
        {
            accountList.Add(node);
        }
        public void AddCredit(Node node)
        {
            creditList.Add(node);
        }

        public IList<Node> TotalList
        {
            get { return totalList; }
            set { totalList = value; }
        }

        public IList<Node> AccountList
        {
            get { return accountList; }
            set { accountList = value; }
        }

        public IList<Node> CreditList
        {
            get { return creditList; }
            set { creditList = value; }
        }
    }
}