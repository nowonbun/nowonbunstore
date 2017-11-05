using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;
using System.IO;

namespace ScrappingCore
{
    class ScrappingNode : ICloneable
    {
        private HtmlDocument doc = null;
        private IHTMLDocument mainDom = null;
        private ScrappingBrowser browser = null;
        private IList<IHTMLDocument> subFrame = new List<IHTMLDocument>();

        public ScrappingNode(ScrappingBrowser browser, HtmlDocument doc)
            : this(browser, doc.DomDocument as IHTMLDocument)
        {
            this.doc = doc;
        }

        private ScrappingNode(ScrappingBrowser browser, IHTMLDocument doc)
        {
            this.browser = browser;
            this.mainDom = doc;
            InitializeSubDocument(mainDom);
        }

        public HtmlDocument GetDocument()
        {
            return this.doc;
        }

        public IList<IHTMLDocument> GetSubDocument()
        {
            return subFrame;
        }

        public IHTMLElement GetElementByXPath(String xPath)
        {
            String[] pathList = xPath.Split('/');
            int idx = 0;
            int tagIndex = GetSplitKey(pathList[idx++]);
            IHTMLElement element;
            if (tagIndex == 0)
            {
                IHTMLDocument3 doc3 = (IHTMLDocument3)mainDom;
                element = doc3.documentElement;
            }
            else
            {
                element = ((IHTMLDocument3)subFrame[tagIndex - 1]).documentElement;
            }

            while (element != null)
            {
                tagIndex = GetSplitKey(pathList[idx]);
                if (tagIndex >= element.children.Length)
                {
                    return null;
                }
                element = element.children[tagIndex];
                idx++;
                if (pathList.Length == idx)
                {
                    break;
                }
            }
            return element;
        }

        public IHTMLElement GetElementById(String id, bool CheckSub)
        {
            IHTMLDocument3 doc3 = (IHTMLDocument3)mainDom;
            IHTMLElement ret = doc3.getElementById(id);
            if (ret == null && CheckSub)
            {
                foreach (IHTMLDocument subDoc in subFrame)
                {
                    ret = (subDoc as IHTMLDocument3).getElementById(id);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }
            return ret;
        }

        public List<IHTMLElement> GetElementByTagName(String tagname)
        {
            List<IHTMLElement> ret = new List<IHTMLElement>();
            IHTMLDocument3 doc3 = (IHTMLDocument3)mainDom;
            foreach (IHTMLElement item in doc3.getElementsByTagName(tagname))
            {
                ret.Add(item);
            }
            return ret;
        }

        public List<IHTMLElement> GetElementByName(String name)
        {
            IHTMLDocument3 doc3 = (IHTMLDocument3)mainDom;
            List<IHTMLElement> ret = new List<IHTMLElement>();
            foreach (IHTMLElement item in doc3.getElementsByName(name))
            {
                ret.Add(item);
            }
            return ret;
        }

        private int GetSplitKey(String key)
        {
            StringBuilder buffer = new StringBuilder();
            bool flag = false;
            foreach (Char t in key.ToCharArray())
            {
                if (Object.Equals('[', t))
                {
                    flag = true;
                    continue;
                }
                if (Object.Equals(']', t))
                {
                    flag = false;
                    continue;
                }
                if (flag)
                {
                    buffer.Append(t);
                }
            }
            if (flag)
            {
                throw new Exception("xPath 가 맞지 않습니다.");
            }
            return Convert.ToInt32(buffer.ToString());
        }

        public void InitializeSubDocument(IHTMLDocument document)
        {
            IHTMLDocument3 buffer = document as IHTMLDocument3;
            foreach (IHTMLElement item in buffer.getElementsByTagName("IFRAME"))
            {
                try
                {
                    IHTMLFrameBase2 ele = item as IHTMLFrameBase2;
                    IHTMLDocument subDoc = ele.contentWindow.document;
                    subFrame.Add(subDoc);
                    InitializeSubDocument(ele.contentWindow.document);
                }
                catch (UnauthorizedAccessException)
                {
                    //권한 에러는 무시
                }
            }
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        public void CreateMapPathCsv(String filename)
        {
            FileInfo fileinfo = new FileInfo(filename);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }
            using (FileStream fs = fileinfo.OpenWrite())
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamWriter sw = new StreamWriter(bs, Encoding.UTF8))
                    {
                        IHTMLDocument3 doc3 = (IHTMLDocument3)mainDom;
                        sw.WriteLine("key, number, tagname, id, classname, html");
                        WriteCSV(sw, this, doc3.documentElement, 0);

                        int index = 1;
                        IList<IHTMLDocument> subFrame = this.GetSubDocument();
                        foreach (IHTMLDocument frame in subFrame)
                        {
                            IHTMLDocument3 framedoc3 = frame as IHTMLDocument3;
                            WriteCSV(sw, this, framedoc3.documentElement, index++);
                        }
                    }
                }
            }
        }

        private void WriteCSV(StreamWriter sw, ScrappingNode list, IHTMLElement item, int index, String parentkey = "")
        {
            String tagnameIndex = String.Format("{0}[{1}]", item.tagName, index);
            IHTMLDOMNode node = item as IHTMLDOMNode;
            if (!browser.DenyTag.Contains(item.tagName))
            {
                if (node.firstChild == null || IsChildTextNode(node))
                {
                    StringBuilder sb = new StringBuilder();
                    if (parentkey != null && !String.Equals(String.Empty, parentkey))
                    {
                        sb.Append(parentkey).Append("/");
                    }
                    sb.Append(tagnameIndex).Append(",");
                    sb.Append(item.tagName).Append(",");
                    sb.Append(item.id).Append(",");
                    sb.Append(item.className).Append(",");
                    String buffer = item.outerHTML;
                    if (buffer != null)
                    {
                        buffer = buffer.Replace("\r\n", "").Replace("\n", "");
                    }
                    sb.Append(buffer);
                    sw.WriteLine(sb.ToString());
                }
            }
            int indexChild = 0;
            try
            {
                foreach (IHTMLElement subNode in item.children)
                {
                    WriteCSV(sw, list, subNode, indexChild, parentkey != null && !String.Equals(String.Empty, parentkey) ? parentkey + "/" + tagnameIndex : tagnameIndex);
                    indexChild++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private bool IsChildTextNode(IHTMLDOMNode node)
        {
            return node.firstChild != null && node.childNodes.Length == 1 && node.firstChild.nodeType == 3;
        }
    }
}
