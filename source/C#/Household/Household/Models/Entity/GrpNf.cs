using System;
using HouseholdORM;

namespace Household.Models.Entity
{
    [Table("CRP_NF")]
    public class GrpNf
    {
        [Column("GRPD", System.Data.SqlDbType.VarChar, Key = true, LogicalName = "그룹ID")]
        private String grpd;

        [Column("GRPNM", System.Data.SqlDbType.VarChar, LogicalName = "그룹명")]
        private String groupnm;

        [Column("CPTN", System.Data.SqlDbType.VarChar, LogicalName = "캡션")]
        private String cptn;

        public String Grpd
        {
            get { return grpd; }
            set { grpd = value; }
        }

        public String Groupnm
        {
            get { return groupnm; }
            set { groupnm = value; }
        }

        public String Cptn
        {
            get { return cptn; }
            set { cptn = value; }
        }
    }
}