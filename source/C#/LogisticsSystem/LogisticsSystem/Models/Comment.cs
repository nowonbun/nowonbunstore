using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using System.Text;

namespace LogisticsSystem.Models
{
    public class Comment : AbstractModel
    {
        private Int64 idx;
        private Int64 boardidx;
        private String context;
        private String creater;
        private DateTime createdate;
        private String state;

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
        public Int64 BoardIdx
        {
            get { return boardidx; }
            set { boardidx = value; }
        }
        public String Context
        {
            get { return context; }
            set { context = value; }
        }
        public String Creater
        {
            get { return creater; }
            set { creater = value; }
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
        public String State
        {
            get { return state; }
            set { state = value; }
        }
    }
}