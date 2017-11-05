using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using mshtml;

namespace ScrappingCore
{
    class ScrappingBrowser : WebBrowser
    {
        private IList<String> denyTagList = new List<String>();
        private ScrappingNode node;
        private int StartingCount = 0;
        private int CompleteCount = 0;
        private bool fixcount = false;
        private bool fixinit = false;
        private bool Complate = true;
        private Action Scrapping = null;
        private Action Scrapped = null;

        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr IpBuffer, int IpdwBufferLength);
        public ScrappingBrowser(Action Scrapping, Action Scrapped)
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            ScriptErrorsSuppressed = true;
            this.NewWindow += ScrappingBrowser_NewWindow;
            this.Navigating += ScrappingBrowser_Navigating;
            this.Navigated += ScrappingBrowser_Navigated;
            this.DocumentCompleted += ScrpngBrwsr_DocumentCompleted;
            this.Scrapping = Scrapping;
            this.Scrapped = Scrapped;
        }

        void ScrappingBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        public ScrappingNode Node
        {
            get { return node; }
        }
        public IList<String> DenyTag
        {
            get { return denyTagList; }
        }
        public void AddDenyTag(String tagname)
        {
            denyTagList.Add(tagname);
        }
        public new void Navigate(string urlString)
        {
            Initialize();
            base.Navigate(urlString);
        }
        private void Initialize()
        {   
            CompleteCount = 0;
            Complate = false;
            if (fixinit)
            {
                fixinit = false;
            }
            else
            {
                StartingCount = 0;
                fixcount = false;
            }
            Scrapping();
        }
        private void finalize(WebBrowser obj)
        {
            node = new ScrappingNode(this, obj.Document);
            Complate = true;
            Scrapped();
        }
        void ScrappingBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            WebBrowser obj = (WebBrowser)sender;
            if (Complate)
            {
                Initialize();
            }
            Console.WriteLine("NAVIGATING : " + e.Url);
            if (!fixcount)
            {
                StartingCount++;
            }
        }
        void ScrappingBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser obj = (WebBrowser)sender;
        }
        void ScrpngBrwsr_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            CompleteCount++;
            Console.WriteLine("COMPLATE : " + e.Url);
            Console.WriteLine(CompleteCount + " / " + StartingCount);
            if (StartingCount != CompleteCount)
            {
                return;
            }
            finalize((WebBrowser)sender);
        }
        public void SetDocumentCount(int count)
        {
            fixcount = true;
            fixinit = true;
            StartingCount = count;
        }
    }
}
