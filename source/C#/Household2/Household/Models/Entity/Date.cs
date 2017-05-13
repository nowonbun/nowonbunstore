using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Models.Entity
{
    public class Date
    {
        private Int64 fastTime;
        private String cdate;

        public Int64 FastTime
        {
            set { fastTime = value; }
        }
        public string Cdate
        {
            set { cdate = value; }
        }
        public DateTime ToDateTime()
        {
            return new DateTime(fastTime);
        }
    }
}