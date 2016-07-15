using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wx.DAL;
using Wx.BLL;
using System.Web;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Wx.Common.Helpers;
namespace Wx.Web.Areas.API
{
    public class GameType
    {
        public string name { get; set; }
        public int id { get; set; }
    }
    public class BaseApiController : ApiController
    {
        SessionHelper SessionHelper = new SessionHelper();
        public GameType GameType
        {
            get
            {
                var gameIds = new Dictionary<string, int> { { "lol", 1 }, { "dota", 2 } };
                var name = this.ControllerContext.RouteData.Values["gameType"].ToString().ToLower();
                return new GameType { name = name, id = gameIds[name] };
            }
        }
        public UserInfo CurrentUser
        {
            get
            {
                UpdateCurrentUser();
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
                var sessionUser = SessionHelper["UserInfo"];
                if (sessionUser == null) return;
                var userInfo = (UserInfo)sessionUser;
                var userId = userInfo.userId;
                var dbUser = dc.users.FirstOrDefault(r => r.userId == userId);
                SessionHelper["UserInfo"] = new UserBLL().FromDbUser(dbUser);
            }
        }

        public object GetTodaySummary()
        {
            using (var dc = new DC())
            {
                var daybegin = DateTime.Now.Date;
                var dayend = DateTime.Now.Date.AddDays(1);
                var todayQuizs = dc.user_options.Where(r => r.createTime < dayend && r.createTime > daybegin);
                var todayRt = dc.roulette_submits.Where(r => r.createTime < dayend && r.createTime > daybegin);
                var any1 = todayQuizs.Any();
                var any2 = todayRt.Any();
                var zj = ZJHelper.GetLog(DateTime.Now);
                var today = new
                {
                    quizUserCount = (any1 ? todayQuizs.Count() : 0) + (any2 ? todayRt.Count() : 0) + zj.PeopleCount,
                    quizGolds = (any1 ? (int)todayQuizs.Sum(r => r.golds) : 0) + (any2 ? (int)todayRt.Sum(r => r.golds) : 0) + (int)zj.Money
                };
                return today;
            }
        }

        public Dictionary<string, object> GetTodaySummaryAndUserBalance()
        {
            var data = new Dictionary<string, object> { };
            data["today"] = this.GetTodaySummary();
            data["user"] = new { user_balance = CurrentUser.golds };
            return data;
        }

    }

}
