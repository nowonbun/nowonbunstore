using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using mshtml;

namespace ScrappingCore
{
    class Scrapping : IScrapping
    {
        private ScrappingContext conn = null;
        private Thread thread = null;
        public Scrapping(Action<String> Navigated)
        {
            thread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(conn = new ScrappingContext((url) => { Navigated(url); }));
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        public void Dispose()
        {
            IsInstance();
            thread.Abort();
        }
        private void IsInstance()
        {
            while (conn == null)
            {
                Thread.Sleep(1000);
            }
            conn.isNavi();
        }
        private ScrappingContext Connection
        {
            get
            {
                IsInstance();
                return conn;
            }
        }
        public void Move(String url)
        {
            Connection.Navigate(url);
        }
        public String GetInputValueById(String id)
        {
            return ((IHTMLInputElement)GetElementById(id)).value;
        }
        public String GetSelectValueById(String id)
        {
            IHTMLSelectElement element = (IHTMLSelectElement)GetElementById(id);
            IHTMLOptionElement option = element.options[element.selectedIndex];
            return option.value;
        }
        public void SetInputValueById(String id, String value)
        {
            ((IHTMLInputElement)GetElementById(id)).value = value;
        }
        public void ClickById(String id)
        {
            GetElementById(id).click();
        }
        public String GetNodeValueById(String id)
        {
            foreach (IHTMLDOMNode sub in ((IHTMLDOMNode)GetElementById(id)).childNodes)
            {
                if (sub.nodeType == 3)
                {
                    return sub.nodeValue;
                }
            }
            return null;
        }
        public IHTMLElement GetElementById(String id)
        {
            return Connection.GetElementById(id);
        }
        public bool IsElementById(String id)
        {
            return Connection.GetElementById(id) != null;
        }
        public String GetInputValueByxPath(String xPath)
        {
            return ((IHTMLInputElement)GetElementByXPath(xPath)).value;
        }
        public String GetSelectValueByXPath(String xPath)
        {
            IHTMLSelectElement element = (IHTMLSelectElement)GetElementByXPath(xPath);
            IHTMLOptionElement option = element.options[element.selectedIndex];
            return option.value;
        }
        public void SetInputValueByxPath(String xPath, String value)
        {
            ((IHTMLInputElement)GetElementByXPath(xPath)).value = value;
        }
        public void ClickByxPath(String xPath)
        {
            GetElementByXPath(xPath).click();
        }
        public String GetNodeValueByxPath(String xPath)
        {
            foreach (IHTMLDOMNode sub in ((IHTMLDOMNode)GetElementByXPath(xPath)).childNodes)
            {
                if (sub.nodeType == 3)
                {
                    return sub.nodeValue;
                }
            }
            return null;
        }
        public IHTMLElement GetElementByXPath(String xPath)
        {
            return Connection.GetElementByXPath(xPath);
        }
        public bool IsElementByXPath(String xPath)
        {
            return Connection.GetElementByXPath(xPath) != null;
        }
        public String GetUrl()
        {
            return Connection.GetUrl();
        }
        public void PrintElementXPath(String file)
        {
            Connection.PrintElementXPath(file);
        }
        public void AddPrintDenyTagName(String tagName)
        {
            Connection.AddPrintDenyTagName(tagName);
        }
        public void AddRangePrintDenyTagName(IList<String> tagNameList)
        {
            Connection.AddRangePrintDenyTagName(tagNameList);
        }
        public void SetDocumentCount(int count)
        {
            Connection.SetDocumentCount(count);
        }
    }
}
