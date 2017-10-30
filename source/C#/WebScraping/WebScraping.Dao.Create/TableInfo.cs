using System;

namespace WebScraping.Dao.Create
{
    class TableInfo
    {
        public String Field { get; set; }
        public String Type { get; set; }
        public String Collation { get; set; }
        public String Null { get; set; }
        public String Key { get; set; }
        public String Default { get; set; }
        public String Extra { get; set; }
        public String Privileges { get; set; }
        public String Comment { get; set; }
    }
}
