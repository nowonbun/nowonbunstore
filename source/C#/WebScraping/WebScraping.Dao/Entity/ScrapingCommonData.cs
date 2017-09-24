using System;
using WebScraping.Dao.Attribute;

namespace WebScraping.Dao.Entity
{
    [Table("ScrapingCommonData")]
    public class ScrapingCommonData
    {
        [Column("KEYCODE", System.Data.SqlDbType.Char, LogicalName = "keycode")]
        private String keycode;
        [Column("KEYINDEX", System.Data.SqlDbType.Int, LogicalName = "keyindex")]
        private int keyindex;
        [Column("DATA", System.Data.SqlDbType.VarChar, LogicalName = "data")]
        private String data;
        [Column("CREATEDATE", System.Data.SqlDbType.DateTime, LogicalName = "createdate")]
        private DateTime createdate;

        public String KeyCode
        {
            get { return this.keycode; }
            set { this.keycode = value; }
        }
        public int keyIndex
        {
            get { return this.keyindex; }
            set { this.keyindex = value; }
        }
        public String Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
        public DateTime CreateDate
        {
            get { return this.createdate; }
            set { this.createdate = value; }
        }
    }
}
