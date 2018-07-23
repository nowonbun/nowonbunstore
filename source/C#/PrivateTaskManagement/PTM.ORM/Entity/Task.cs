using System;
using System.Data.OleDb;
using PTM.ORM.Attribute;
using PTM.ORM.Common;

namespace PTM.ORM.Entity
{
    [Table("Task")]
    public sealed class Task
    {
        [Column("idx", OleDbType.Integer, Key = true, Identity = true)]
        private int idx;
        public int Idx
        {
            set { this.idx = value; }
            get { return this.idx; }
        }

        [Column("tasktype", OleDbType.Integer)]
        private int tasktype;
        public int Tasktype
        {
            set { this.tasktype = value; }
            get { return this.tasktype; }
        }

        [Column("importance", OleDbType.Integer)]
        private int importance;
        public int Importance
        {
            set { this.importance = value; }
            get { return this.importance; }
        }

        [Column("taskdate", OleDbType.Date)]
        private DateTime taskdate;
        public DateTime Taskdate
        {
            set { this.taskdate = value; }
            get { return this.taskdate; }
        }

        [Column("title", OleDbType.VarChar, ColumnSize = 255)]
        private String title;
        public String Title
        {
            set { this.title = value; }
            get { return this.title; }
        }

        [Column("contents", OleDbType.VarChar)]
        private String contents;
        public String Contents
        {
            set { this.contents = value; }
            get { return this.contents; }
        }

        [Column("isDelete", OleDbType.Integer)]
        private int isDelete;
        public int IsDelete
        {
            set { this.isDelete = value; }
            get { return this.isDelete; }
        }
    }
}
