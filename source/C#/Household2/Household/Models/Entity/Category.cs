using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Entity
{
    public class Category : AbstractBean
    {
        private string cd;
        private string nm;

        public String Cd
        {
            get { return this.cd; }
            set { this.cd = value; }
        }

        public String Nm
        {
            get { return this.nm; }
            set { this.nm = value; }
        }
    }
}