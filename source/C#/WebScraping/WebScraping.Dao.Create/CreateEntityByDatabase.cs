using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

namespace WebScraping.Dao.Create
{
    class CreateEntityByDatabase : AbstractDaoCreate
    {
        private Dictionary<String, PropertyInfo> propertys;
        public CreateEntityByDatabase(String connectionStr) : base(connectionStr)
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
                CreateEntity(dirpath, item, info);
            }
        }
        private void CreateEntity(String dirpath, String tablename, IList<TableInfo> tableinfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using MySql.Data.MySqlClient;");
            sb.AppendLine();

            sb.AppendLine("namespace Entity");
            sb.AppendLine("{");
            sb.Append("    [Table(\"").Append(tablename).AppendLine("\")]");
            sb.Append("    public class ").AppendLine(tablename);
            sb.AppendLine("    {");
            foreach (var info in tableinfo)
            {
                sb.Append("        [Column(\"").Append(info.Field).Append("\"");
                sb.Append(",  MySqlDbType.").Append(GetDbType(info.Type)).Append(", LogicalName = \"");
                sb.Append(info.Field).Append("\"");
                if ("PRI".Equals(info.Key))
                {
                    sb.Append(", Key = true");
                }
                sb.AppendLine(")]");
                sb.Append("        private ").Append(GetDataType(info.Type));
                sb.Append(" ").Append(info.Field.ToLower()).AppendLine(";");
            }
            sb.AppendLine();
            foreach (var info in tableinfo)
            {
                String name = info.Field.ToLower();
                var buffer = String.Concat(name.Substring(0, 1).ToUpper(), name.Substring(1));
                sb.Append("        public ").Append(GetDataType(info.Type)).Append(" ").AppendLine(buffer);
                sb.AppendLine("        {");
                sb.Append("            get { return this.").Append(name).AppendLine("; }");
                sb.Append("            set { this.").Append(name).AppendLine(" = value; }");
                sb.AppendLine("        }");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            String filename = String.Concat(tablename, ".cs");
            base.SaveFile(Path.Combine(dirpath, filename), sb.ToString());
        }
        private String GetDbType(String type)
        {
            int pos = type.IndexOf("(");
            string b = pos == -1 ? type : type.Substring(0, pos);
            switch (b)
            {
                case "int":
                    return MySql.Data.MySqlClient.MySqlDbType.Int32.ToString();
                case "char":
                    return MySql.Data.MySqlClient.MySqlDbType.String.ToString();
                case "varchar":
                    return MySql.Data.MySqlClient.MySqlDbType.VarChar.ToString();
                case "datetime":
                    return MySql.Data.MySqlClient.MySqlDbType.DateTime.ToString();
            }
            Console.WriteLine(b);
            return null;
        }
        private String GetDataType(String type)
        {
            int pos = type.IndexOf("(");
            string b = pos == -1 ? type : type.Substring(0, pos);
            switch (b)
            {
                case "int":
                    return "int";
                case "varchar":
                case "char":
                    return "string";
                case "datetime":
                    return "DateTime?";
            }
            Console.WriteLine(b);
            return null;
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
        private IList<TableInfo> GetTableInfo(String tablename)
        {
            IList<TableInfo> ret = new List<TableInfo>();
            base.Transaction(() =>
            {
                base.ExcuteReader(
                    "show full columns from " + tablename,
                    null,
                    (dr) =>
                    {
                        var record = new TableInfo();
                        ret.Add(record);
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
                            property.SetValue(record, dr.GetString(i));
                        }
                    });
            });
            return ret;
        }
    }
}