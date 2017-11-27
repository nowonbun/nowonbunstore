using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gecko;
using System.IO;
using WebScraping.Library.Log;
using WebScraping.Library.Config;
using System.Collections.Generic;
using System.Linq;

namespace WebScraping.ServerForm
{
    public partial class MainForm : Form
    {
        private Logger logger;
        private ScriptHook hook;

        public MainForm()
        {
            logger = LoggerBuilder.Init().Set(this.GetType());
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));
            logger.Info("FireFox Dll Initialize");
            InitializeComponent();
            InitializeFlow();

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            hook = new ScriptHook();
            webBrowser1.Navigate("http://localhost:" + ServerInfo.GetPort() + "/ControllView");
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            hook.Dispose();
            base.OnClosing(e);
        }
    }
}
