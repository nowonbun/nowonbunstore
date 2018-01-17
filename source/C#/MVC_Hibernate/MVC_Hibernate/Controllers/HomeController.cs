using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Hibernate.Bean;
using MVC_Hibernate.Dao;
using MVC_Hibernate.Entity;

namespace MVC_Hibernate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session.Clear();
            return View("Index");
        }
        [HttpPost]
        public ActionResult Login(LoginBean bean)
        {
            Usertable user = FactoryDao.GetUsertableDao().SelectId(bean.id, bean.password);
            if (user == null)
            {
                return Index();
            }
            Session[Define.USER_SESSION_NAME] = user;
            return Main();
        }
        [MVC_Hibernate.Filter.AuthorizeFilter]
        public ActionResult Main()
        {
            return View("Main");
        }
    }
}
