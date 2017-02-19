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
using Household.Models.Master;

namespace Household.Tests.Dao
{
    [TestClass]
    public class HshldDaoTest
    {
        [TestMethod]
        public void InsertToInfo()
        {
            HshldDao dao = FactoryDao.Instance().GetHshldDao();
            String cd = FactoryMaster.Instance().GetCategoryMaster().GetAll().First().Cd;
            String tp = FactoryMaster.Instance().GetTypeMaster().GetByCategoryCode(cd).First().TP;
            dao.InsertToInfo("TEST1", "tester", cd, tp, DateTime.Now.ToString("yyyy-MM-dd"), "TEST", "1");
            Console.WriteLine("Press Any Key...");
        }
        [TestMethod]
        public void SelectToInfoByDate()
        {
            HshldDao dao = FactoryDao.Instance().GetHshldDao();
            IList<Hshld> list = dao.SelectToInfoByDate("TEST1", DateTime.Now);
            Console.WriteLine("Press Any Key...");
        }
    }
}