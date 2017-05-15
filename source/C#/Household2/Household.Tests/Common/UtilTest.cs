using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Household;
using Household.Controllers;
using Household.Models.Entity;
using Household.Common;

namespace Household.Tests.Common
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void MD5HashCrypt()
        {
            String val = Util.MD5HashCrypt("ghkdtnsduq1");
            Assert.AreEqual("718d81708bbc1d59dd4148189602e3b2", val);
        }
    }
}