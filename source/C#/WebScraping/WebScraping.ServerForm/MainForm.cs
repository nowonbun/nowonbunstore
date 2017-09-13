using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using Gecko.DOM;
using System.IO;
using WebScraping.WebServer;

namespace WebScraping.ServerForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));
            WebScraping.Commander.Commnader.Start();
            InitializeComponent();          
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //webBrowser1.Navigate("about:mozilla");
            webBrowser1.Navigate("http://localhost:19999/ControllView");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
