using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Wx.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Login",
                url: "Home/Login",
                defaults: new { controller = "Home", action = "Login" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "Home/Logout",
                defaults: new { controller = "Home", action = "Logout" }
            );
            routes.MapRoute(
                name: "Sample",
                url: "Sample/{seg1}/{seg2}/{id}",
                defaults: new { controller = "Home", action = "Sample", seg1 = UrlParameter.Optional, seg2 = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{seg1}/{seg2}/{id}",
                defaults: new { controller = "Home", action = "Index", seg1 = UrlParameter.Optional, seg2 = UrlParameter.Optional, id = UrlParameter.Optional }
            );
        }
    }
}
