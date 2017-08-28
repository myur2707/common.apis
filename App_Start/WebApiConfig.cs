using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
//using System.Web.HttpApplication;
using System.Web.Routing;

namespace WebApiCampusConcierge
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CCApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            RouteTable.Routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionStateRouteHandler();
        }
    }
}
