using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Model;
using System.IO;
using Ionic.Zip;

namespace WindowExample
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            ControlFactory.SetControl(this);
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            button1.Click += Button1_Click;
        }
        private void EnableControl(bool enable)
        {
            QueueThread.InvokeControl(this, () =>
            {
                button1.Enabled = enable;
                button2.Enabled = enable;
            });
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            QueueThread.Push(() =>
            {
                EnableControl(false);
                ControlFactory.GetForm<Form1>().SetStatusMessage("The excel is started to create.");
                DataGridView grid = ControlFactory.GetForm<Form1>().GetGridData();
                QueueThread.InvokeControl(grid, () =>
                {
                    grid.Enabled = false;
                });
                //Create workbook
                IWorkbook book = new XSSFWorkbook();
                //Create font
                IFont boldFont = book.CreateFont();
                boldFont.Boldweight = (short)FontBoldWeight.Bold;
                ICellStyle boldStyle = book.CreateCellStyle();
                boldStyle.SetFont(boldFont);
                boldStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;

                int rowidx = 0;
                //Create sheet;
                ISheet sheet = book.CreateSheet("TEST");
                //Header
                IRow row = sheet.CreateRow(rowidx++);
                for (int j = 1; j < grid.ColumnCount; j++)
                {
                    String data = grid.Columns[j].HeaderText;
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(data);
                    cell.CellStyle = boldStyle;
                }

                //Contents
                for (int i = 0; i < grid.RowCount; i++)
                {
                    DataGridViewRow gridrow = grid.Rows[i];
                    DataGridViewCheckBoxCell checkcehll = gridrow.Cells[0] as DataGridViewCheckBoxCell;
                    if (checkcehll.Value == null || !((bool)checkcehll.Value))
                    {
                        continue;
                    }
                    row = sheet.CreateRow(rowidx++);
                    for (int j = 1; j < grid.ColumnCount; j++)
                    {
                        String data = grid[j, i].Value.ToString();
                        ICell cell = row.CreateCell(j);
                        cell.SetCellValue(data);
                    }
                }

                ControlFactory.GetForm<Form1>().SetStatusMessage("The excel is be creating.");
                //Create excel
                using (FileStream stream = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\TEST.xlsx", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    book.Write(stream);
                }

                ControlFactory.GetForm<Form1>().SetStatusMessage("The excel is be compressing.");
                //compress zip
                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < 1; i++)
                    {
                        using (MemoryStream memory = new MemoryStream())
                        {
                            book.Write(memory);
                            byte[] data = memory.ToArray();
                            zip.AddEntry("Test" + i + ".xlsx", data);
                        }
                    }

                    zip.Save(Path.GetDirectoryName(Application.ExecutablePath) + "\\TEST.zip");
                }

                ControlFactory.GetForm<Form1>().HideStatusMessage();
                QueueThread.InvokeControl(grid, () =>
                {
                    grid.Enabled = true;
                });
                EnableControl(true);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QueueThread.Push(() =>
            {
                EnableControl(false);
                ControlFactory.GetForm<Form1>().SetStatusMessage("The data is started to insert to the grid.");
                DataGridView grid = ControlFactory.GetForm<Form1>().GetGridData();
                QueueThread.InvokeControl(grid, () =>
                {
                    grid.Enabled = false;
                });
                for (int i = 0; i < 10000; i++)
                {
                    QueueThread.InvokeControl(grid, () =>
                    {
                        grid.Rows.Add(true, "TEST1_" + i, "TEST2_" + i, "TEST3_" + i, "TEST4_" + i, "TEST5_" + i);
                    });
                }
                QueueThread.InvokeControl(grid, () =>
                {
                    grid.Enabled = true;
                });
                ControlFactory.GetForm<Form1>().HideStatusMessage();
                EnableControl(true);
            });

        }
    }
}
