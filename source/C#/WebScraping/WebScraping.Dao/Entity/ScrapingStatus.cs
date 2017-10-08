using System;
using WebScraping.Dao.Attribute;
using MySql.Data.MySqlClient;

namespace WebScraping.Dao.Entity
{
    [Table("ScrapingStatus")]
    public class ScrapingStatus
    {
        [Column("KEYCODE", MySqlDbType.VarChar, LogicalName = "keycode", Key = true)]
        private String keycode;
        [Column("STARTTIME", MySqlDbType.DateTime, LogicalName = "starttime")]
        private DateTime? starttime;
        [Column("ENDTIME", MySqlDbType.DateTime, LogicalName = "endtime")]
        private DateTime? endtime;
        [Column("SCODE", MySqlDbType.VarChar, LogicalName = "scode")]
        private String scode;
        [Column("SID", MySqlDbType.VarChar, LogicalName = "sid")]
        private String sid;
        [Column("STATUS", MySqlDbType.VarChar, LogicalName = "status")]
        private String status;

        public String KeyCode
        {
            get { return this.keycode; }
            set { this.keycode = value; }
        }
        public DateTime? StartTime
        {
            get { return this.starttime; }
            set { this.starttime = value; }
        }
        public DateTime? EndTime
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
