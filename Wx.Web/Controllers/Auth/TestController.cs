using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace Senparc.Weixin.MP.Sample.Controllers
{
	public class TestController : Controller
	{
		public bool IsWx()
		{
			//Wechat
			return Request.Headers["User-Agent"].Contains("Wechat");
		}
		//
		// GET: /UserInfo/


		private string appId = ConfigurationManager.AppSettings["TenPayV3_AppId"];
		[ActionName("Index")]
		public ActionResult Index(string gametypename = "lol", int uid = 0)
		{
			Response.Write(IsWx());
			if (Session["OAuthUserInfo"] == null && HttpRuntime.Cache["OAuthUserInfo"] == null)
			{
				var redirect = Request.Url.ToString().Replace(Request.Url.Authority, ConfigurationManager.AppSettings["UserInfoCallbackDomain"]);
				var callback = "http://" + ConfigurationManager.AppSettings["UserInfoCallbackDomain"] + "/oauth2/UserInfoCallback/?redirect=" + Server.UrlEncode(redirect);
				callback += "&inviteUid=" + uid;
				var authUrl = OAuthApi.GetAuthorizeUrl(appId, callback, "JeffreySu", OAuthScope.snsapi_userinfo);
				ViewBag.redUrl = authUrl;
				return View();
				//Response.Redirect(OAuthApi.GetAuthorizeUrl(appId, redUrl, "JeffreySu", OAuthScope.snsapi_userinfo));
				return null;
			}
			var userinfo = (OAuthUserInfo)(Session["OAuthUserInfo"] ?? HttpRuntime.Cache["OAuthUserInfo"]);

			return Content(string.Format("Hello {0}.", userinfo.nickname));
		}

		public ActionResult ClearSession()
		{
			Session["UserInfo"] = null;
			//HttpRuntime.Cache.Remove("UserInfo");
			return Content("session cleared.");
		}

	}
}
