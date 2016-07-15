using System.Collections.Generic;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLibV3;
using Wx.BLL;
using Wx.Common;
using Wx.DAL;
using System.Configuration;
using System.Linq;
using System;

namespace Wx.Web.Areas.API
{
    public class InviteController : BaseApiController
    {

        //RouletteCommon common = new RouletteCommon("dota");

        public object qrCode(Dictionary<string, object> dict)
        {
            using (var dc = new DC())
            {
                //inviteQrCode
                var inviteQrCode = new UserBLL().GetInviteQrCode(CurrentUser.userId);
                var url = dict["url"].ToString();
                var data = new { CurrentUser.userId, inviteQrCode, jssdkUiPackage = GetJssdkUiPackage(url), url, abs = Request.RequestUri.AbsoluteUri };
                return new ReturnMessage { success = true, data = data };
            }
        }
        private object GetJssdkUiPackage(string url)
        {
            string appId = ConfigurationManager.AppSettings["WeixinAppId"];
            string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, secret, url);
            return new
            {
                appId = jssdkUiPackage.AppId,
                nonceStr = jssdkUiPackage.NonceStr,
                signature = jssdkUiPackage.Signature,
                timestamp = jssdkUiPackage.Timestamp
            };
        }

        public object GetMyFriends()
        {

            using (var dc = new DC())
            {
                var minDate = DateTime.MinValue;
                var friends = dc.users.Where(r => r.invite_by_userId == CurrentUser.userId)
                .Select(u => new
                {
                    u.name,
                    time = dc.user_invite_logs.OrderByDescending(r => r.user_invite_logId).Where(r => r.invite_by_user_id == CurrentUser.userId && r.openId == u.openId).FirstOrDefault()
                });
                var q = (from u in dc.users
                         let inviteLog = dc.user_invite_logs.OrderByDescending(r => r.user_invite_logId).Where(r => r.invite_by_user_id == CurrentUser.userId && r.openId == u.openId).FirstOrDefault()
                         where u.invite_by_userId == CurrentUser.userId
                         select new
                         {
                             u.name,
                             u.header,
                             time = inviteLog != null ? inviteLog.createTime : minDate
                         }
                         ).ToList();
                var data = base.GetTodaySummaryAndUserBalance();
                data["friends"] = q;
                return new ReturnMessage { success = true, data = data };

            }
        }
    }
}