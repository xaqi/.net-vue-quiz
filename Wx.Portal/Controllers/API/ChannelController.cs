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
    public class ChannelController : ApiController
    {
        public object getList(int ps = 10, int p = 1)
        {
            using (var dc = new DC())
            {
                var q = dc.user_invite_channels.OrderBy(r => r.user_invite_channelId).Select(r => new
                {
                    r.user_invite_channelId,
                    r.name,
                    r.order,
                    r.qr_url,
                    r.qr_ticket,
                    qr_img = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + r.qr_ticket,
                    r.user_count
                });
                return q.ToPagedList();
            }

        }

        public object save(Dictionary<string, object> dict)
        {
            var art = JsonHelper.Deserialize<user_invite_channel>(JsonHelper.Serialize(dict));
            using (var dc = new DC())
            {
                if (art.user_invite_channelId > 0)
                {
                    var row = dc.user_invite_channels.FirstOrDefault(r => r.user_invite_channelId == art.user_invite_channelId);
                    row.name = art.name;
                    row.order = art.order;
                }
                else
                {
                    dc.user_invite_channels.Add(art);
                    dc.SaveChanges();
                    new ChannelBLL().GetInviteQrCode(art.user_invite_channelId);
                }
                dc.SaveChanges();
            }
            return dict;

        }



    }
}
