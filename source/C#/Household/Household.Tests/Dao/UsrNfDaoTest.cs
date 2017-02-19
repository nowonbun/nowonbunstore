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
    public class UsrNfDaoTest
    {
        [TestMethod]
        public void SelectForSign()
        {
            UsrNfDao dao = FactoryDao.Instance().GetUsrNfDao();
            UsrNf ret = dao.SelectForSign("SY01", "nowonbun", "ghkdtsnduq1");
            Assert.AreEqual(ret.Nm,"SoonYub Hwang");
            ret = dao.SelectForSign("SY01", "nowonbun", "ghkdtsnd");
            Assert.AreEqual(ret.Nm,null);

        }
    }
}