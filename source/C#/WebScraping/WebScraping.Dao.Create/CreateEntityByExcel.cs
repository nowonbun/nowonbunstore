using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Library.Excel;
using System.IO;

namespace WebScraping.Dao.Create
{
    class CreateEntityByExcel
    {
        class ExcelTemplate
        {
            [ExcelHeader("",0)]
            public String titleName;
            [ExcelHeader("", 1)]
            public String variable;
        }
        public CreateEntityByExcel(String excelpath, String output, String className, String namespaceName)
        {
            int index = 0;
            IList<ExcelTemplate> list = ReadExcel(excelpath);
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("using System;");
            buffer.AppendLine("using WebScraping.Library.Excel;");
            buffer.AppendLine();
            buffer.Append("namespace ").AppendLine(namespaceName);
            buffer.AppendLine("{");
            buffer.Append(" ").Append("class ").AppendLine(className);
            buffer.Append(" ").AppendLine("{");
            foreach(var item in list)
            {
                buffer.Append(" ").Append(" ").Append("[ExcelHeader(\"").Append(item.titleName).Append("\", ").Append(index).AppendLine(")]") ;
                buffer.Append(" ").Append(" ").Append("private String ").Append(item.variable).AppendLine(";");
                buffer.AppendLine();
                buffer.Append(" ").Append(" ").Append("public String ").AppendLine(Trans(item.variable));
                buffer.Append(" ").Append(" ").AppendLine("{");
                buffer.Append(" ").Append(" ").Append(" ").Append("get { return this.").Append(item.variable).Append("; }").AppendLine();
                buffer.Append(" ").Append(" ").AppendLine("}");
                buffer.AppendLine();
            }
            buffer.Append(" ").AppendLine("}");
            buffer.AppendLine("}");
            using (FileStream stream = new FileStream(output + "\\" + className + ".cs", FileMode.Create, FileAccess.Write))
            {
                byte[] temp = Encoding.UTF8.GetBytes(buffer.ToString());
                stream.Write(temp, 0, temp.Length);
            }
            
        }
        private String Trans(String a)
        {
            return a.Substring(0, 1).ToUpper() + a.Substring(1);
        }
        private IList<ExcelTemplate> ReadExcel(String excelpath)
        {
            BuilderExcelEntity<ExcelTemplate> builder = new BuilderExcelEntity<ExcelTemplate>();
            return builder.Builder(excelpath, false);
        }
    }
}
