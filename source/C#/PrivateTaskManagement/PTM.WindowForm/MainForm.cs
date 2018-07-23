using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gecko;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PTM.WindowForm
{
    public class MainForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private String config_size = "full";
        private bool isView = false;
        private WebBrowser browser;
        private ProgressBar progress;
        private delegate void SetSizeInvoke(String size);
        private delegate void BootingInvoke();

        public MainForm(String port)
        {
            this.SuspendLayout();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Controls.Add(progress = new ProgressBar());
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progress.MarqueeAnimationSpeed = 10;
            this.progress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Size = new System.Drawing.Size(450, 80);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(browser = new WebBrowser(port));
            browser.DocumentCompleted += Browser_DocumentCompleted;
            this.ResumeLayout(false);
        }
        public void SetSize(String size)
        {
            config_size = size;
            if (!this.isView)
            {
                return;
            }
            if (InvokeRequired)
            {
                SetSizeInvoke invoke = new SetSizeInvoke(SetSizeInline);
                this.Invoke(invoke, size);
            }
            else
            {
                SetSizeInline(size);
            }
        }
        private void SetSizeInline(String size)
        {
            if (String.Equals("full", size))
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                String[] buffer = size.Split('*');
                if (buffer.Length != 2)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    try
                    {
                        int width = Convert.ToInt32(buffer[0]);
                        int height = Convert.ToInt32(buffer[1]);
                        this.Size = new System.Drawing.Size(width, height);
                        this.WindowState = FormWindowState.Normal;
                    }
                    catch
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                }
            }
        }
        private void Browser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            this.progress.Visible = false;
            this.isView = true;
            this.SetSize(this.config_size);
        }

        public void Booting()
        {
            //this.Visible = true;
            this.Show();
            //this.SetSize(this.config_size);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            //this.WindowState = FormWindowState.Minimized;
            this.Hide();
            //this.Visible = false;
            //base.OnFormClosing(e);
        }
        public void Exit()
        {
            this.Dispose();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
