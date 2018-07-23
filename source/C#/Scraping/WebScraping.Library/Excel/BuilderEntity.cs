using System;
using System.Collections.Generic;
using NPOI;
using System.IO;
using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;

namespace WebScraping.Library.Excel
{
    public class BuilderExcelEntity<T> where T : class
    {
        //TODO : filesize - 0?
        //TODO : cell check.. data
        private Dictionary<int, FieldInfo> flyweight = new Dictionary<int, FieldInfo>();
        public BuilderExcelEntity() { }
        public List<T> Builder(String filepath, int startrow = 0, int startcell = 0, bool delete = false)
        {
            List<T> ret = new List<T>();
            FileInfo info = new FileInfo(filepath);
            if (info.Length == 0)
            {
                return ret;
            }
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                var wb1 = new HSSFWorkbook(file);
                ISheet sheet = wb1.GetSheetAt(0);

                for (int rowIndex = startrow; true; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row == null)
                    {
                        break;
                    }
                    T entity = null;
                    for (int cellIndex = startcell; true; cellIndex++)
                    {
                        ICell cell = row.GetCell(cellIndex);
                        if (cell == null)
                        {
                            ret.Add(entity);
                            break;
                        }
                        if (cellIndex == 0)
                        {
                            entity = Activator.CreateInstance(typeof(T)) as T;
                        }
                        if (row.GetCell(cellIndex).CellType == CellType.String)
                        {
                            SetData(entity, cellIndex, row.GetCell(cellIndex).StringCellValue);
                        }
                        else
                        {
                            SetData(entity, cellIndex, row.GetCell(cellIndex).NumericCellValue.ToString());
                        }
                    }
                }
                return ret;
            }
        }
        private void SetData(T entity, int index, String data)
        {
            var field = GetFieldInfo(index);
            if (field == null)
            {
                return;
            }
            field.SetValue(entity, data);
        }

        private FieldInfo GetFieldInfo(int index)
        {
            if (flyweight.ContainsKey(index))
            {
                return flyweight[index];
            }
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes();
                foreach (var attr in attrs)
                {
                    if (!(attr is ExcelHeader))
                    {
                        continue;
                    }
                    var temp = attr as ExcelHeader;
                    if (temp.ColumnIndex == index)
                    {
                        flyweight.Add(index, field);
                        return field;
                    }
                }
            }
            return null;
        }
    }
}
