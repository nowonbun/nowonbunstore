using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIsSocketServices;

namespace AisWebServicesDao
{
    [Table("testDB")]
    public class TestDB
    {
        [Column("TEST", System.Data.SqlDbType.VarChar, LogicalName = "test")]
        [Messgae("TEST")]
        private String test;

        public String Test
        {
            get { return this.test; }
            set { this.test = value; }
        }
    }
}
