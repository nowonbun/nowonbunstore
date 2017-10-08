using System;
using WebScraping.Dao.Attribute;
using MySql.Data.MySqlClient;

namespace WebScraping.Dao.Entity
{
    [Table("ScrapingStatusType")]
    public class ScrapingStatusType
    {
        [Column("STATUS", MySqlDbType.VarChar, LogicalName = "status")]
        private String status;
        [Column("STATUSNAME", MySqlDbType.VarChar, LogicalName = "statusname")]
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
