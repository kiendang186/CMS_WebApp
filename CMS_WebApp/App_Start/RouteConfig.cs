using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS_WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            // blog category
            routes.MapRoute(
                name: "Blog Category",
                url: "the-loai/{metatitle}-{id}",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Controllers" }
            );

            // blog detail
            routes.MapRoute(
                name: "Blog Detail",
                url: "bai-viet/{metatitle}-{id}",
                defaults: new { controller = "Blog", action = "Detail", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Controllers" }
            );

            // contact page
            routes.MapRoute(
                name: "Contact Page",
                url: "lien-he",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Controllers" }
            );

            // team page
            routes.MapRoute(
                name: "Team Page",
                url: "co-cau-to-chuc",
                defaults: new { controller = "About", action = "Team", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Controllers" }
            );

            // emp detail
            routes.MapRoute(
                name: "Emp Detail",
                url: "co-cau-to-chuc/{metatitle}-{id}",
                defaults: new { controller = "About", action = "EmpDetail", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Controllers" }
            );            
        }
    }
}
