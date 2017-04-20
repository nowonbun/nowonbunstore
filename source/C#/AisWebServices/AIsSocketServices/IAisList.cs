using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIsSocketServices
{
    public interface IAisList
    {
        String Push(IList<IDictionary<String, Object>> data);
        String Push(IDictionary<String, Object> data);
        String Pop(String key);
    }
}
