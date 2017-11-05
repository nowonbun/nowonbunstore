using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class Connect : AbstractModel
    {
        private String userid;
        private DateTime connectdate;
        private String state;
        private String language;
        private String ipaddress;

        protected override bool KeySetting(String columnName)
        {
            if (Object.Equals("ipaddress", columnName))
            {
                return true;
            }
            return false;
        }

        public String UserId
        {
            get { return userid; }
            set { userid = value; }
        }
        public DateTime ConnectDate
        {
            get { return connectdate; }
            set { connectdate = value; }
        }
        public String State
        {
            get { return state; }
            set { state = value; }
        }
        public String Language
        {
            get { return language; }
            set { language = value; }
        }
        public String IpAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }
    }
}