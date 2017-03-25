using System;

namespace HouseholdORM
{
    [Table("TP")]
    public class Tp : Entity
    {
        [Column("TP", System.Data.SqlDbType.Char, Key = true, LogicalName = "타입")]
        private String tp;
        [Column("NM", System.Data.SqlDbType.VarChar, LogicalName = "타입이름")]
        private String nm;
        [Column("CD", System.Data.SqlDbType.Char, LogicalName = "카테고리코드")]
        private String cd;

        public String TP
        {
            get { return tp; }
            set { tp = value; }
        }

        public String Nm
        {
            get { return nm; }
            set { nm = value; }
        }

        public String Cd
        {
            get { return cd; }
            set { cd = value; }
        }
    }
}