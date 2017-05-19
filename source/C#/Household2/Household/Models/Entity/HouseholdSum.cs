using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Household.Models.Entity
{
    public class HouseholdSum
    {
        private decimal value;
        public decimal Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}