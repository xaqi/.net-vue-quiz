using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wx.DAL;
using Wx.BLL;
using Wx.Common;
using Wx.Web.App_Start.Filters;
using Wx.Common.Helpers;

namespace Wx.Web.Areas.API
{
    public class RouletteController : BaseApiController
    {

        //RouletteCommon common = new RouletteCommon("dota");

        [HttpGet]
        [ApiAuthFilter]
        public object getRouletteInfo()
        {
            using (var dc = new DC())
            {
                //var data = new { bll.roulette, ssc = SscItem.GetSscList(DateTime.Now) };
                var data = base.GetTodaySummaryAndUserBalance();
                var heroCount = dc.roulette_heros.Where(r => r.game_typeId == this.GameType.id).Count();
                var q = (from a in dc.roulette_property_types
                         where a.game_typeId == this.GameType.id
                         orderby a.displayOrder
                         select new
                         {
                             a.name,
                             propertys = a.roulette_propertys.OrderBy(r => r.displayOrder)
                             .Select(r => new
                             {
                                 propertyId = r.roulette_propertyId,
                                 r.name,
                                 active = false,
                                 odds = (double)heroCount / (double)dc.roulette_hero_propertys.Count(p => p.roulette_propertyId == r.roulette_propertyId)
                             })
                         })
                         .ToList();
                data["property_types"] = q;
                data["sscHistory"] = this.GetSSCHistory().data;
                data["sscTimer"] = this.GetSSCTimer().data;
                data["hero_propertys"] = GetHeroPropertys();
                return new ReturnMessage { success = true, data = data };
            }
        }

        public object GetHeroPropertys()
        {
            using (var dc = new DC())
            {
                var heroOdds = (from a in dc.roulette_hero_propertys
                                .Where(r => r.property.roulette_property_type.game_typeId == this.GameType.id)
                                select a)
                                .GroupBy(a => a.roulette_heroId)
                                .Select(r => new { heroId = r.Key, propertys = r.Select(p => p.roulette_propertyId).OrderBy(p => p) })
                                .Select(r => r.propertys)
                                .ToList();
                return heroOdds;
            }
        }
        public object Submit(SubmiRouletteParam param)
        {
            if (param.golds > 3000) return new ReturnMessage { success = false, message = "最多投注3000金币" };
            using (var dc = new DC())
            {
                var userId = CurrentUser.userId;
                var rbll = new RouletteBLL();
                rbll.Submit(userId, param.selectPropertyIds, param.golds, GameType.id);
                var data = new { };
                UpdateCurrentUser();
                return new ReturnMessage { success = true, message = "投注成功。", data = data };
            }
        }


        [HttpGet]
        public ReturnMessage GetSSCTimer()
        {
            var sh = new SSCHelper();
            var dt = DateTime.Now;
            var items = SSCHelper.GetSSCList(dt);
            var currentItem = items.FirstOrDefault(r => r.startTime < dt && r.endTime >= dt);
            var timer = (currentItem.endTime - dt).TotalSeconds.Round(1);
            var nextRefreshTimer = (currentItem.endTime.AddSeconds(80) - dt).TotalSeconds.Round(1);

            return new ReturnMessage { success = true, data = new { name = currentItem.period.Substring(currentItem.period.Length - 3), timer } };
        }

        [HttpGet]
        public ReturnMessage GetSSCHistory(int minPeriod = 0)
        {
            var dt = DateTime.Now.Date;
            SSCHelper.FetchSSC(dt);
            using (var dc = new DC())
            {
                var roulette_property_types = CacheHelper.Get("roulette_property_types", () => dc.roulette_property_types.ToList());
                var roulette_propertys = CacheHelper.Get("roulette_propertys", () => dc.roulette_propertys.ToList());
                var roulette_heros = CacheHelper.Get("roulette_heros", () => dc.roulette_heros.ToList());
                var roulette_hero_propertys = CacheHelper.Get("roulette_hero_propertys", () => dc.roulette_hero_propertys.ToList());

                var heroCount = roulette_heros.Count(r => r.game_typeId == GameType.id);
                var q = (
                    from a in SSCHelper.SSCItemList.Where(r => r.endTime < DateTime.Now)
                    let hero = string.IsNullOrWhiteSpace(a.no) ? null : roulette_heros.Where(r => r.game_typeId == GameType.id).Skip(int.Parse(a.no) % heroCount).FirstOrDefault()

                    select new
                    {
                        period = int.Parse(a.period.Substring(a.period.Length - 3)),
                        no = string.IsNullOrWhiteSpace(a.no) ? "开奖中" : int.Parse(a.no).ToString("00000"),
                        hero = hero == null ? "正在等待时时彩开奖结果" :
                                            (
                                            hero.name + " - " +
                                                string.Join(" - ",
                                                roulette_hero_propertys
                                                .Where(r => r.roulette_heroId == hero.roulette_heroId)
                                                .Select(r => r.roulette_propertyId)
                                                .Select(r => roulette_propertys.FirstOrDefault(p => p.roulette_propertyId == r))
                                                .Select(r => r.name)
                                                .ToArray())
                                            )
                    }
                           );

                return new ReturnMessage { success = true, data = new { history = q.Where(r => r.period > minPeriod).OrderByDescending(r => r.period).ToList() } };
            }

        }
    }

    public class SubmiRouletteParam
    {
        public int[] selectPropertyIds { get; set; }
        public double golds { get; set; }
    }
}
