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
    public class GrpNfDaoTest
    {
        [TestMethod]
        public void SelectByKey()
        {
            GrpNfDao dao =  FactoryDao.Instance().GetGrpNfDao();
            IList<GrpNf> list = dao.SelectByKey("SY01");
            foreach(GrpNf l in list){
                Console.WriteLine(l);
                
            }
            Console.WriteLine("Press Any Key...");
                
        }
    }
}