using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using mshtml;
namespace ScrappingCore
{
    class ScrappingContext : ApplicationContext
    {
        private ScrappingBrowser browser = null;
        private bool navi = false;

        public ScrappingContext(Action<String> Navigated)
        {
            this.MainForm = new Form();
            browser = new ScrappingBrowser(() => { navi = true; }, () => { navi = false; Navigated(GetUrl()); });
            this.MainForm.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            this.MainForm.Load += MainForm_Load;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            form.Visible = false;
            form.ShowInTaskbar = false;
        }

        public void Navigate(String url)
        {
            navi = true;
            browser.Navigate(url);
        }
        public void isNavi()
        {
            while (navi)
            {
                Thread.Sleep(1000);
            }
        }
        public IHTMLElement GetElementById(String id)
        {
            isNavi();
            return browser.Node.GetElementById(id, true);
        }
        public IHTMLElement GetElementByXPath(String xPath)
        {
            isNavi();
            return browser.Node.GetElementByXPath(xPath);
        }
        public void PrintElementXPath(String file)
        {
            isNavi();
            browser.Node.CreateMapPathCsv(file);
        }
        public void AddPrintDenyTagName(String tagName)
        {
            browser.AddDenyTag(tagName);
        }
        public void AddRangePrintDenyTagName(IList<String> tagNameList)
        {
            foreach (String tagname in tagNameList)
            {
                AddPrintDenyTagName(tagname);
            }
        }
        public void SetDocumentCount(int count)
        {
            browser.SetDocumentCount(count);
        }
        public String GetUrl()
        {
            isNavi();
            return browser.Url.ToString();
        }
    }
}
