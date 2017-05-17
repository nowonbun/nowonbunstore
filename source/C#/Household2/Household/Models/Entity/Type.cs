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
        private String cd;

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
        public String Cd
        {
            get { return this.cd; }
            set { this.cd = value; }
        }
    }
}