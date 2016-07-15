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
    public class BetController : BaseApiController
    {

        [HttpGet]
        public object getBetHistory(int tabIndex = 0)
        {
            using (var dc = new DC())
            {
                object page = new { };
                if (tabIndex == 0 || tabIndex == 1)
                    page = getBetHistoryByPage(tabIndex).data;
                else if (tabIndex == 2)
                    page = getBetHistoryByPage_Tab3().data;

                var data = new
                {

                    today = GetTodaySummary(),
                    user = new { user_balance = CurrentUser.golds },
                    page
                };
                return new ReturnMessage { success = true, data = data };
            }
        }

        [HttpGet]
        public ReturnMessage getBetHistoryByPage(int tabIndex)
        {
            using (var dc = new DC())
            {
                var q = (from a in dc.user_options
                         where a.userId == CurrentUser.userId
                         select new
                         {
                             a.user_optionId,
                             a.createTime,
                             a.golds,
                             a.odds,
                             a.optionId,
                             option_subject = a.option.subject,
                             a.option.optionstatus,
                             a.UserOptionStatus,
                             quiz_subject = a.option.quiz.subject,
                             bet_name = a.option.quiz.bet.name,
                             game_name = a.option.quiz.bet.game.name,
                             team1_name = a.option.quiz.bet.game.team1.name,
                             team2_name = a.option.quiz.bet.game.team2.name
                         })
                         ;
                if (tabIndex == 0)
                {
                    q = q.Where(r => r.team2_name == null);
                }
                if (tabIndex == 1)
                {
                    q = q.Where(r => r.team2_name != null);
                }
                var pagedListResult = q.OrderByDescending(r => r.user_optionId).ToPagedList();
                var data = pagedListResult.AsDictionary();
                data["rows"] = pagedListResult.rows.Select(r =>
               {
                   var dict = r.AsDictionary();
                   dict["optionstatus_cn"] = dict["optionstatus"].ToString();
                   dict["UserOptionStatus_cn"] = dict["UserOptionStatus"].ToString();
                   return dict;
               });
                return new ReturnMessage { success = true, data = data };
            }
        }
        [HttpGet]
        public ReturnMessage getBetHistoryByPage_Tab0()
        {
            return getBetHistoryByPage(0);
        }
        [HttpGet]
        public ReturnMessage getBetHistoryByPage_Tab1()
        {
            return getBetHistoryByPage(1);
        }
        [HttpGet]
        public ReturnMessage getBetHistoryByPage_Tab3()
        {
            using (var dc = new DC())
            {
                var q = (from a in dc.roulette_submits
                         join b in dc.game_types on a.game_typeId equals b.game_typeId
                         where a.userId == CurrentUser.userId
                         select new
                         {
                             a.roulette_submitId,
                             gameTypeName = b.cnName,
                             a.golds,
                             a.rouletteSubmitStatus,
                             a.createTime,
                             a.propertys,
                             a.sscPeriod,
                             a.ssc_no,
                             a.resolve_time,
                             a.odds,
                             a.correct_heroId
                         }
                );
                var pagedListResult = q.OrderByDescending(r => r.roulette_submitId).ToPagedList();
                var data = pagedListResult.AsDictionary();
                var roulette_propertys = Wx.Common.Helpers.CacheHelper.Get("roulette_propertys", () => dc.roulette_propertys.ToList());
                data["rows"] = pagedListResult.rows.Select(r =>
                {
                    var dict = r.AsDictionary();
                    var propertys = JsonHelper.Deserialize<int[]>(dict["propertys"].ToString()).Select(x => roulette_propertys.FirstOrDefault(p => p.roulette_propertyId == x)?.name);
                    dict["propertys"] = propertys;
                    dict["rouletteSubmitStatus_cn"] = dict["rouletteSubmitStatus"].ToString();
                    return dict;
                });
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

    }
}
