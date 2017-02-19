using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Bean
{
    public class ApplyBean : AbstractBean
    {
        private String householdIdx;
        private String householdYear;
        private String householdMonth;
        private String householdDay;
        private String householdType;
        private String householdCategory;
        private String householdContent;
        private String householdPrice;
        private String householdPdt;

        public String HouseholdIdx
        {
            get { return householdIdx; }
            set { householdIdx = value; }
        }
        public String HouseholdYear
        {
            get { return householdYear; }
            set { householdYear = value; }
        }

        public String HouseholdMonth
        {
            get { return householdMonth; }
            set { householdMonth = value; }
        }

        public String HouseholdDay
        {
            get { return householdDay; }
            set { householdDay = value; }
        }

        public String HouseholdType
        {
            get { return householdType; }
            set { householdType = value; }
        }

        public String HouseholdCategory
        {
            get { return householdCategory; }
            set { householdCategory = value; }
        }

        public String HouseholdContent
        {
            get { return householdContent; }
            set { householdContent = value; }
        }

        public String Householdprice
        {
            get { return householdPrice; }
            set { householdPrice = value; }
        }

        public String HouseholdPdt
        {
            get { return householdPdt; }
            set { householdPdt = value; }
        }
    }
}