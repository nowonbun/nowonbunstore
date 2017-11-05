using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Models
{
    public class CodeMaster : AbstractModel
    {
        private String tblname;
        private String codekey;
        private String codename_k;
        private String codename_j;

        protected override bool KeySetting(String columnName)
        {
            return false;
        }

        public String TblName
        {
            get { return tblname; }
            set { tblname = value; }
        }
        public String CodeKey
        {
            get { return codekey; }
            set { codekey = value; }
        }
        public String CodeName
        {
            get;
            set;
        }
        public void Trans(LanguageType? ltype)
        {
            if (Object.Equals(ltype, LanguageType.Korea))
            {
                CodeName = codename_k;
            }
            else
            {
                CodeName = codename_j;
            }
        }
    }
}