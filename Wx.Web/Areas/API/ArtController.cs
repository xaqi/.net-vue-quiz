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
    public class ArtController : BaseApiController
    {
        [HttpGet]
        public object GetArt(string url)
        {
            using (var dc = new DC())
            {
                var arts = Wx.Common.Helpers.CacheHelper.Get("sys_arts", () => dc.sys_arts.ToList());
                var art = arts.FirstOrDefault(r => r.search_value == url.ToLower());
                var html = art == null ? "" : art.content;
                var dict = GetTodaySummaryAndUserBalance();
                dict["html"] = html;
                dict["url"] = url;
                return new ReturnMessage { success = true, data = dict };

            }
        }

        [HttpGet]
        public object GetBetAbout()
        {
            return this.GetArt($"bet/about/{GameType.name}");
        }

        [HttpGet]
        public object GetRouletteAbout()
        {
            return this.GetArt($"{GameType.name}/roulette/about");
        }

    }


}
