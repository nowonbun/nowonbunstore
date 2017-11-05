using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class UserInfo : AbstractModel
    {
        private Int64 idx;
        private String userid;
        private String password;
        private String username;
        private String state;
        private String permission;
        private String usernumber;
        private String useremail;
        private DateTime userincidentday;
        private DateTime createdate;
        private String creater;
        private String companycode;
        private String username_en;
        private String usernumbertype;

        protected override bool KeySetting(String columnName)
        {
            if (object.Equals("idx", columnName))
            {
                return true;
            }
            return false;
        }

        public Int64 Idx
        {
            get { return idx; }
            set { idx = value; }
        }
        public String UserId 
        {
            get { return userid; }
            set { userid = value; } 
        }
        public String Password 
        {
            get { return password; }
            set { password = value; } 
        }  
        public String UserName
        {
            get { return username; }
            set { username = value; }
        }
        public String State
        {
            get { return state; }
            set { state = value; }
        }
        public String Permission
        {
            get { return permission; }
            set { permission = value; }
        }
        public String UserNumber
        {
            get { return usernumber; }
            set { usernumber = value; }
        }
        public String UserEmail
        {
            get { return useremail; }
            set { useremail = value; }
        }
        public DateTime UserIncidentday
        {
            get { return userincidentday; }
            set { userincidentday = value; }
        }
        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate = value; }
        }
        public String Creater
        {
            get { return creater; }
            set { creater = value; }
        }
        public String CompanyCode
        {
            get { return companycode; }
            set { companycode = value; }
        }
        public String UserName_En
        {
            get { return username_en; }
            set { username_en = value; }
        }
        public String UsernumberType
        {
            get { return usernumbertype; }
            set { usernumbertype = value; }
        }

        public String PasswordCheck
        {
            get; set;
        }
        public String UserNumber1
        {
            get; set;
        }
        public String UserNumber2
        {
            get; set;
        }
        public String UserNumber3
        {
            get; set;
        }

        public void NumberSplit()
        {
            try
            {
                String[] buffer = usernumber.Split('-');
                UserNumber1 = buffer[0];
                UserNumber2 = buffer[1];
                UserNumber3 = buffer[2];
            }
            catch (Exception e)
            {
                LogWriter.Instance().FunctionLog();
                LogWriter.Instance().LineLog();
                LogWriter.Instance().LogWrite("전화번호 분할시 에러가 발생했습니다. ↓");
                LogWriter.Instance().LogWrite(e.ToString());
            }
        }
        public void NumberJoin()
        {
            usernumber = UserNumber1 + "-" + UserNumber2 + "-" + UserNumber3;
        }
        
        
    }
}