using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;
using Wx.Common.Helpers;
using System.IO;

namespace Senparc.Weixin.MP.Sample.Controllers
{
	public class LoginController : BaseController
	{
		private string appId = ConfigurationManager.AppSettings["TenPayV3_AppId"];
		[AllowAnonymous]
		public ActionResult InviteQrCode(int uid = 0)
		{
			var url = string.Format("http://" + Request.Url.Host + "/login/reg?uid=" + uid);
			using (var ms = new MemoryStream())
			{
				QRCodeHelper.GetQRCode(url, ms);
				Response.ContentType = "image/Png";
				Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
				Response.End();
				return null;
			}
		}


		[AllowAnonymous]
		[ActionName("Index")]
		public ActionResult Index(string gametypename = "lol", int uid = 0)
		{
			if (Session["UserInfo"] == null)
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
			var userinfo = (Wx.BLL.UserInfo)Session["UserInfo"];

			return Content(string.Format("Hello {0}.", userinfo.name));
		}

		[AllowAnonymous]
		public ActionResult ClearSession()
		{
			Session["UserInfo"] = null;
			HttpRuntime.Cache.Remove("UserInfo");
			return Content("session cleared.");
		}
		[AllowAnonymous]
		public ActionResult Logout()
		{
			return ClearSession();
		}

	}
}
