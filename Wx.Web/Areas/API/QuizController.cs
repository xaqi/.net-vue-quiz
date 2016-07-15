using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wx.DAL;
using Wx.BLL;
using Wx.Common;

namespace Wx.Web.Areas.API
{
    public class QuizController : BaseApiController
    {

        [HttpGet]
        public object getList()
        {
            using (var dc = new DC())
            {
                var daybegin = DateTime.Now.Date;
                var dayend = DateTime.Now.Date.AddDays(1);
                var dt_now = DateTime.Now;
                var quizs = (
                    from a in dc.quizs
                    let bet = a.bet
                    let game = bet.game
                    let team1 = game.team1
                    let team2 = game.team2
                    let options = a.options
                    let quizGolds = (double)a.options.SelectMany(k => k.user_options).Sum(r => r.golds)
                    where (bet.startTime <= dt_now) && (bet.endTime >= dt_now)
                    && options.Count() == 2
                    && game.game_typeId == GameType.id

                    //&& game.game_type > 2
                    select new
                    {
                        a.subject,
                        a.quizstatus,
                        a.displayOrder,
                        bet = new
                        {
                            bet.name,
                            bet.betId,
                            bet.startTime,
                            bet.endTime,
                            bet.betstatus,
                            bet.displayOrder
                        },
                        game = new
                        {
                            game.gameId,
                            game.name,
                            game.game_typeId,
                            team1 = new
                            {
                                team1.teamId,
                                team1.name,
                                team1.avatar
                            }
                            ,
                            team2 = new
                            {
                                team2.teamId,
                                team2.name,
                                team2.avatar
                            }
                        },
                        options = (from p in a.options
                                   let totalGolds = (double)p.user_options.Select(k => k.golds).Sum()
                                   select new
                                   {

                                       p.optionId,
                                       p.subject,
                                       p.optionstatus,
                                       odds = (quizGolds + 1) / (totalGolds + 1),
                                       fixedOdds = (quizGolds + 1) / (totalGolds + 1),
                                       userCount = p.user_options.Select(k => k.userId).Count(),
                                       totalGolds,
                                       quizGolds
                                   }
                                  )
                    }
                    )
                    .Where(r => r.game.game_typeId == GameType.id && r.options.Count() == 2).ToList();

                var zb_quizs = quizs.Where(r => r.game.team2.teamId == 0).ToList().OrderBy(r => r.bet.displayOrder).ThenBy(r => r.displayOrder).ToList();
                var zd_quizs = quizs.Where(r => r.game.team2.teamId != 0).ToList().OrderBy(r => r.bet.displayOrder).ThenBy(r => r.displayOrder).ToList();
                var user = new { user_balance = CurrentUser.golds };
                var data = new { today = GetTodaySummary(), zb_quizs, zd_quizs, user, dt = DateTime.Now.ToString("HH:mm:ss") };
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

    public class SubmitQuizParam
    {
        public int optionId { get; set; }
        public double odds { get; set; }
        public double golds { get; set; }
    }
}
