using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Entity
{
    public class SystemData : AbstractBean
    {
        private String kycd;
        private String dt;

        public String Kycd
        {
            get { return this.kycd; }
            set { this.kycd = value; }
        }

        public string Dt
        {
            get { return this.dt; }
            set { this.dt = value; }
        }
    }
}