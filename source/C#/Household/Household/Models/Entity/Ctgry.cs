using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;

namespace Household.Models.Entity
{
    [Table("CTGRY")]
    public class Ctgry
    {
        [Column("CD", System.Data.SqlDbType.Char, Key = true, LogicalName = "카테코리코드")]
        private String cd;

        [Column("NM", System.Data.SqlDbType.VarChar, LogicalName = "카테고리이름")]
        private String nm;

        public String Cd
        {
            get { return cd; }
            set { cd = value; }
        }

        public String Nm
        {
            get { return nm; }
            set { nm = value; }
        }
    }
}