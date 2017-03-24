using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;
using System.IO;

namespace ScrapCore
{
    class ScrapNode : ICloneable
    {
        private IHTMLDocument document = null;
        private IList<IHTMLDocument> subFrameDocument = new List<IHTMLDocument>();

        public ScrapNode(IHTMLDocument document)
        {
            this.document = document;
            InitializeSubDocument(document);
        }

        private void InitializeSubDocument(IHTMLDocument document)
        {
            foreach (IHTMLElement item in ((IHTMLDocument3)document).getElementsByTagName("IFRAME"))
            {
                try
                {
                    subFrameDocument.Add(((IHTMLFrameBase2)item).contentWindow.document);
                    InitializeSubDocument(((IHTMLFrameBase2)item).contentWindow.document);
                }
                catch (UnauthorizedAccessException) { }
            }
        }

        public IHTMLDocument Document
        {
            get { return document; }
        }

        public IList<IHTMLDocument> SubFrameDocument
        {
            get { return subFrameDocument.ToList(); }
        }

        public IHTMLElement GetElementByXPath(String xPath)
        {
            String[] path = xPath.Split('/');
            int dept = 0;
            int idx = GetSplitKey(path[dept++]);
            IHTMLElement element;
            if (idx == 0)
            {
                IHTMLDocument3 doc3 = (IHTMLDocument3)document;
                element = doc3.documentElement;
            }
            else
            {
                element = ((IHTMLDocument3)subFrameDocument[idx - 1]).documentElement;
            }

            while (element != null)
            {
                idx = GetSplitKey(path[dept]);
                if (idx >= element.children.Length)
                {
                    return null;
                }
                element = element.children[idx];
                dept++;
                if (path.Length == dept)
                {
                    return element;
                }
            }
            return null;
        }

        private int GetSplitKey(String key)
        {
            int spos = key.IndexOf("[") + 1;
            int epos = key.LastIndexOf("]");
            if (spos < 0 || epos < 0)
            {
                throw new Exception("xPath 가 맞지 않습니다.");
            }
            try
            {
                return Convert.ToInt32(key.Substring(spos, epos - spos));
            }
            catch (FormatException)
            {
                throw new FormatException("xPath 가 맞지 않습니다.");
            }
        }

        public IHTMLElement GetElementById(String id)
        {
            IHTMLElement ret = ((IHTMLDocument3)document).getElementById(id);
            if (ret != null)
            {
                return ret;
            }
            foreach (IHTMLDocument sub in subFrameDocument)
            {
                ret = ((IHTMLDocument3)sub).getElementById(id);
                if (ret != null)
                {
                    return ret;
                }
            }
            return null;
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
