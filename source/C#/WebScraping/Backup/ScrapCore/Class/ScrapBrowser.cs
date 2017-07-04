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
    class ScrapBrowser : WebBrowser
    {
        class StateNode
        {
            private IList<Uri> connect = new List<Uri>();
            public void Before(Uri uri)
            {
                connect.Add(uri);
            }
            public void After(Uri uri)
            {
                connect.Remove(uri);
            }
            public bool IsComplete()
            {
                return connect.Count == 0;
            }
        }
        private ScrappingNode node;
        private StateNode state = null;

        public ScrapBrowser()
        {
            //InternetSetOption(this.Handle, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            ScriptErrorsSuppressed = true;
            this.NewWindow += (s, e) => { e.Cancel = true; };
            this.Navigating += Start;
            this.Navigated += End;
            this.DocumentCompleted += Complate;
        }

        public ScrappingNode Node
        {
            get { return node; }
        }

        public new void Navigate(string urlString)
        {
            Initialize();
            base.Navigate(urlString);
        }

        private void Initialize()
        {
            state = new StateNode();
        }

        private void Finalize(WebBrowser obj)
        {
            state = null;
        }

        void Start(object sender, WebBrowserNavigatingEventArgs e)
        {
            state.Before(e.Url);
        }

        void End(object sender, WebBrowserNavigatedEventArgs e)
        {
            state.After(e.Url);
        }

        void Complate(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (state.IsComplete())
            {
                Finalize(this);
            }
        }
    }
}
