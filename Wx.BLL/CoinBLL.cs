using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.DAL;
using Wx.Enums;
using Wx.Common;
using System.Transactions;

namespace Wx.BLL
{
    public class CoinBLL
    {
        public virtual ReturnMessage RedeemSubmit(int userId, double golds, string alipay, string phone, string detail)
        {
            golds = double.Parse(golds.ToString("0.00"));
            if (golds < 100)
            {
                return new ReturnMessage { success = false, message = "请至少兑换100金币。" };
            }
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    var user = dc.users.FirstOrDefault(r => r.userId == userId);
                    if (user.golds < golds)
                    {
                        return new ReturnMessage { success = false, message = "余额不足。" };
                    }
                    user.golds -= golds;
                    dc.redeems.Add(new redeem
                    {
                        userId = userId,
                        golds = golds,
                        createTime = DateTime.Now,
                        redeemStatus = RedeemStatus.UnPayed,
                        rmb = 0,
                        detail = detail,
                        alipay = alipay,
                        phone = phone
                    });
                    var record = new user_golds_record
                    {
                        amount = user.golds,
                        changeAmount = 0 - golds,
                        userId = userId,
                        detail = string.Format("兑换 {0}", golds.ToString("0.00")),
                        recordTime = DateTime.Now
                    };
                    dc.user_golds_records.Add(record);
                    dc.SaveChanges();
                    tran.Complete();
                }
            }
            return new ReturnMessage { success = true, message = "兑换成功！" };
        }
        public virtual ReturnMessage PaySubmit(int userId, double rmb)
        {
            rmb = double.Parse(rmb.ToString("0.00"));
            //if (rmb <= 0)
            //{
            //	return new ReturnMessage { success = false, message = "请至少充值1元。" };
            //}
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    var user = dc.users.FirstOrDefault(r => r.userId == userId);
                    var golds = rmb * 100 * 10000;
                    dc.pays.Add(new pay { userId = user.userId, rmb = rmb, golds = golds, payTime = DateTime.Now, tip = "" });
                    user.golds += golds;
                    var record = new user_golds_record
                    {
                        amount = user.golds,
                        changeAmount = golds,
                        userId = userId,
                        detail = string.Format("充值 {0} 元。", rmb.ToString("0.00")),
                        recordTime = DateTime.Now
                    };
                    dc.user_golds_records.Add(record);
                    dc.SaveChanges();
                    tran.Complete();
                }
            }
            return new ReturnMessage { success = true, message = "充值成功！" };
        }



        public virtual object SaveJsapiPackageRequest(string prepayId, int userId, Dictionary<string, string> dict_package)
        {
            using (var dc = new DC())
            {
                var pr = JsonHelper.Deserialize<pay_jsapi_package_request>(dict_package);
                pr.prepayId = prepayId;
                pr.createTime = DateTime.Now;
                pr.userId = userId;
                dc.pay_jsapi_package_requests.Add(pr);
                dc.SaveChanges();
            }
            return null;
        }

        public void Resolve(Dictionary<string, object> notifyData)
        {
            using (var dc = new DC())
            {
                var nd = JsonHelper.Deserialize<pay_jsapi_notify>(notifyData);
                nd.createTime = DateTime.Now;
                var user = dc.users.FirstOrDefault(r => r.openId == nd.openid);
                nd.userId = user.userId;
                var pr = dc.pay_jsapi_package_requests.FirstOrDefault(r => r.out_trade_no == nd.out_trade_no);
                nd.pay_jsapi_package_request = pr;
                dc.pay_jsapi_notifys.Add(nd);
                dc.SaveChanges();
                pr.pay_jsapi_notify = nd;
                dc.SaveChanges();

                if (dc.pay_jsapi_notifys.Count(r => r.out_trade_no == nd.out_trade_no) == 1)
                {
                    PaySubmit(nd.userId, ((double)pr.total_fee) / 100);
                }
            }

        }
    }
}


