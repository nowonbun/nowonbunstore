using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ControlFactory.SetForm(this);
            QueueThread.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddCheckBoxColumn();
            AddColumn(100, "Value1");
            AddColumn(100, "Value2");
            AddColumn(100, "Value3");
            AddColumn(100, "Value4");
            AddColumn(100, "Value5");
            //dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //dataGridView1.ColumnHeadersHeight = 30;
            //dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.dataGridView1.Height = this.Size.Height - 120;
            OnStatus.Visible = false;
            OffStatus.Visible = true;
            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = false;
            toolStripStatusLabel1.Text = "";
        }

        public void SetStatusMessage(String msg)
        {
            QueueThread.Push(() =>
            {
                QueueThread.InvokeControl(statusStrip1, () =>
                {
                    OnStatus.Visible = true;
                    OffStatus.Visible = false;
                    toolStripProgressBar1.Visible = true;
                    toolStripStatusLabel1.Visible = true;
                });
            });
        }
        public void HideStatusMessage()
        {
            QueueThread.Push(() =>
            {
                QueueThread.InvokeControl(statusStrip1, () =>
                {
                    OnStatus.Visible = false;
                    OffStatus.Visible = true;
                    toolStripProgressBar1.Visible = false;
                    toolStripStatusLabel1.Visible = false;
                    toolStripStatusLabel1.Text = "";
                });
            });
        }

        private void AddCheckBoxColumn()
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.Width = 30;
            column.FalseValue = false;
            column.TrueValue = true;
            var allcheck = new CheckBox();
            allcheck.Size = new Size(15, 15);
            //location
            allcheck.Location = new Point(50, 10);
            allcheck.Click += (s, e) =>
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var c = dataGridView1.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                    c.Value = allcheck.Checked;
                }
            };
            this.dataGridView1.Controls.Add(allcheck);
            dataGridView1.Columns.Add(column);

            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 30;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }
        private void AddColumn(int width, String name, bool _readonly = false, Type type = null)
        {
            if (type == null)
            {
                type = typeof(String);
            }
            DataGridViewColumn column = new DataGridViewColumn();
            column.Width = width;
            column.ValueType = type;
            column.HeaderText = name;
            column.CellTemplate = new DataGridViewTextBoxCell();
            column.ReadOnly = _readonly;
            dataGridView1.Columns.Add(column);
        }
        public DataGridView GetGridData()
        {
            return dataGridView1;
        }
    }
}
