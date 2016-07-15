using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.DAL;
using Wx.Enums;
using Wx.Common;
using System.Transactions;
using Wx.Common.Helpers;

namespace Wx.BLL
{
    public class RouletteBLL
    {
        public virtual ReturnMessage Submit(int userId, int[] propertyIds, double golds, int gameTypeId)
        {
            golds = double.Parse(golds.ToString("0.00"));
            if (golds <= 0)
            {
                return new ReturnMessage { success = false, message = "请至少投注1金币。" };
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

                    dc.roulette_submits.Add(new roulette_submit
                    {
                        userId = userId,
                        golds = golds,
                        propertys = JsonHelper.Serialize(propertyIds),
                        createTime = DateTime.Now,
                        odds = 1,
                        rouletteSubmitStatus = RouletteSubmitStatus.待定,
                        sscPeriod = "",
                        game_typeId = gameTypeId
                    });
                    user.golds -= golds;
                    if (user.invite_by_userId != null)
                    {
                        var inviteUser = dc.users.FirstOrDefault(r => r.userId == user.invite_by_userId);
                        if (inviteUser != null)
                        {
                            inviteUser.golds += golds * 0.03;
                            dc.user_golds_records.Add(new user_golds_record
                            {
                                changeAmount = golds * 0.03,
                                amount = user.golds + golds * 0.03,
                                recordTime = DateTime.Now,
                                detail = "战友竞猜奖励",
                                userId = inviteUser.userId
                            });
                        }
                    }
                    var record = new user_golds_record
                    {
                        amount = user.golds,
                        changeAmount = 0 - golds,
                        userId = userId,
                        detail = string.Format("竞猜 {0}", dc.roulette_propertys.FirstOrDefault(r => propertyIds.Contains(r.roulette_propertyId)).roulette_property_type.game_type.cnName, "", golds),
                        recordTime = DateTime.Now
                    };
                    dc.user_golds_records.Add(record);
                    dc.SaveChanges();
                    tran.Complete();
                }
            }
            return new ReturnMessage { success = true, message = "竞猜成功！" };
        }

        public static DateTime LastResolvingTime = DateTime.MinValue;
        protected void Resolve(DateTime dt, int GameTypeId = 1)
        {
            var dt_s = dt.Date;
            var dt_e = dt.Date.AddDays(1);
            using (var dc = new DC("prod"))
            {
                var sscList = Wx.Common.Helpers.SSCHelper.FetchSSC(dt);
                var lastSSC = sscList.Where(r => !string.IsNullOrWhiteSpace(r.no)).LastOrDefault();
                if (lastSSC == null) return;
                var toResolvedSubmits = dc.roulette_submits
                    .Where(r => r.game_typeId == GameTypeId || r.game_typeId == 0)
                    .Where(r => r.createTime >= dt_s && r.createTime < dt_e && r.createTime <= lastSSC.endTime)
                    .Where(r => r.rouletteSubmitStatus == Wx.Enums.RouletteSubmitStatus.待定).ToList();
                toResolvedSubmits.ForEach(r =>
                {
                    var ssc = sscList.FirstOrDefault(p => r.createTime >= p.startTime && r.createTime < p.endTime);
                    if (ssc == null) return;
                    r.sscPeriod = ssc.period;
                    r.ssc_no = ssc.no;
                });

                var roulette_property_types = CacheHelper.Get("roulette_property_types", () => dc.roulette_property_types.ToList());
                var roulette_propertys = CacheHelper.Get("roulette_propertys", () => dc.roulette_propertys.ToList());
                var roulette_heros = CacheHelper.Get("roulette_heros", () => dc.roulette_heros.ToList());
                var roulette_hero_propertys = CacheHelper.Get("roulette_hero_propertys", () => dc.roulette_hero_propertys.ToList());

                var heroCount = roulette_heros.Count(r => r.game_typeId == GameTypeId);
                var hps = (from a in roulette_heros
                           where a.game_typeId == GameTypeId
                           select new
                           {
                               a.roulette_heroId,
                               propertys = roulette_hero_propertys.Where(r => r.roulette_heroId == a.roulette_heroId).Select(r => r.roulette_propertyId)
                           }).ToList();
                for (int i = 0; i < toResolvedSubmits.Count(); i++)
                {
                    var trs = toResolvedSubmits[i];
                    if (string.IsNullOrWhiteSpace(trs.ssc_no)) continue;
                    var selectedPropertys = JsonHelper.Deserialize<int[]>(trs.propertys);
                    var matchedHeroCount = hps.Count(hp => selectedPropertys.All(s => hp.propertys.Contains(s)));
                    if (matchedHeroCount == 0) continue;
                    trs.matched_hero_count = matchedHeroCount;
                    trs.total_hero_count = hps.Count();
                    trs.odds = (double)trs.total_hero_count / (double)matchedHeroCount;
                    trs.ssc_no = sscList.FirstOrDefault(r => r.period == trs.sscPeriod).no;
                    trs.correct_heroId = roulette_heros.OrderBy(r => r.roulette_heroId).Where(r => r.game_typeId == GameTypeId)
                            .Skip(int.Parse(trs.ssc_no) % trs.total_hero_count).FirstOrDefault().roulette_heroId;
                    var correct_propertys = hps.FirstOrDefault(hp => hp.roulette_heroId == trs.correct_heroId).propertys;
                    trs.correct_propertys = JsonHelper.Serialize(correct_propertys);
                    var isCorrect = selectedPropertys.All(sp => correct_propertys.Contains(sp));
                    trs.rouletteSubmitStatus = isCorrect ? Wx.Enums.RouletteSubmitStatus.正确 : Wx.Enums.RouletteSubmitStatus.错误;
                    if (isCorrect)
                    {
                        var user = dc.users.FirstOrDefault(r => r.userId == trs.userId);
                        var changeAmount = trs.golds * trs.odds * 0.9;
                        user.golds += changeAmount;
                        var ugr = new user_golds_record
                        {
                            amount = user.golds,
                            changeAmount = changeAmount,
                            recordTime = DateTime.Now,
                            userId = user.userId,
                            detail = $"英雄猜 {trs.sscPeriod}期"
                        };
                        dc.user_golds_records.Add(ugr);
                    }
                    dc.SaveChanges();
                }
            }
        }

        public void Resolve(bool force)
        {
            if (!force && LastResolvingTime.AddMinutes(5) > DateTime.Now) return;
            LastResolvingTime = DateTime.Now;
            this.Resolve(DateTime.Now.Date, 1);
            this.Resolve(DateTime.Now.Date, 2);
        }
    }
}

