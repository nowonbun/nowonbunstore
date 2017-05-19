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
        private String id;
        private String ctgry;
        private String tp;
        private DateTime dt;
        private String context;
        private Decimal price;
        private DateTime createdate;

        public int Ndx
        {
            set { index = value; }
        }
        public int Index
        {
            get { return index; }
        }
        public String Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public String Cd
        {
            set { this.ctgry = value; }
        }
        public String Ctgry
        {
            get { return this.ctgry; }
        }
        public String Tp
        {
            get { return this.tp; }
            set { this.tp = value; }
        }
        public DateTime Dt
        {
            set { this.dt = value; }
        }
        public DateTime Date
        {
            get { return this.dt; }
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
        public DateTime Pdt
        {
            set {
                if(value != null)
                {
                    this.createdate = value;
                }
            }
        }
        public DateTime Createdate
        {
            get { return this.createdate; }
        }
    }
}