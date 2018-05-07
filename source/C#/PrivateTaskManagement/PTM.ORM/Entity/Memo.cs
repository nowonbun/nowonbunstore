using System;
using System.Data.OleDb;
using PTM.ORM.Attribute;
using PTM.ORM.Common;

namespace PTM.ORM.Entity
{
    [Table("PTMMemo")]
    public sealed class Memo : IEntity
    {
        [Column("idx", OleDbType.Integer, Key = true, Identity = true)]
        private int idx;
        public int Idx
        {
            set { this.idx = value; }
            get { return this.idx; }
        }

        [Column("title", OleDbType.VarChar, ColumnSize = 255)]
        private String title;
        public String Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        [Column("contents", OleDbType.VarChar)]
        private String contents;
        public String Contents
        {
            get { return this.contents; }
            set { this.contents = value; }
        }

        [Column("recentlydate", OleDbType.Date)]
        private DateTime recentlydate;
        public DateTime RecentlyDate
        {
            get { return this.recentlydate; }
            set { this.recentlydate = value; }
        }
    }
}
