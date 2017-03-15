using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using mshtml;

namespace ScrapCore
{
    class ScrappingBrowser : WebBrowser
    {
        private IList<String> denyTagList = new List<String>();
        private ScrappingNode node;
        private int StartingCount = 0;
        private int FixCount = 0;
        private int CompleteCount = 0;
        private bool CheckFixcount = false;
        private bool fixinit = false;
        private bool Complate = true;
        private Action Scrapping = null;
        private Action Scrapped = null;
        private Timer navigateChecker = new Timer();
        private int checkcount = 0;

        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr IpBuffer, int IpdwBufferLength);
        public ScrappingBrowser(Action Scrapping, Action Scrapped)
        {
            InternetSetOption(this.Handle, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            ScriptErrorsSuppressed = true;
            this.NewWindow += ScrappingBrowser_NewWindow;
            this.Navigating += ScrappingBrowser_Navigating;
            this.Navigated += ScrappingBrowser_Navigated;
            this.DocumentCompleted += ScrpngBrwsr_DocumentCompleted;
            this.Scrapping = Scrapping;
            this.Scrapped = Scrapped;
            this.navigateChecker.Interval = 1000;
            this.navigateChecker.Tick += navigateChecker_Tick;
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
                CheckFixcount = false;
            }
            Scrapping();
        }
        private void finalize(WebBrowser obj)
        {
            this.navigateChecker.Enabled = false;
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
            StartingCount++;
            checkcount = 0;
            if (!this.navigateChecker.Enabled)
            {
                this.navigateChecker.Enabled = true;
            }
            if (CheckFixcount)
            {
                if (FixCount <= StartingCount)
                {
                    StartingCount = FixCount;
                }
            }
        }
        void navigateChecker_Tick(object sender, EventArgs e)
        {
            checkcount++;
            if (checkcount > 10)
            {
                this.navigateChecker.Enabled = false;
                checkcount = 0;
                finalize(this);
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
            CheckFixcount = true;
            fixinit = true;
            FixCount = count;
        }
    }
}
