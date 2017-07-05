using AisProjectCore.Dinject.Abstract;
using AisProjectCore.Dinject.Concrete;
using AisProjectCore.Domain.Abstract;
using AisProjectCore.Domain.Concrete;
using AisProjectCore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AisProjectCore
{
    public class Application : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // create DI container
            //カプセル化
            IKernel kernel = FactoryCore.getKernel();

            // binding Di container
            kernel.Binding<IRepository, MysqlRepo>();
            kernel.Binding<IAccountProc, AccountProc>();
            kernel.Binding<IReserveProc, ReserveProc>();
            kernel.Binding<ITestProc, TestProc>();

            FactoryCore.getSocket().Run();
            //initialize
            FactoryCore.getList();
        }
        protected void Application_End()
        {
            FactoryCore.getSocket().Shutdown();
        }
    }
}