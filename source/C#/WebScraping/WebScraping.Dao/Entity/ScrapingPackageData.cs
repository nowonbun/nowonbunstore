using System;
using WebScraping.Dao.Attribute;
using MySql.Data.MySqlClient;

namespace WebScraping.Dao.Entity
{
    [Table("ScrapingPackageData")]
    public class ScrapingPackageData
    {
        [Column("KEYCODE", MySqlDbType.VarChar, LogicalName = "keycode", Key = true)]
        private String keycode;
        [Column("KEYINDEX", MySqlDbType.Int32, LogicalName = "keyindex", Key = true)]
        private int keyindex;
        [Column("SEPARATION", MySqlDbType.Int32, LogicalName = "separation", Key = true)]
        private int separation;
        [Column("DATA", MySqlDbType.VarChar, LogicalName = "data")]
        private String data;
        [Column("CREATEDATE", MySqlDbType.DateTime, LogicalName = "createdate")]
        private DateTime? createdate;

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
        public int Separation
        {
            get { return this.separation; }
            set { this.separation = value; }
        }
        public String Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
        public DateTime? CreateDate
        {
            get { return this.createdate; }
            set { this.createdate = value; }
        }
    }
}
