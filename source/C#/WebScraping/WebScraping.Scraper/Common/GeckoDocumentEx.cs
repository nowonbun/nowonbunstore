using System;
using Gecko;
using Gecko.DOM;

namespace WebScraping.Scraper.Common
{
    public static class GeckoDocumentEx
    {
        public static T GetElementByName<T>(this GeckoDocument document, String name, int index = 0) where T : GeckoElement
        {
            return document.GetElementsByName(name)[index] as T;
        }
        public static T GetElementById<T>(this GeckoDocument document, String id) where T : GeckoElement
        {
            return document.GetElementById(id) as T;
        }
        public static T GetElementByClassName<T>(this GeckoDocument document, String classname, int index = 0) where T : GeckoElement
        {
            return document.GetElementsByClassName(classname)[index] as T;
        }
        public static T GetElementByTagName<T>(this GeckoElement element, String tag, int index = 0) where T : GeckoElement
        {
            return element.GetElementsByTagName(tag)[index] as T;
        }
        public static String GetElementByIdToNodeValue(this GeckoDocument document, String id)
        {
            return document.GetElementById(id).FirstChild.NodeValue;
        }
        public static String GetElementByTagNameToNodeValue(this GeckoElement element, String tag, int index = 0)
        {
            return element.GetElementsByTagName(tag)[index].FirstChild.NodeValue;
        }
        /*
        public static void SelectElementById<T>(this GeckoDocument document, String id, String value) where T : GeckoSelectElement
        {
            var options = document.GetElementById<T>(id).Options;
            for (int i = 0; i < options.Length; i++)
            {
                var option = options.item((uint)i);
                if (String.Equals(option.Value, value))
                {
                    document.GetElementById<GeckoSelectElement>("Account").SelectedIndex = i;
                    return;
                }
            }
        }*/
        public static ScrapTable SelectTableById(this GeckoDocument document, String id)
        {
            ScrapTable ret = new ScrapTable();
            GeckoElement element = document.GetElementById(id);
            var collection = element.GetElementsByTagName("TR");
            foreach (var row in collection)
            {
                foreach (var col in row.ChildNodes)
                {
                    if (!(col is GeckoHtmlElement))
                    {
                        continue;
                    }
                    var buffer = col as GeckoHtmlElement;
                    if ("TD".Equals(buffer.TagName) || "TH".Equals(buffer.TagName))
                    {
                        //ret.Set(col.TextContent);
                        ret.Set(col as GeckoHtmlElement);
                    }
                }
                ret.Next();
            }
            return ret;
        }
        public static ScrapTable SelectTableByClass(this GeckoDocument document, String classname, int index = 0)
        {
            ScrapTable ret = new ScrapTable();
            GeckoElement element = document.GetElementsByClassName(classname)[index] as GeckoElement;
            var collection = element.GetElementsByTagName("TR");
            foreach (var row in collection)
            {
                foreach (var col in row.ChildNodes)
                {
                    if (!(col is GeckoHtmlElement))
                    {
                        continue;
                    }
                    var buffer = col as GeckoHtmlElement;
                    if ("TD".Equals(buffer.TagName) || "TH".Equals(buffer.TagName))
                    {
                        //ret.Set(col.TextContent);
                        //ret.Set((col as GeckoHtmlElement).OuterHtml);
                        ret.Set(col as GeckoHtmlElement);
                    }
                }
                ret.Next();
            }
            return ret;
        }
    }
}
