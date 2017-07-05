using System;
using System.Collections.Generic;

namespace AisProjectCore.Dinject.Abstract
{
    public interface IStructureList
    {
        String Push(IList<IDictionary<String, Object>> data);
        String Push(IDictionary<String, Object> data);
        String Pop(String key);
    }
}
