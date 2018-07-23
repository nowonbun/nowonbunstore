using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gecko;
using WebScraping.Scraper.Common;

namespace WebScraping.Scraper.Common
{
    public class FlowList : List<Flow>
    {
        public List<String> UrlList
        {
            get
            {
                return this.Select(x => x.Url).ToList();
            }
        }
        public void Add(String url, Action<FlowModelData> func, String next, Action<FlowModelData> callback = null)
        {
            base.Add(new Flow
            {
                Url = url,
                Func = func,
                Next = next,
                Callback = callback
            });
        }
        public Flow this[string key]
        {
            get
            {
                return this.Where(x => String.Equals(x.Url, key)).FirstOrDefault();
            }
            set
            {
                int i = 0;
                for (i = 0; i < Count; i++)
                {
                    if (String.Equals(base[i].Url, key))
                    {
                        base[i] = value;
                        return;
                    }
                }
                value.Url = key;
                base.Add(value);
            }
        }
    }
}
