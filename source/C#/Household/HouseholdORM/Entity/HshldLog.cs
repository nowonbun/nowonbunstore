using System;

namespace HouseholdORM
{
    [Table("HSHLD_LOG")]
    public class HshldLog : Entity
    {
        [Column("NDX", System.Data.SqlDbType.Int, LogicalName = "인덱스")]
        private int ndx;

        [Column("NDX2", System.Data.SqlDbType.Int, LogicalName = "인덱스2")]
        private int ndx2;

        [Column("GRPD", System.Data.SqlDbType.VarChar, LogicalName = "그룹명")]
        private String grpd;

        [Column("USRD", System.Data.SqlDbType.VarChar, LogicalName = "유저명")]
        private String usrd;

        [Column("TP", System.Data.SqlDbType.VarChar, LogicalName = "타입")]
        private String tp;

        [Column("CD", System.Data.SqlDbType.VarChar, LogicalName = "카테고리코드")]
        private String cd;

        [Column("DT", System.Data.SqlDbType.Date, LogicalName = "가계일시")]
        private DateTime dt;

        [Column("CNTXT", System.Data.SqlDbType.VarChar, LogicalName = "가계내용")]
        private String cntxt;

        [Column("PRC", System.Data.SqlDbType.Decimal, LogicalName = "가계금액")]
        private Decimal prc;

        [Column("PDT", System.Data.SqlDbType.DateTime, LogicalName = "등록일자")]
        private DateTime pdt;

        [Column("CDT", System.Data.SqlDbType.DateTime, LogicalName = "편집일자")]
        private DateTime cdt;

        public int Ndx
        {
            get { return ndx; }
            set { ndx = value; }
        }

        public int Ndx2
        {
            get { return ndx2; }
            set { ndx2 = value; }
        }

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

        public String Tp
        {
            get { return tp; }
            set { tp = value; }
        }

        public String Cd
        {
            get { return cd; }
            set { cd = value; }
        }

        public DateTime Dt
        {
            get { return dt; }
            set { dt = value; }
        }

        public String Cntxt
        {
            get { return cntxt; }
            set { cntxt = value; }
        }

        public Decimal Prc
        {
            get { return prc; }
            set { prc = value; }
        }

        public DateTime Pdt
        {
            get { return pdt; }
            set { pdt = value; }
        }

        public DateTime Cdt
        {
            get { return cdt; }
            set { cdt = value; }
        }
    }
}