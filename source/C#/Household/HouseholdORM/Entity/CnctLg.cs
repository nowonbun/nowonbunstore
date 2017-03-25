using System;

namespace HouseholdORM
{
    [Table("CNCT_LG")]
    public class CnctLg : Entity
    {
        [Column("GRPD", System.Data.SqlDbType.VarChar, Key = true, LogicalName = "그룹명")]
        private String grpd;

        [Column("USRD", System.Data.SqlDbType.VarChar, Key = true, LogicalName = "유저명")]
        private String usrd;

        [Column("CNCTM", System.Data.SqlDbType.DateTime, LogicalName = "접속시간")]
        private String cnctm;

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

        public String Cnctm
        {
            get { return cnctm; }
            set { cnctm = value; }
        }


    }
}