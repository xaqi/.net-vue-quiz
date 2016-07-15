using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.DAL;
using Wx;
using System.Transactions;
using Wx.Common;

namespace Wx.BLL
{
    public class BonusBLL
    {


        public object GetNewUserGolds(int userId)
        {
            using (var dc = new DC())
            {
                var isExists = dc.user_login_bonuses.Any(r => r.bonus_typeId == 1 && r.userId == userId);
                var closeIfSuccess = dc.user_login_bonuses.Any(r => r.userId == userId && r.bonus_typeId == 2);
                if (isExists) return new ReturnMessage { success = true, message = "新手礼包已领取！", data = new { close = closeIfSuccess } };

                var golds = 88;
                var bonus = new user_login_bonus
                {
                    userId = userId,
                    bonus_typeId = 1,
                    create_time = DateTime.Now,
                    golds = golds,
                    remain_golds = 0
                };
                var user = dc.users.FirstOrDefault(r => r.userId == userId);
                user.golds += golds;
                var log = new user_golds_record
                {
                    userId = userId,
                    amount = user.golds,
                    changeAmount = golds,
                    recordTime = DateTime.Now,
                    detail = $"新手礼包{golds.ToString("0.00")}金币"
                };
                dc.user_login_bonuses.Add(bonus);
                dc.user_golds_records.Add(log);
                dc.SaveChanges();
                return new ReturnMessage { success = true, message = "领取成功！", data = new { close = closeIfSuccess } };
            }

        }

        public object GetDailyLoginGolds(int userId)
        {
            using (var dc = new DC())
            {
                var today = DateTime.Now.Date;
                var isExists = dc.user_login_bonuses.Any(r => r.bonus_typeId == 2 && r.create_time > today && r.userId == userId);
                var closeIfSuccess = dc.user_login_bonuses.Any(r => r.userId == userId && r.bonus_typeId == 1);
                if (isExists) return new ReturnMessage { success = true, message = "今日已签到！", data = new { close = closeIfSuccess } };
                var bonusConfig = new UserBLL().GetDailyLoginBonusConfigs(userId).FirstOrDefault(r => r.IsActive);
                var golds = bonusConfig.golds;
                var bonus = new user_login_bonus
                {
                    userId = userId,
                    bonus_typeId = 2,
                    create_time = DateTime.Now,
                    golds = golds,
                    remain_golds = golds
                };
                var user = dc.users.FirstOrDefault(r => r.userId == userId);
                user.golds += golds;
                var log = new user_golds_record
                {
                    userId = userId,
                    amount = user.golds,
                    changeAmount = golds,
                    recordTime = DateTime.Now,
                    detail = $"{bonusConfig.name}签到{golds.ToString("0.00")}金币"
                };
                dc.user_login_bonuses.Add(bonus);
                dc.user_golds_records.Add(log);
                dc.SaveChanges();
                var close = dc.user_login_bonuses.Count(r => r.userId == userId) >= 2;
                return new ReturnMessage { success = true, message = "签到成功!", data = new { close = closeIfSuccess } };
            }

        }

    }
}
