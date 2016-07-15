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


namespace Wx.Portal.Controllers.API
{
    public class BetController : ApiController
    {
        public object getList(int ps = 10, int p = 1)
        {
            using (var dc = new DC())
            {
                var q = dc.bets.OrderByDescending(r => r.betId).Select(r => new
                {
                    r.name,
                    gameName = r.game.name,
                    r.endTime,
                    r.startTime,
                    r.betId,
                    r.betstatus,
                    r.displayOrder,
                    r.gameId
                });
                return q.ToPagedList();
            }

        }

        public object save(Dictionary<string, string> dict)
        {
            if (dict.ContainsKey("quizs"))
            {
                dict.Remove("quizs");
            }
            var json = JsonHelper.Serialize(dict);
            var bet = JsonHelper.Deserialize<bet>(JsonHelper.Serialize(dict));
            using (var dc = new DC())
            {
                if (bet.betId > 0)
                {
                    var row = dc.bets.FirstOrDefault(r => r.betId == bet.betId);
                    row.name = bet.name;
                    row.startTime = bet.startTime;
                    row.endTime = bet.endTime;
                    row.displayOrder = bet.displayOrder;
                }
                else
                {
                    dc.bets.Add(bet);
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
                var bet = dc.bets.FirstOrDefault(r => r.betId == id);
                dc.bets.Remove(bet);
                return dc.SaveChanges();
            }

        }

    }
}
