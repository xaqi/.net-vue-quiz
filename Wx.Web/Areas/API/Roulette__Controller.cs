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
	public class Roulette__Controller : BaseApiController
	{

		//RouletteBLL bll = new RouletteBLL();

		[HttpGet]
		public object getRouletteInfo()
		{
			using (var dc = new DC())
			{
				//var daybegin = DateTime.Now.Date;
				//var dayend = DateTime.Now.Date.AddDays(1);

				//var rouletteHistory = (from a in dc.bets
				//					   join b in dc.rouletteResults on a.betId equals b.betId into ab
				//					   from b in ab.DefaultIfEmpty()
				//					   where a.startTime >= daybegin && a.startTime <= DateTime.Now
				//					   && a.gameId == 2
				//					   select new
				//					   {
				//						   a.betId,
				//						   a.name,
				//						   a.betstatus,
				//						   heroName = b == null ? "" : b.heroName
				//					   }
				//				 ).OrderByDescending(r => r.betId).Take(10).ToList();


				//var user = new { user_balance = CurrentUser.money };
				//var nextRouletteItem = RouletteBLL.getNextResolveItem();
				//if (nextRouletteItem == null)
				//{
				//	return new ReturnMessage { success = false, message = "今天的竞猜还未开始，请稍候再来！" };
				//}
				//var bet = nextRouletteItem.bet;
				//var data = new
				//{
				//	today = GetTodaySummary(),
				//	user,
				//	roulette = new
				//	{
				//		date = DateTime.Now.ToString("MM月dd日"),
				//		nextRouletteItem.bet.name,
				//		nextRouletteItem.remainSeconds,
				//		quizs = dc.quizs.Where(r => r.betId == bet.betId)
				//			.Select(q => new
				//			{
				//				q.subject,
				//				bet = new { q.bet.startTime },
				//				options = q.options.Select(op =>
				//				new
				//				{
				//					op.optionId,
				//					op.subject,
				//					op.fixedOdds
				//				})
				//			})
				//			.ToList()
				//	},
				//	rouletteHistory
				//};
				return new ReturnMessage { success = true, data = null };
			}
		}


		public object Submit(SubmitQuizParam param)
		{
			using (var dc = new DC())
			{
				var qbll = new QuizBLL();
				var rm = qbll.Submit(CurrentUser.userId, param.optionId, param.odds, param.golds);
				return rm;
			}
		}
	}
}
