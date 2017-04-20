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

namespace RefreshRouter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));
            InitializeComponent();
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            webBrowser1.Navigate("http://192.168.0.7");
        }

        void webBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            if (e.Uri == null)
            {
                return;
            }
            if (e.Uri.ToString().IndexOf("login.html") != -1)
            {
                GeckoDocument doc = webBrowser1.Document;
                /*GeckoInputElement element = doc.GetElementsByName("nosave_Username").Single() as GeckoInputElement;
                element.Value = "admin";*/
                GeckoInputElement element = doc.GetElementById("id_nosave_Password") as GeckoInputElement;
                if (element != null)
                {
                    element.Value = "password";
                    element = doc.GetElementById("id_login") as GeckoInputElement;
                    element.Click();
                }
            }
            if (e.Uri.ToString().IndexOf("index_pc.html") != -1)
            {
                webBrowser1.Navigate("http://192.168.0.7/init.html");
            }
            if (e.Uri.ToString().IndexOf("init.html") != -1)
            {
                GeckoDocument doc = webBrowser1.Document;
                GeckoInputElement element = doc.GetElementById("id_rebootBtn") as GeckoInputElement;
                if (element != null)
                {
                    element.Click();
                    Timer timer = new Timer();
                    timer.Interval = 1000 * 60;
                    timer.Tick += (s,a) =>
                    {
                        this.Close();
                    };
                    timer.Enabled = true;
                }
            }

        }
    }
}
