using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Household;
using Household.Controllers;
using Household.Models.Bean;
using Household.Common;
using Household.Models.Entity;
using Moq;

namespace Household.Tests.Controllers
{
    [TestClass]
    public class AjaxControllerTest 
    {
        [TestMethod]
        public void Search()
        {
            // Arrange
            UsrNf userinfo = new UsrNf();
            userinfo.Usrd = "tester";
            userinfo.Grpd = "TEST1";

            var contextMock = new Mock<ControllerContext>();
            var mockHttpContext = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();

            mockHttpContext.Setup(ctx => ctx.Session).Returns(session.Object);
            contextMock.Setup(ctx => ctx.HttpContext).Returns(mockHttpContext.Object);

            contextMock.SetupGet(p => p.HttpContext.Session[Define.USER_SESSION_NAME]).Returns(userinfo);

            AjaxController controller = new AjaxController();
            controller.ControllerContext = contextMock.Object;
            
            SearchBean bean = new SearchBean();
            bean.Year = "2017";
            bean.Month = "01";
            JsonResult result = controller.Search(bean) as JsonResult;

            Console.WriteLine("OK");

            // Assert
            //Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }
    }
}
