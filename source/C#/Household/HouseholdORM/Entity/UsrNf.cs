using System;

namespace HouseholdORM
{
    [Table("USR_NF")]
    public class UsrNf : Entity
    {
        [Column("GRPD", System.Data.SqlDbType.VarChar, LogicalName = "그룹ID")]
        private String grpd;

        [Column("USRD", System.Data.SqlDbType.VarChar, LogicalName = "유저ID")]
        private String usrd;

        [Column("NM", System.Data.SqlDbType.VarChar, LogicalName = "유저명")]
        private String nm;

        [Column("PSWRD", System.Data.SqlDbType.VarChar, LogicalName = "패스워드")]
        private String pswrd;

        public String Grpd
        {
            get { return grpd; }
            set { grpd = value; }
        }

        public String Usrd
        {
            get { return usrd; }
            set { usrd = value; }
        }

        public String Nm
        {
            get { return nm; }
            set { nm = value; }
        }

        public String Pswrd
        {
            get { return pswrd; }
            set { pswrd = value; }
        }
    }
}