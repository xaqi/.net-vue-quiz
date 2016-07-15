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
    public class TeamController : ApiController
    {
        public object getList(int ps = 10, int p = 1)
        {
            using (var dc = new DC())
            {
                var q = dc.teams.Select(r => new { r.name, r.teamId, r.avatar })
                .OrderBy(r => r.teamId);
                return q.ToPagedList();
            }

        }

        public object save(Dictionary<string, object> dict)
        {
            var art = JsonHelper.Deserialize<team>(JsonHelper.Serialize(dict));
            using (var dc = new DC())
            {
                if (art.teamId > 0)
                {
                    var row = dc.teams.FirstOrDefault(r => r.teamId == art.teamId);
                    row.name = art.name;
                    row.avatar = art.avatar;
                }
                else
                {
                    dc.teams.Add(art);
                    dc.SaveChanges();
                }
                dc.SaveChanges();
            }
            return dict;

        }
    }
}
