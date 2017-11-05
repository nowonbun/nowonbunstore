using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrappingCore
{
    
    class ScrappingList
    {
        private Dictionary<int, object> memory = new Dictionary<int, object>();

        public object this[int index]
        {
            get { return memory[index]; }
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
    }
}
