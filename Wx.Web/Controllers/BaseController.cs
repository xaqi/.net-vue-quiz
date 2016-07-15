using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;
using Wx.Common.Helpers;
using Wx.BLL;
using Wx.DAL;

namespace Senparc.Weixin.MP.Sample.Controllers
{
	public class BaseController : Controller
	{
		SessionHelper SessionHelper = new SessionHelper();

		public UserInfo CurrentUser
		{
			get
			{
				var sessionUser = SessionHelper["UserInfo"];
				if (sessionUser == null) return null;
				var userInfo = (UserInfo)sessionUser;
				return userInfo;
			}
		}

		public void UpdateCurrentUser()
		{
			using (var dc = new DC())
			{
				var userId = CurrentUser.userId;
				var dbUser = dc.users.FirstOrDefault(r => r.userId == userId);
				SessionHelper["UserInfo"] = new UserBLL().FromDbUser(dbUser);
			}
		}


		public string UserInfoCallbackDomain = ConfigurationManager.AppSettings["UserInfoCallbackDomain"];

	}
}
