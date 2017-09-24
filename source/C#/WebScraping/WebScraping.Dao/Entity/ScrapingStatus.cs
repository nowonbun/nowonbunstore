using System;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Entity
{
    [Table("ScrapingStatus")]
    public class ScrapingStatus
    {
        [Column("KEYCODE", System.Data.SqlDbType.Char, LogicalName = "keycode")]
        private String keycode;
        [Column("STARTTIME", System.Data.SqlDbType.DateTime, LogicalName = "starttime")]
        private DateTime starttime;
        [Column("ENDTIME", System.Data.SqlDbType.DateTime, LogicalName = "endtime")]
        private DateTime endtime;
        [Column("SCODE", System.Data.SqlDbType.Char, LogicalName = "scode")]
        private String scode;
        [Column("SID", System.Data.SqlDbType.VarChar, LogicalName = "sid")]
        private String sid;
        [Column("STATUS", System.Data.SqlDbType.Char, LogicalName = "status")]
        private String status;

        public String KeyCode
        {
            get { return this.keycode; }
            set { this.keycode = value; }
        }
        public DateTime StartTime
        {
            get { return this.starttime; }
            set { this.starttime = value; }
        }
        public DateTime EndTime
        {
            get { return this.endtime; }
            set { this.endtime = value; }
        }
        public String SCode
        {
            get { return this.scode; }
            set { this.scode = value; }
        }
        public String Sid
        {
            get { return this.sid; }
            set { this.sid = value; }
        }
        public String Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
    }
}
