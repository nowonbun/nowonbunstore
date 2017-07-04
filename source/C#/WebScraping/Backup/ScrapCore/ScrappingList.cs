using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapCore
{

    class ScrappingList
    {
        private Dictionary<int, object> memory = new Dictionary<int, object>();

        public object this[int index]
        {
            get
            {
                if (memory.ContainsKey(index))
                {
                    return memory[index];
                }
                else
                {
                    return null;
                }
                
            }
            set
            {
                if (memory.ContainsKey(index))
                {
                    memory[index] = value;
                }
                else
                {
                    memory.Add(index, value);
                }
            }
        }
        public int Count()
        {
            return memory.Count;
        }
    }
}
