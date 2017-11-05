using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Models
{
    public class Document : AbstractModel
    {
        private Int64 idx;
        private String documentcode;
        private String documenttype;
        private Int64 documentindex;
        private DateTime createdate;
        private String creater;
        private String state;
        private String companycode;
        private String creater_en;

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
        public String DocumentCode
        {
            get { return documentcode; }
            set { documentcode = value; }
        }
        public String DocumentType
        {
            get { return documenttype; }
            set { documenttype = value; }
        }
        public Int64 DocumentIndex
        {
            get { return documentindex; }
            set { documentindex = value; }
        }
        public DateTime CreateDate
        {
            get { return createdate; }
            set { createdate = value; }
        }
        public String CreateDateString
        {
            get { return createdate.ToString("yyyy-MM-dd"); }
        }
        public String Creater
        {
            get { return creater; }
            set { creater = value; }
        }
        public String Creater_En
        {
            get { return creater_en; }
            set { creater_en = value; }
        }
        public String State
        {
            get { return state; }
            set { state = value; }
        }
        public String CompanyCode
        {
            get { return companycode; }
            set { companycode = value; }
        }
    }
}