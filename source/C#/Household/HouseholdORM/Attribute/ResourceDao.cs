﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdORM
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResourceDao : System.Attribute
    {
        public ResourceDao() { }
    }
}
