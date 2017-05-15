using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Models.Bean;
using Household.Common;

namespace Household.Models.Entity
{
    public class HouseHold
    {
        private int index;
        private LoginBean id;
        private Category ctgry;
        private Type tp;
        private Date dt;
        private String context;
        private Decimal price;
        private Date createdate;

        public int Ndx
        {
            set { index = value; }
        }
        public int Index
        {
            get { return index; }
        }
        public LoginBean Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public Category Ctgry
        {
            get { return this.ctgry; }
            set { this.ctgry = value; }
        }
        public Type TpBean
        {
            set { this.tp = value; }
        }
        public Type Tp
        {
            get { return this.tp; }
        }
        public Date Dt
        {
            set { this.dt = value; }
        }
        public DateTime Date
        {
            get { return this.dt.ToDateTime(); }
        }
        public String Cntxt
        {
            set { this.context = value; }
        }
        public String Context
        {
            get { return this.context; }
        }
        public Decimal Prc
        {
            set { this.price = value; }
        }
        public Decimal Price
        {
            get { return this.price; }
        }
        public Date Pdt
        {
            set { this.createdate = value; }
        }
        public DateTime Createdate
        {
            get { return this.createdate != null ? this.createdate.ToDateTime() : default(DateTime); }
        }
    }
}