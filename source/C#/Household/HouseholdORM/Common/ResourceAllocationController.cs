﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HouseholdORM
{
    public abstract class ResourceAllocationController : System.Web.Mvc.Controller
    {
        public ResourceAllocationController()
        {
            var fields = from field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         let attrs = field.GetCustomAttributes()
                         from attr in attrs
                         where attr is ResourceDao
                         select field;

            Parallel.ForEach(fields, field =>
            {
                ImplementDao impl = field.FieldType.GetCustomAttribute(typeof(ImplementDao)) as ImplementDao;
                field.SetValue(this, FactoryDao.GetInstance().GetDao(impl.ClassName));
            });
        }
    }
}
