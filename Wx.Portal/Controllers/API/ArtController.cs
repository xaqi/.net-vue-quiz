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
    public class ArtController : ApiController
    {
        public object getList(int ps = 10, int p = 1)
        {
            using (var dc = new DC())
            {
                var q = dc.sys_arts.OrderBy(r => r.sys_artId).Select(r => new
                {
                    r.sys_artId,
                    r.name,
                    r.search_value,
                    r.content,
                    r.update_time,
                    r.create_time
                });
                return q.ToPagedList();
            }

        }

        public object save(Dictionary<string, object> dict)
        {
            var art = JsonHelper.Deserialize<sys_art>(JsonHelper.Serialize(dict));
            using (var dc = new DC())
            {
                if (art.sys_artId > 0)
                {
                    var row = dc.sys_arts.FirstOrDefault(r => r.sys_artId == art.sys_artId);
                    row.name = art.name;
                    row.content = art.content;
                    row.create_time = DateTime.Now;
                    row.update_time = DateTime.Now;

                }
                else
                {
                    //dc.sys_arts.Add(art);
                }
                dc.SaveChanges();
            }
            return dict;

        }



    }
}
