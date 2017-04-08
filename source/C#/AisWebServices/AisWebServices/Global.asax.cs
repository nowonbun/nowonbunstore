using System;
using AIsSocketServices;
using AisWebServicesDao;

namespace AisWebServices
{

    public class AisApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FactoryDao.CreateInstance(System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            AisSocketServices.GetInstance().Run();
        }
    }
}