using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StuMSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "SearchStudent",
               url: "students/Search",
               defaults: new { controller = "students", action = "Search" }
           );

            // Thêm route cho studentsController
            routes.MapRoute(
                name: "CreateStudent",
                url: "students/Create",
                defaults: new { controller = "students", action = "Create" }
            );

        }
    }
}
