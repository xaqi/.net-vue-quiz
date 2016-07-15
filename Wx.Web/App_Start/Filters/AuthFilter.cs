using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using Wx.Common.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using System.Configuration;

namespace Wx.Web.App_Start.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        SessionHelper SessionHelper = new SessionHelper();
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];

        bool IsTestMode()
        {
			return true;
            if (System.IO.File.Exists(@"E:\env_local.txt")) return true;
            var Request = HttpContext.Current.Request;
            var mode = Request.Cookies.Get("mode");
            if (mode != null && !string.IsNullOrWhiteSpace(mode.Value) && mode.Value == "test")
            {
                return true;
            }
            return false;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            if (!actionFilter.Any())
            {
                //filterContext.Controller.ControllerContext.HttpContext.Response.Write("FLL");
                var Request = HttpContext.Current.Request;
                var Response = HttpContext.Current.Response;
                var Session = HttpContext.Current.Session;

                if (IsTestMode() && SessionHelper["UserInfo"] == null)
                {
                    var ubll = new Wx.BLL.UserBLL();
                    SessionHelper["UserInfo"] = ubll.FromDbUser(ubll.TestUser);
                }
                else if (SessionHelper["UserInfo"] == null)
                {
                    var redirect = Request.Url.ToString();//.Replace(Request.Url.Authority, ConfigurationManager.AppSettings["UserInfoCallbackDomain"]);
                    var callback = "http://" + ConfigurationManager.AppSettings["UserInfoCallbackDomain"] + "/oauth2/UserInfoCallback/?redirect=" +
                        HttpContext.Current.Server.UrlEncode(redirect);
                    //callback += "&inviteUid=" + uid;
                    var authUrl = OAuthApi.GetAuthorizeUrl(appId, callback, "JeffreySu", OAuthScope.snsapi_userinfo);
                    //Response.Write(string.Format("<a href={0}>{1}</a>", authUrl, HttpContext.Current.Server.UrlDecode(authUrl)));
                    //System.Threading.Thread.Sleep(200);
                    var redirectCount = int.Parse((Session["RedirectCount"] ?? 0).ToString());
                    if (redirectCount > 20)
                    {
                        System.IO.File.AppendAllLines(@"D:\\log.txt", new string[] { string.Format("auth rd:{0}", Request.QueryString["redirect"]) });
                        Response.Write("Reach Max Redirect Count!");
                        return;
                    }
                    Session["RedirectCount"] = redirectCount + 1;
                    Response.Redirect(authUrl);
                    //Response.Write(authUrl);
                    return;
                }
                Session["RedirectCount"] = 0;
            };


            base.OnActionExecuting(filterContext);
        }
    }

}