using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Entity
{
    public class Type : AbstractBean
    {
        private String tp;
        private String nm;
        private Category ctgry;

        public String Tp
        {
            get { return this.tp; }
            set { this.tp = value; }
        }

        public String Nm
        {
            get { return this.nm; }
            set { this.nm = value; }
        }

        public Category Ctgry
        {
            get { return this.ctgry; }
            set { this.ctgry = value; }
        }
    }
}