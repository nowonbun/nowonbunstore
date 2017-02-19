using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;

namespace Household.Models.Bean
{
    public class SearchBean : AbstractBean
    {
        private String year;
        private String month;
        private String day;
        private String type;
        
        public String Year
        {
            get { return year; }
            set { year = value; }
        }

        public String Month
        {
            get { return month; }
            set { month = value; }
        }

        public String Day
        {
            get { return day; }
            set { day = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}