using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebScraping.Dao.Create
{
    class CreateDaoByDatabase : AbstractDaoCreate
    {
        public CreateDaoByDatabase(String connectionStr) : base(connectionStr)
        {

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
                CreateIDao(dirpath, item);
                CreateDao(dirpath, item);
            }
        }
        private void CreateDao(String dirpath, String tablename)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("namespace Dao");
            sb.AppendLine("{");
            sb.Append("    class ").Append(tablename).Append("Dao : Dao<").Append(tablename).Append(">, I").Append(tablename).AppendLine("Dao");
            sb.AppendLine("    {");
            sb.Append("        public IList<").Append(tablename).AppendLine("> Select()");
            sb.AppendLine("        {");
            sb.AppendLine("            return base.SelectByEntity(null);");
            sb.AppendLine("        }");
            sb.Append("        public int Insert(").Append(tablename).AppendLine(" entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            return base.InsertByEntity(entity);");
            sb.AppendLine("        }");
            sb.Append("        public int Update(").Append(tablename).AppendLine(" entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            return base.UpdateByEntity(entity);");
            sb.AppendLine("        }");
            sb.Append("        public int Delete(").Append(tablename).AppendLine(" entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            return base.DeleteByEntity(entity);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.Append("}");
            String filename = String.Concat(tablename, ".cs");
            base.SaveFile(Path.Combine(dirpath, filename), sb.ToString());
        }
        private void CreateIDao(String dirpath, String tablename)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("namespace IDao");
            sb.AppendLine("{");
            sb.Append("    [ImplementDao(\"").Append(tablename).AppendLine("Dao\")]");
            sb.Append("    public interface I").Append(tablename).AppendLine("Dao");
            sb.AppendLine("    {");
            sb.Append("        IList <").Append(tablename).AppendLine("> Select();");
            sb.AppendLine();
            sb.Append("        int Insert(").Append(tablename).AppendLine(" entity);");
            sb.AppendLine();
            sb.Append("        int Update(").Append(tablename).AppendLine(" entity);");
            sb.AppendLine();
            sb.Append("        int Delete(").Append(tablename).AppendLine(" entity);");
            sb.AppendLine("    }");
            sb.Append("}");
            String filename = String.Concat(tablename, ".cs");
            filename = String.Concat("I", filename);
            base.SaveFile(Path.Combine(dirpath, filename), sb.ToString());
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
    }
}
