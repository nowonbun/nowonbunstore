using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrappingCore
{
    public sealed class AdapterScrapping
    {
        private static AdapterScrapping instance = null;
        private IDictionary<String, IScrappingProcess> memory = null;
        private AdapterScrapping()
        {
            memory = new Dictionary<string, IScrappingProcess>();
        }
        public static AdapterScrapping Instance()
        {
            if (instance == null)
            {
                instance = new AdapterScrapping();
            }
            return instance;
        }
        public string RunScrapping(Func<string,string,string,string,string[],IScrappingProcess> action, string code, string id, string pw, params string[] other)
        {
            string scrapcode = "SCRAP" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            IScrappingProcess scrap = action(scrapcode,code, id, pw, other);
            scrap.Run();
            memory.Add(scrapcode, scrap);
            return scrapcode;
        }
        public ScrapState StateScrapping(string code)
        {
            if (!memory.ContainsKey(code))
            {
                return ScrapState.EMPTY;
            }
            IScrappingProcess scrap = memory[code];
            return scrap.GetState();
        }
        public IScrappingProcess CompleteScrapping(String code)
        {
            ScrapState state = StateScrapping(code);
            if (Object.Equals(ScrapState.COMPLETE, state))
            {
                IScrappingProcess ret = memory[code];
                return ret;
            }
            return null;
        }
        public bool RemoveScrapping(String code)
        {
            ScrapState state = StateScrapping(code);
            if (Object.Equals(ScrapState.COMPLETE, state))
            {
                memory.Remove(code);
                return true;
            }
            return false;
        }

        public IList<String> GetKey()
        {
            return memory.Keys.ToList();
        }
    }
}
