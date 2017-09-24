using System;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Entity
{
    [Table("ScrapingStatusType")]
    public class ScrapingStatusType
    {
        [Column("STATUS", System.Data.SqlDbType.Char, LogicalName = "status")]
        private String status;
        [Column("STATUSNAME", System.Data.SqlDbType.VarChar, LogicalName = "statusname")]
        private String statusname;

        public String Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        public String StatusName
        {
            get { return this.statusname; }
            set { this.statusname = value; }
        }
    }
}
