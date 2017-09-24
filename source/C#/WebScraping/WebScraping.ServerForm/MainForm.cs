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
using WebScraping.Library.Log;

namespace WebScraping.ServerForm
{
    public partial class MainForm : Form
    {
        private Logger logger;
        public MainForm()
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));
            logger.Info("FireFox Dll Initialize");
            WebScraping.WebServer.WebServer.Start(19999, Path.Combine(app_dir, "WebServer"), e =>
            {

            });
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            webBrowser1.Navigate("http://localhost:19999/ControllView");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            WebScraping.WebServer.WebServer.End();
            base.OnClosing(e);
        }
    }
}
