using System;

namespace HouseholdORM
{
    [Table("SYS_DT")]
    public class SysDt : Entity
    {
        [Column("KYCD", System.Data.SqlDbType.Char, Key = true, LogicalName = "시스템코드")]
        private String kycd;

        [Column("DT", System.Data.SqlDbType.VarChar, LogicalName = "데이터")]
        private String dt;

        public String Kycd
        {
            get { return kycd; }
            set { kycd = value; }
        }

        public String Dt
        {
            get { return dt; }
            set { dt = value; }
        }
    }
}