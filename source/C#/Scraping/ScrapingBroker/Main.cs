using System;
using System.ComponentModel;
using System.Windows.Forms;
using WebScraping.Library.Config;
using Newtonsoft.Json;
using WebScraping.Library.Log;

namespace WebScraping.Broker
{
    public partial class Main : Form
    {
        private Logger logger = null;

        public Main()
        {
            logger = LoggerBuilder.Init().Set(GetType()).Info("Broker Program Start");
            ControlFactory.SetForm(this);
            InitializeComponent();
            SetConnection(false);
            SetMessage(false);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ServerConnector.Instance().OnReceive += Main_OnReceive;
            AddColumn(100, "Key");
            AddColumn(100, "MallCD");
            AddColumn(100, "Id1");
            AddColumn(100, "Id2");
            AddColumn(100, "Id3");
            AddColumn(100, "Option1");
            AddColumn(100, "Option2");
            AddColumn(100, "Option3");
            AddColumn(100, "Sdate");
            AddColumn(100, "Edate");
            AddColumn(100, "Exec");
            AddColumn(100, "ScrapType");
            AddColumn(100, "Starttime");
            AddColumn(100, "Pingtime");
            AddColumn(100, "state");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            logger.Info("This program will be exit.");
            QueueThread.Abort();
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

        private void Main_OnReceive(string msg)
        {
            if (String.Equals("PING", msg.ToUpper()))
            {
                ServerConnector.Instance().Send("PONG");
                return;
            }
            else if (String.IsNullOrEmpty(msg))
            {
                return;
            }
            try
            {
                Parameter node = JsonConvert.DeserializeObject<Parameter>(msg);
                logger.Info(" [WEB LOG] : Message" + node.ToJson());
                logger.Info(" [SCRAP LOG] Scraper call!");
                ScrapListenner.Instance().SetExecuter(new ScrapExecutor(node).Run());
            }
            catch(Exception e)
            {
                logger.Error(e.ToString());
            }
        }

        public void SetConnection(bool active)
        {
            QueueThread.InvokeControl(statusStrip1, () =>
            {
                if (active)
                {
                    onStatus.Visible = true;
                    offStatus.Visible = false;
                    toolStripStatusLabel3.Text = "Connected";
                }
                else
                {
                    onStatus.Visible = false;
                    offStatus.Visible = true;
                    toolStripStatusLabel3.Text = "DIsconnected";
                }
            });
        }
        public void SetMessage(String msg)
        {
            QueueThread.InvokeControl(statusStrip1, () =>
            {
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel4.Text = msg;
            });
        }
        public void SetMessage(bool active)
        {
            QueueThread.InvokeControl(statusStrip1, () =>
            {
                toolStripStatusLabel4.Text = "";
                toolStripProgressBar1.Visible = active;
            });
        }
        public void SetGrid(Parameter param)
        {
            QueueThread.InvokeControl(dataGridView1, () =>
            {
                dataGridView1.Rows.Add(param.Key,
                                        param.MallCD,
                                        param.Id1,
                                        param.Id2,
                                        param.Id3,
                                        param.Option1,
                                        param.Option2,
                                        param.Option3,
                                        param.Sdate,
                                        param.Edate,
                                        param.Exec,
                                        param.ScrapType,
                                        Convert.ToDateTime(param.Starttime).ToString("yyyyMMdd"),
                                        Convert.ToDateTime(param.Pingtime).ToString("yyyyMMdd"),
                                        param.State
                                        );
            });

        }
        public void RemoveGrid(Parameter param)
        {
            QueueThread.InvokeControl(dataGridView1, () =>
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (String.Equals(dataGridView1.Rows[i].Cells[0].Value, param.Key))
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        break;
                    }
                }

            });
        }
    }
}
