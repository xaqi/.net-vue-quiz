using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.DAL;
using Wx;
using System.Transactions;

namespace Wx.BLL
{
    public class UserBLL
    {
        public static UserBLL Instance
        {
            get
            {
                return new UserBLL();
            }
        }
        public int GetUserAmount()
        {

            using (var dc = new DC())
            {
                return dc.users.Count();
            }
        }
        public user CurrentUser
        {
            get
            {
                using (var dc = new DC())
                {
                    return dc.users.FirstOrDefault();
                }
            }
        }

        public user TestUser
        {
            get { return this.CurrentUser; }
        }

        public UserInfo SaveOAuthUser(Dictionary<string, object> oAuthUser)
        {
            var openId = oAuthUser["openid"].ToString();
            var name = oAuthUser["nickname"].ToString();
            var header = oAuthUser["headimgurl"].ToString();
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    var user = dc.users.FirstOrDefault(r => r.openId == openId);
                    if (user != null)
                    {
                        user.last_login_time = DateTime.Now;
                        if (user.first_login_time < DateTime.Parse("2000-01-01"))
                        {
                            user.first_login_time = DateTime.Now;
                        }
                        dc.SaveChanges();
                        tran.Complete();
                        return FromDbUser(user);
                    }
                    user = new user { openId = openId, name = name, header = header, first_login_time = DateTime.Now };
                    dc.users.Add(user);
                    var inviteLog = dc.user_invite_logs.OrderByDescending(r => r.user_invite_logId).Where(r => r.openId == openId).FirstOrDefault();
                    if (inviteLog != null)
                    {
                        user.invite_by_userId = inviteLog.invite_by_user_id;
                        user.invite_by_channelId = inviteLog.invite_by_channel_id;
                    }
                    dc.SaveChanges();
                    // first login money
                    // every day login money
                    //GoldsBLL.Instance.Everydaylogin(user.userId);
                    tran.Complete();
                    return FromDbUser(user);
                }
            }
        }

        public UserInfo FromDbUser(user user)
        {
            return new UserInfo { name = user.name, openId = user.openId, header = user.header, golds = user.golds, userId = user.userId };
        }


        public user GetUser(string openId, string name = null, string header = null)
        {
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    var user = dc.users.FirstOrDefault(r => r.openId == openId);
                    if (user != null)
                    {
                        return user;
                    }
                    user = new user { openId = openId, name = name, header = header };
                    dc.users.Add(user);
                    dc.SaveChanges();
                    // first login money
                    // every day login money
                    //GoldsBLL.Instance.Everydaylogin(user.userId);
                    tran.Complete();
                    return user;
                }
            }
        }

        public user GetUser(int userId)
        {
            using (var dc = new DC())
            {
                return dc.users.FirstOrDefault(r => r.userId == userId);
            }
        }

        public string GetInviteQrCode(int userId)
        {

            using (var dc = new DC())
            {
                var user = dc.users.FirstOrDefault(r => r.userId == userId);
                if (string.IsNullOrWhiteSpace(user.qr_ticket))
                {
                    var appId = System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"];
                    var secret = System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"];
                    var qrTicket = Wx.Common.Helpers.QRCodeHelper.QrCode(appId, secret, "u=" + userId);
                    if (string.IsNullOrWhiteSpace(qrTicket.ticket))
                    {
                        return "";

                    }
                    user.qr_ticket = qrTicket.ticket;
                    user.qr_url = qrTicket.url;
                    user.qr_create_time = DateTime.Now;
                    dc.SaveChanges();
                }
                return $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={user.qr_ticket}";
            }
        }


        public void SaveInviteLog(string openId, int inviteByUserId, string event_type)
        {
            using (var dc = new DC())
            {
                dc.user_invite_logs.Add(new user_invite_log { openId = openId, invite_by_user_id = inviteByUserId, event_type = event_type, createTime = DateTime.Now });
                dc.SaveChanges();
                var invitedUser = dc.users.FirstOrDefault(r => r.openId == openId);
                if (invitedUser != null)
                {
                    invitedUser.invite_by_userId = inviteByUserId;
                    if (invitedUser.status == "取消关注")
                    {
                        invitedUser.status = "再次关注";
                    }
                }
                dc.SaveChanges();
            }
        }

        public LoginTimesInfo GetLoginInfo(int userId)
        {
            using (var dc = new DC())
            {
                var today = DateTime.Now.Date;

                //新用户
                var isNewUser = dc.user_login_bonuses.Count(r => r.userId == userId) < 2;
                if (isNewUser) return new LoginTimesInfo { IsNewUser = true };

                //今日已签到
                var isGitFinished = dc.user_login_bonuses.Any(r => r.userId == userId && r.bonus_typeId == 2 && r.create_time >= today);
                if (isGitFinished) return new LoginTimesInfo { IsGitFinished = true };


                var dt = DateTime.Now.Date.AddDays(-6);
                var recentLoginTimes = dc.user_login_bonuses.Where(r => r.create_time > dt && r.bonus_typeId == 2).Select(r => r.create_time).ToList().Select(r => r.Date).ToList();
                var continuityLoginCount = 1;
                for (int i = 0; i <= recentLoginTimes.Count(); i++)
                {
                    if (recentLoginTimes.Count(r => r >= DateTime.Now.Date.AddDays(-i)) == i)
                    {
                        continuityLoginCount = i;
                    }
                    else
                    {
                        break;
                    }
                }
                return new LoginTimesInfo { IsNewUser = false, ContinuityLoginCount = continuityLoginCount };
            }

        }
        public List<DailyLoginBonusConfig> GetDailyLoginBonusConfigs(int userId)
        {
            var loginInfo = new UserBLL().GetLoginInfo(userId);
            var clc = loginInfo.ContinuityLoginCount;
            return new List<DailyLoginBonusConfig>
            {
                new DailyLoginBonusConfig { LoginTimes=1, golds=30, name="登录一天", IsActive=clc<2 },
                new DailyLoginBonusConfig { LoginTimes=3, golds=50, name="连续三天",IsActive=(clc>=2 && clc<4)  },
                new DailyLoginBonusConfig { LoginTimes=5, golds=70, name="连续五天",IsActive=clc>=4 }
            };
        }
        public void Unsubscribe(string openId)
        {
            using (var dc = new DC())
            {
                var user = dc.users.FirstOrDefault(r => r.openId == openId);
                if (user == null) return;
                user.status = "取消关注";
                dc.SaveChanges();
            }
        }
    }
    public class LoginTimesInfo
    {
        public bool IsNewUser { get; set; }
        public int ContinuityLoginCount { get; set; }
        public bool IsGitFinished { get; set; }
    }
    public class DailyLoginBonusConfig
    {
        public int LoginTimes { get; set; }
        public string name { get; set; }
        public int golds { get; set; }
        public bool IsActive { get; set; }
    }
}
