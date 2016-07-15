/*----------------------------------------------------------------
    Copyright (C) 2016 Senparc
    
    文件名：OAuth2Controller.cs
    文件功能描述：提供OAuth2.0授权测试（关注微信公众号：盛派网络小助手，点击菜单【功能体验】 【OAuth2.0授权测试】即可体验）
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using Wx.Common;

namespace Senparc.Weixin.MP.Sample.Controllers
{
	public class OAuth2Controller : Controller
	{
		//下面换成账号对应的信息，也可以放入web.config等地方方便配置和更换
		private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
		private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];

		[AllowAnonymous]
		public ActionResult Index()
		{
			//此页面引导用户点击授权
			ViewData["UrlUserInfo"] = OAuthApi.GetAuthorizeUrl(appId, "http://52052df0.nat123.net/oauth2/UserInfoCallback", "JeffreySu", OAuthScope.snsapi_userinfo);
			ViewData["UrlBase"] = OAuthApi.GetAuthorizeUrl(appId, "http://52052df0.nat123.net/oauth2/BaseCallback", "JeffreySu", OAuthScope.snsapi_base);
			return View();
		}

		/// <summary>
		/// OAuthScope.snsapi_userinfo方式回调
		/// </summary>
		/// <param name="code"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult UserInfoCallback(string code, string state, int inviteUid = 0)
		{
			if (string.IsNullOrEmpty(code))
			{
				return Content("您拒绝了授权！");
			}

			if (state != "JeffreySu")
			{
				//这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
				//实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
				return Content("验证失败！请从正规途径进入！");
			}

			OAuthAccessTokenResult result = null;

			//通过，用code换取access_token
			try
			{
				result = OAuthApi.GetAccessToken(appId, secret, code);
			}
			catch (Exception ex)
			{
				return Content(ex.Message);
			}
			if (result.errcode != ReturnCode.请求成功)
			{
				return Content("错误：" + result.errmsg);
			}
			//下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
			//如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
			Session["OAuthAccessTokenStartTime"] = DateTime.Now;
			Session["OAuthAccessToken"] = result;

			//因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
			try
			{
				OAuthUserInfo oAuthUserInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
				//userInfo.nickname += ": invite by" + inviteUid;
				//Session["OAuthUserInfo"] = userInfo;
				var ubll = new Wx.BLL.UserBLL();
				var userInfo = ubll.SaveOAuthUser(oAuthUserInfo.AsDictionary());
				Session["UserInfo"] = userInfo;
				HttpRuntime.Cache["UserInfo"] = userInfo;
				if (userInfo == null)
				{
					return Content("保存用户失败!");
				}

				//HttpRuntime.Cache["OAuthUserInfo"] = userInfo;
				var sh = new Wx.Common.Helpers.SessionHelper();
				if (Request.QueryString["redirect"] != null && sh["UserInfo"] != null)
				{
					Response.Redirect(Request.QueryString["redirect"]);
					//System.IO.File.AppendAllLines(@"D:\\log.txt", new string[] { string.Format("callback rd:{0}", Request.QueryString["redirect"]) });
				}
				return Content(oAuthUserInfo.nickname + "<br />" + oAuthUserInfo.headimgurl);
			}
			catch (ErrorJsonResultException ex)
			{
				return Content(ex.Message);
			}
		}

		/// <summary>
		/// OAuthScope.snsapi_base方式回调
		/// </summary>
		/// <param name="code"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		//[AllowAnonymous]
		//public ActionResult BaseCallback(string code, string state)
		//{
		//	if (string.IsNullOrEmpty(code))
		//	{
		//		return Content("您拒绝了授权！");
		//	}

		//	if (state != "JeffreySu")
		//	{
		//		//这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
		//		//实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
		//		return Content("验证失败！请从正规途径进入！");
		//	}

		//	//通过，用code换取access_token
		//	var result = OAuthApi.GetAccessToken(appId, secret, code);
		//	if (result.errcode != ReturnCode.请求成功)
		//	{
		//		return Content("错误：" + result.errmsg);
		//	}

		//	//下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
		//	//如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
		//	Session["OAuthAccessTokenStartTime"] = DateTime.Now;
		//	Session["OAuthAccessToken"] = result;

		//	//因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
		//	OAuthUserInfo userInfo = null;
		//	try
		//	{
		//		//已关注，可以得到详细信息
		//		userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
		//		Session["OAuthUserInfo"] = userInfo;
		//		ViewData["ByBase"] = true;
		//		if (Request.QueryString["redirect"] != null)
		//		{
		//			Response.Redirect(Request.QueryString["redirect"]);
		//		}
		//		return View("UserInfoCallback", userInfo);
		//	}
		//	catch (ErrorJsonResultException ex)
		//	{
		//		//未关注，只能授权，无法得到详细信息
		//		//这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
		//		return Content("用户已授权，授权Token：" + result);
		//	}
		//}
	}
}