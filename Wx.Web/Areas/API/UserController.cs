using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wx.DAL;
using Wx.BLL;
using Wx.Common;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Wx.Web.Areas.API
{
    public class UserController : BaseApiController
    {

        //RouletteBLL bll = new RouletteBLL();

        public object getUserInfo()
        {
            var cu = CurrentUser;
            if (cu == null)
            {
                return new ReturnMessage { success = false, message = "未登录。" };
            }
            var data = new { cu.name, cu.golds, cu.header };
            return new ReturnMessage { success = true, data = data };
        }

        [HttpGet]
        public object getCoinRecords(int pageIndex = 0, int pageSize = 20)
        {
            using (var dc = new DC())
            {
                var coinRecoreds = dc.user_golds_records.Where(r => r.userId == CurrentUser.userId)
                    .OrderByDescending(r => r.user_golds_recordId)
                    .Select(r => new
                    {
                        r.user_golds_recordId,
                        r.recordTime,
                        r.changeAmount,
                        r.detail,
                        r.amount
                    }).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var data = new
                {

                    today = GetTodaySummary(),
                    user = new { user_balance = CurrentUser.golds },
                    coinRecoreds
                };
                return new ReturnMessage { success = true, data = data };
            }
        }
        [HttpGet]
        public object getBetRecords(int pageIndex = 0, int pageSize = 20)
        {
            using (var dc = new DC())
            {
                var betRecoreds = dc.user_golds_records.Where(r => r.userId == CurrentUser.userId)
                    .OrderByDescending(r => r.user_golds_recordId)
                    .Where(r => r.detail.Contains("竞猜"))
                    .Select(r => new
                    {
                        r.user_golds_recordId,
                        r.recordTime,
                        r.changeAmount,
                        r.detail,
                        r.amount
                    }).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var data = new
                {

                    today = GetTodaySummary(),
                    user = new { user_balance = CurrentUser.golds },
                    betRecoreds
                };
                return new ReturnMessage { success = true, data = data };
            }
        }


        public object Submit(SubmitQuizParam param)
        {
            using (var dc = new DC())
            {
                var qbll = new QuizBLL();
                var rm = qbll.Submit(CurrentUser.userId, param.optionId, param.odds, param.golds);
                UpdateCurrentUser();
                return rm;
            }
        }


    }
}
