using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wx.Common.Helpers;
using System.Configuration;

namespace Wx.Web.App_Start.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        SessionHelper SessionHelper = new SessionHelper();
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            if (!actionFilter.Any())
            {
                var Request = HttpContext.Current.Request;
                var Response = HttpContext.Current.Response;
                var Session = HttpContext.Current.Session;

                if (SessionHelper["UserInfo"] == null)
                {
                    Response.Redirect("~/Home/Login");
                    return;
                }
            };
            base.OnActionExecuting(filterContext);
        }
    }

}