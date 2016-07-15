using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Wx.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "weixin",
                url: "weixin/{action}/",
                defaults: new { action = "Index", controller = "Weixin" },
                namespaces: new string[] { "Senparc.Weixin.MP.Sample.Controllers" }
            );
            routes.MapRoute(
                name: "invite",
                url: "invite/{userId}/",
                defaults: new { action = "Index", controller = "Invite" },
                namespaces: new string[] { "Senparc.Weixin.MP.Sample.Controllers" }
            );
            routes.MapRoute(
                name: "public",
                url: "public/login/",
                defaults: new { controller = "Home", action = "Public_Login" },
                namespaces: new string[] { "Wx.Web.Controllers" }
            );
            routes.MapRoute(
                name: "lol",
                url: "lol/{seg1}/{seg2}/{seg3}",
                defaults: new { controller = "Home", action = "GameIndex", seg1 = UrlParameter.Optional, seg2 = UrlParameter.Optional, seg3 = UrlParameter.Optional },
                namespaces: new string[] { "Wx.Web.Controllers" }
            );
            routes.MapRoute(
                name: "dota",
                url: "dota/{seg1}/{seg2}/{seg3}",
                defaults: new { controller = "Home", action = "GameIndex", seg1 = UrlParameter.Optional, seg2 = UrlParameter.Optional, seg3 = UrlParameter.Optional },
                namespaces: new string[] { "Wx.Web.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { action = "Index", controller = "Home" },
                namespaces: new string[] { "Wx.Web.Controllers", "Senparc.Weixin.MP.Sample.Controllers" }
            );
        }
    }
}
