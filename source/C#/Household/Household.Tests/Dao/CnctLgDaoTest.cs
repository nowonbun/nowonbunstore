using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Household;
using Household.Controllers;
using Household.Models.Entity;
using Household.Dao;

namespace Household.Tests.Dao
{
    [TestClass]
    public class CnctLgDaoTest
    {
        [TestMethod]
        public void InsertToSignin()
        {
            CnctLgDao dao = FactoryDao.Instance().GetCnctLgDao();
            dao.InsertToSignin("SY01", "nowonbun");
            Console.WriteLine("Press Any Key...");
                
        }
    }
}