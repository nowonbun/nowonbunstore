using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using Gecko.DOM;

namespace WebScraping.Scraper.Common
{
    public class ScrapTable
    {
        private List<List<GeckoHtmlElement>> map = new List<List<GeckoHtmlElement>>();
        private List<GeckoHtmlElement> point;

        public ScrapTable()
        {
            Next();
        }
        public void Set(GeckoHtmlElement data)
        {
            point.Add(data);
        }
        public void Next()
        {
            map.Add(point = new List<GeckoHtmlElement>());
        }
        public GeckoHtmlElement Get(int row, int col)
        {
            if (row >= map.Count)
            {
                return null;
            }
            var buffer = map[row];
            if (col >= buffer.Count)
            {
                return null;
            }
            return buffer[col];
        }
    }
}
