using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wx.DAL;
using Wx.BLL;
using Wx.Common;
using System.Linq.Dynamic;
using System.Data.Entity;


namespace Wx.Portal.Controllers.API
{
	public class GameController : ApiController
	{
		public object getList(int ps = 10, int p = 1)
		{
			using (var dc = new DC())
			{
				var q = dc.games.OrderByDescending(r => r.gameId).Select(r => new
				{
					r.gameId,
					r.name,
					r.game_typeId,
					team1 = r.team1.name,
					team2 = r.team2 == null ? "" : r.team2.name,
					team1Id = r.team1.teamId,
					team2Id = r.team2 == null ? null : (int?)r.team2.teamId,
					r.createTime,
					r.displayOrder
				});
				return q.ToPagedList();
			}

		}

		public object getDetail(int gameId)
		{
			using (var dc = new DC())
			{
				//return dc.games.Select(r=>new { r.name}).ToList();
				var q = dc.games
					.Include(r => r.bets)
					.Select(r => new
					{
						r.gameId,
						r.name,
						r.createTime,
						team1_name = r.team1.name,
						team2_name = r.team2.name,
						bets = r.bets.OrderBy(p => p.displayOrder).Select(p => new
						{
							p.betId,
							p.name,
							p.endTime,
							p.startTime,
							p.betstatus,
							p.displayOrder,
							quizs = p.quizs.OrderBy(k => k.displayOrder).Select(k =>
								new
								{
									k.quizId,
									k.subject,
									k.quizstatus,
									k.displayOrder,
									//k.subject,
									options = k.options.Select(o =>
										new
										{
											o.optionId,
											o.optionstatus,
											o.subject

										})

								})
						})

					}).FirstOrDefault(r => r.gameId == gameId);
				return q;
			}
		}



		public object save(Dictionary<string, object> dict)
		{
			dict.Remove("team1");
			dict.Remove("team2");
			var game = JsonHelper.Deserialize<game>(JsonHelper.Serialize(dict));
			team team1 = null;
			team team2 = null;
			using (var dc = new DC())
			{
				if (dict.ContainsKey("team1Id"))
				{
					var team1Id = int.Parse(dict["team1Id"].ToString());
					team1 = dc.teams.FirstOrDefault(r => r.teamId == team1Id);
					game.team1 = team1;
				}
				if (dict.ContainsKey("team2Id"))
				{
					var team2Id = int.Parse(dict["team2Id"].ToString());
					team2 = dc.teams.FirstOrDefault(r => r.teamId == team2Id);
					game.team2 = team2;
				}
				if (game.gameId > 0)
				{
					var row = dc.games.FirstOrDefault(r => r.gameId == game.gameId);
					row.team1 = game.team1;
					row.team2 = game.team2;
					row.name = game.name;
					row.game_typeId = game.game_typeId;

				}
				else
				{
					dc.games.Add(game);
				}
				dc.SaveChanges();
			}
			return dict;

		}
		[HttpPost]
		public object delete(Dictionary<string, object> dict)
		{
			using (var dc = new DC())
			{
				var id = int.Parse(dict["id"].ToString());
				var game = dc.games.FirstOrDefault(r => r.gameId == id);
				dc.games.Remove(game);
				return dc.SaveChanges();
			}

		}

	}
}
