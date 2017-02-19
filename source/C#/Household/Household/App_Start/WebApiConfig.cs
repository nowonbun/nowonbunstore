using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Household
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // IQueryable 또는 IQueryable<T> 반환 유형을 갖는 작업에 대한 쿼리 지원을 사용하도록 설정하려면 다음 코드 행의 주석 처리를 해제하십시오.
            // 예기치 않았거나 악의적인 쿼리를 처리하지 않으려면 QueryableAttribute의 유효성 검사 설정을 사용하여 수신 쿼리가 유효한지 확인하십시오.
            // 자세한 내용은 http://go.microsoft.com/fwlink/?LinkId=279712를 참조하십시오.
            //config.EnableQuerySupport();
        }
    }
}