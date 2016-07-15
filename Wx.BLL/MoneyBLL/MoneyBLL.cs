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
    public class GoldsBLL
    {
        public static GoldsBLL Instance
        {
            get
            {
                return new GoldsBLL();
            }
        }

        public double everydayLoginGolds = 30;

        public void Everydaylogin(int userId)
        {
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    //var user = dc.users.FirstOrDefault(r => r.userId == userId);
                    //if (user == null)
                    //{
                    //	return;
                    //}
                    //var date = DateTime.Now.Date;
                    //var hasLoginedToday = dc.user_login_historys.Any(r => r.userId == userId && r.loginTime == date);
                    //if (hasLoginedToday) return;
                    //var el = new user_login_history { loginTime = date, userId = userId, golds = everydayLoginGolds, goldsRemain = everydayLoginGolds };
                    //dc.user_login_historys.Add(el);
                    //user.golds += everydayLoginGolds;
                    //var record = new user_golds_record { amount = user.golds, changeAmount = everydayLoginGolds, recordTime = DateTime.Now, userId = user.userId, detail = string.Format("每日登录赠送{0}金币。", everydayLoginGolds) };
                    //dc.user_golds_records.Add(record);
                    //dc.SaveChanges();
                    //tran.Complete();
                }

            }


        }

        public int EditUserMoney(string openId, double changeAmount, string mark)
        {
            using (var dc = new DC())
            {
                var user = dc.users.FirstOrDefault(r => r.openId == openId);
                if (user == null) return 0;
                user.golds += changeAmount;
                var record = new user_golds_record
                {

                    userId = user.userId,
                    changeAmount = changeAmount,
                    amount = user.golds,
                    detail = mark,
                    recordTime = DateTime.Now
                };
                dc.user_golds_records.Add(record);
                return dc.SaveChanges();
            }
        }


    }
}
