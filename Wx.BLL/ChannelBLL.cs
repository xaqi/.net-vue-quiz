using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.DAL;
using Wx;
using System.Transactions;

namespace Wx.BLL
{
    public class ChannelBLL
    {
        public void SaveInviteLog(string openId, int invite_by_channel_id, string event_type)
        {
            using (var dc = new DC())
            {
                dc.user_invite_logs.Add(new user_invite_log { openId = openId, invite_by_channel_id = invite_by_channel_id, event_type = event_type, createTime = DateTime.Now });
                dc.SaveChanges();
                var invitedUser = dc.users.FirstOrDefault(r => r.openId == openId);
                if (invitedUser != null)
                {
                    invitedUser.invite_by_channelId = invite_by_channel_id;
                    if (invitedUser.status == "取消关注")
                    {
                        invitedUser.status = "再次关注";
                    }
                }
                var channel = dc.user_invite_channels.FirstOrDefault(r => r.user_invite_channelId == invite_by_channel_id);
                if (channel != null)
                {
                    channel.user_count++;
                }
                dc.SaveChanges();
            }
        }

        public user_invite_channel GetChannel(int channelId)
        {

            using (var dc = new DC())
            {
                var channel = dc.user_invite_channels.FirstOrDefault(r => r.user_invite_channelId == channelId);
                return channel;

            }
        }


        public string GetInviteQrCode(int channelId)
        {

            using (var dc = new DC())
            {
                var channel = dc.user_invite_channels.FirstOrDefault(r => r.user_invite_channelId == channelId);
                if (string.IsNullOrWhiteSpace(channel.qr_ticket))
                {
                    var appId = System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"];
                    var secret = System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"];
                    var qrTicket = Wx.Common.Helpers.QRCodeHelper.QrCode(appId, secret, "c=" + channelId);
                    if (string.IsNullOrWhiteSpace(qrTicket.ticket))
                    {
                        return "";
                    }
                    channel.qr_ticket = qrTicket.ticket;
                    channel.qr_url = qrTicket.url;
                    channel.qr_create_time = DateTime.Now;
                    dc.SaveChanges();
                }
                return $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={channel.qr_ticket}";
            }
        }


    }
}
