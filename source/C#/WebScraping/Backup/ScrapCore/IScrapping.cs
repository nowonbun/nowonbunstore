using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mshtml;

namespace ScrapCore
{
    public interface IScrapping : IDisposable
    {
        void Move(String url);
        String GetInputValueById(String id);
        void SetInputValueById(String id, String value);
        void ClickById(String id);
        String GetNodeValueById(String id);
        String GetSelectValueById(String id);
        IHTMLElement GetElementById(String id);
        bool IsElementById(String id);
        String GetInputValueByxPath(String xPath);
        void SetInputValueByxPath(String xPath, String value);
        void ClickByxPath(String xPath);
        String GetNodeValueByxPath(String xPath);
        
        String GetSelectValueByXPath(String xPath);
        
        IHTMLElement GetElementByXPath(String xPath);
        
        bool IsElementByXPath(String xPath);
        
        String GetUrl();
        void SetDocumentCount(int count);
        
        void PrintElementXPath(String file);
        
        void AddPrintDenyTagName(String tagName);
        
        void AddRangePrintDenyTagName(IList<String> tagNameList);
    }
}
