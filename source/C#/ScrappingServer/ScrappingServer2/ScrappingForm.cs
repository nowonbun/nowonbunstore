using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScrappingHttpCore;
using ScrappingCore;
using ScrappingServer.ScrapClass;

namespace ScrappingServer
{
    public partial class ScrappingForm : Form
    {
        private static ScrappingForm mainform;
        private Timer timer = new Timer();
        private HttpServer webServer = null;
        private const int MAX_LOG_LENGTH = 5000;
        private IList<ScrappingStruct> scraplist = new List<ScrappingStruct>();
        private ScriptHook hook = new ScriptHook();
        private IDictionary<String, Type> ScrapKindDic = new Dictionary<String, Type>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrappingForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Form load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ScrappingForm.mainform = this;
            this.dataGridView1.ColumnCount = 6;
            this.dataGridView1.Columns[0].Name = "SHOP_CODE";
            this.dataGridView1.Columns[1].Name = "ID";
            this.dataGridView1.Columns[2].Name = "PW";
            this.dataGridView1.Columns[3].Name = "SCRAP_CODE";
            this.dataGridView1.Columns[4].Name = "START_TIME";
            this.dataGridView1.Columns[5].Name = "STATE";

            this.textBox1.Text = "10000";
            this.textBox2.Text = "Stop";
            this.textBox2.BackColor = Color.Red;

            this.comboBox1.SelectedValueChanged += comboBox1_SelectedValueChanged;
            //this.timer.Tick += timer_Tick;
            //TODO: this code is thing for test.
            this.comboBox1.Items.Add(1);
            this.comboBox1.Items.Add(30);
            this.comboBox1.Items.Add(60);
            this.comboBox1.Items.Add(90);
            //this.comboBox1.SelectedItem = 60;
            //TODO: this code is thing for test.
            this.comboBox1.SelectedItem = 1;

            ScrapKindDic.Add("001", typeof(Gmarket));
            ScrapKindDic.Add("002", typeof(Domekuk));
            ScrapKindDic.Add("006", typeof(Auction));
            ScrapKindDic.Add("999", typeof(Sample));

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            hook.Dispose();
            base.OnClosing(e);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //timer.Interval = (int)this.comboBox1.SelectedItem * 1000; //* 60;
            //timer.Enabled = true;
        }
        /// <summary>
        /// This Method is thing that clear the memory what is applied scrapping result.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            int dateInterval = (int)this.comboBox1.SelectedItem;
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                object datebuffer = row.Cells["START_TIME"].Value;
                if (datebuffer == null)
                {
                    continue;
                }
                DateTime dt = (DateTime)datebuffer;
                if (DateTime.Now.AddMinutes(dateInterval * -1).CompareTo(dt) <= 0)
                {

                    String code = (string)row.Cells["SCRAP_CODE"].Value;
                    RemoveList(code);
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String port = textBox1.Text;
            int portNumber = 10000;
            try
            {
                portNumber = Convert.ToInt32(port);
            }
            catch (FormatException)
            {
                WriteLog("This port setting is wrong.Select to number between 10000 to 20000,please.");
                this.textBox1.Text = "10000";
                return;
            }
            try
            {
                webServer = new HttpServer(portNumber);
            }
            catch (Exception)
            {
                WriteLog("This port setting is wrong.Do not overlapped the other port.");
                WriteLog("The step of confirmation) This order press prompt on commander; netstat -an |find \"LISTEN\" . Then the port check at. Finally, The port set that is not overlapped.");
                this.textBox1.Text = "10000";
                return;
            }

            button1.Enabled = false;
            textBox1.Enabled = false;
            this.textBox2.Text = "Processing";
            this.textBox2.BackColor = Color.Blue;

            webServer.Start((header) =>
            {
                if (String.IsNullOrEmpty(header["TYPE"]))
                {
                    return "PARAMETER_ERROR";
                }
                if (String.IsNullOrEmpty(header["CODE"]))
                {
                    return "PARAMETER_ERROR";
                }
                // It check code type that is defined the scrap code.
                if (!ScrapKindDic.ContainsKey(header["CODE"]))
                {
                    return "PARAMETER_ERROR";
                }
                switch ((header["TYPE"]))
                {
                    case "scrap_request":
                        {
                            if (String.IsNullOrEmpty(header["ID"]))
                            {
                                return "PARAMETER_ERROR";
                            }
                            if (String.IsNullOrEmpty(header["PW"]))
                            {
                                return "PARAMETER_ERROR";
                            }
                            if (String.IsNullOrEmpty(header["APPLY"]))
                            {
                                return "PARAMETER_ERROR";
                            }
                            String scrapcode = AdapterScrapping.Instance().RunScrapping((scrap_code, _code, _id, _pw, _param) =>
                            {
                                int gridIndex = (int)this.Invoke(new Func<int>(() =>
                                {
                                    String[] gridData = { _code, _id, _pw, scrap_code, DateTime.Now.ToString(), ScrapState.RUNNING.ToString() };
                                    return this.dataGridView1.Rows.Add(gridData);
                                }));
                                ICommonScrap scrap = FactoryScrapClass(_code, _id, _pw, _param);

                                ScrappingStruct item;
                                item.code = _code;
                                item.id = _id;
                                item.pw = _pw;
                                item.scrapcode = scrap_code;
                                scraplist.Insert(0, item);

                                //It enroll data of scrap to datagridview.
                                scrap.SetHandler((state) =>
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.dataGridView1.Rows[gridIndex].Cells[5].Value = state.ToString();
                                    }));
                                });
                                return scrap;
                            }, header["CODE"], header["ID"], header["PW"], header["APPLY"]);
                            return scrapcode;
                        }
                    case "scrap_state":
                        {
                            return AdapterScrapping.Instance().StateScrapping(header["CODE"]).ToString();
                        }
                    case "scrap_get":
                        {
                            ScrapState state = AdapterScrapping.Instance().StateScrapping(header["CODE"]);
                            if (object.Equals(state, ScrapState.COMPLETE))
                            {
                                IScrappingProcess scrap = AdapterScrapping.Instance().CompleteScrapping(header["CODE"]);
                                return scrap.ToString();
                            }
                            return state.ToString();
                        }
                    case "scrap_codesearch":
                        {
                            foreach (ScrappingStruct item in scraplist)
                            {
                                if (object.Equals(item.code, header["CODE"]) && object.Equals(item.id, header["ID"]) && object.Equals(item.pw, header["PW"]))
                                {
                                    return item.scrapcode;
                                }
                            }
                            return "NOTHING";
                        }
                    default:
                        return "TYPE_PARAMETER_ERROR";
                }
            });
        }

        public ICommonScrap FactoryScrapClass(string scrapcode, string id, string pw, string[] param)
        {
            if (ScrapKindDic.ContainsKey(scrapcode))
            {
                Type clsType = ScrapKindDic[scrapcode];
                return (ICommonScrap)Activator.CreateInstance(clsType, id, pw, param);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// This method is thing what write log.
        /// </summary>
        /// <param name="text"></param>
        public void WriteLog(String text)
        {
            this.Invoke(new Action<String>((t) =>
            {
                if (this.richTextBox1.TextLength > MAX_LOG_LENGTH)
                {
                    String buf = this.richTextBox1.Text;
                    buf.Substring(this.richTextBox1.TextLength - MAX_LOG_LENGTH, MAX_LOG_LENGTH);
                    this.richTextBox1.Text = buf;
                }
                this.richTextBox1.Focus();
                this.richTextBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd.fff") + "\t");
                this.richTextBox1.AppendText(t);
                this.richTextBox1.AppendText("\r\n");
                this.richTextBox1.ScrollToCaret();
            }), text);
        }

        public static void SetFormLog(String text)
        {
            ScrappingForm.mainform.WriteLog(text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String code = textBox4.Text;
            if (code == null)
            {
                return;
            }
            RemoveList(code);
        }
        private void RemoveList(String code)
        {
            IList<ScrappingStruct> list = (from item in scraplist where String.Equals(item.scrapcode, code) select item).ToList();

            if (list.Count < 1)
            {
                return;
            }
            //First, It remove the object in the scrappingClass 
            ScrapState state = AdapterScrapping.Instance().StateScrapping(code);
            if (object.Equals(state, ScrapState.COMPLETE))
            {
                AdapterScrapping.Instance().RemoveScrapping(code);
            }
            //Second of all, It remove the object in list of variable.
            foreach (ScrappingStruct item in list)
            {
                scraplist.Remove(item);
            }
            //Third of all, It remove the object in the data_grid_view
            IList<DataGridViewRow> removelist = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (String.Equals(code, (string)row.Cells["SCRAP_CODE"].Value))
                {
                    removelist.Add(row);
                }
            }
            foreach (DataGridViewRow row in removelist)
            {
                this.dataGridView1.Rows.Remove(row);
            }
        }
    }
}
