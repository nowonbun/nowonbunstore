using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

namespace WebScraping.Dao.Create
{
    class CreateDatabaseByEntity : AbstractDaoCreate
    {
        private Dictionary<String, PropertyInfo> propertys;
        public CreateDatabaseByEntity(String connectionStr) : base(connectionStr)
        {
            this.propertys = typeof(TableInfo).GetProperties().ToDictionary(p => p.Name);
        }
        public override void Run(String dirpath)
        {
            if (!Directory.Exists(dirpath))
            {
                throw new Exception("not directory");
            }
            var tablenamelist = GetTableName();
            foreach (var item in tablenamelist)
            {
                var info = GetTableInfo(item);
            }
        }
        private IList<String> GetTableName()
        {
            IList<String> table = new List<String>();
            base.Transaction(() =>
            {
                base.ExcuteReader("show tables", null, (dr) =>
                {
                    table.Add(dr.GetString(0));
                });
            });
            return table;
        }
        private TableInfo GetTableInfo(String tablename)
        {
            TableInfo ret = null;
            base.Transaction(() =>
            {
                base.ExcuteReader(
                    "show full columns from " + tablename,
                    null,
                    (dr) =>
                    {
                        ret = new TableInfo();
                        int count = dr.FieldCount;
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            if (dr.IsDBNull(i))
                            {
                                continue;
                            }
                            if (!this.propertys.ContainsKey(dr.GetName(i)))
                            {
                                continue;
                            }
                            var property = propertys[dr.GetName(i)];
                            property.SetValue(ret, dr.GetString(i));
                        }
                    });
            });
            return ret;
        }
    }
}
