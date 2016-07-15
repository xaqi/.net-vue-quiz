/*----------------------------------------------------------------
    Copyright (C) 2016 Senparc
    
    文件名：CustomMessageHandler_Events.cs
    文件功能描述：自定义MessageHandler
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Sample.CommonService.Download;
using Senparc.Weixin.MP.Sample.CommonService.Utilities;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        /// 订阅（关注）事件
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content += LogInvite(requestMessage.EventKey, requestMessage.FromUserName, "Scan");
            return responseMessage;
        }
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //通过扫描关注
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = LogInvite(requestMessage.EventKey, requestMessage.FromUserName, "Scan");
            return responseMessage;
        }

        private string LogInvite(string eventKey, string fromUserName, string eventType)
        {
            //http://domain/lol/coin/pay
            string welcome = $@"感谢关注小菠！[亲亲]
这里有一大波LOLer&Dotaer在跟小菠一起玩竞猜，变土豪！
同时，小菠还为你准备了好多大礼包哦~
1.首次登录即送88金币[愉快]<a href=""http://domain/lol/"">【戳我立即领金币】</a>
2.首次充值即可获得“首充大礼包”[礼物]<a href=""http://domain/lol/coin/pay"">【戳我马上领取】</a>
3.如果你想介绍新朋友给小菠，小菠就把自己的绝世宝箱送给你[害羞]<a href=""http://domain/lol/"">【戳我打开绝世宝箱】</a>

偷偷告诉你：
每天小菠都会送你30-70金币哦[嘘]";


            welcome = $@"感谢关注小菠！[亲亲]
这里有一大波LOLer&Dotaer在跟小菠一起玩竞猜，变土豪！
同时，小菠还为你准备了好多大礼包哦~
1.首次登录即送88金币[愉快]
2.首次充值即可获得“首充大礼包”[礼物]
3.如果你想介绍新朋友给小菠，小菠就把自己的绝世宝箱送给你[害羞]

偷偷告诉你：
每天小菠都会送你30-70金币哦[嘘]";
            var content = eventKey;
            if (!string.IsNullOrWhiteSpace(eventKey))
            {
                var enviteType = eventKey[0].ToString();
                switch (enviteType)
                {
                    case "u":
                        var ubll = new Wx.BLL.UserBLL();
                        var inviteByUserId = int.Parse(eventKey.Replace("u=", ""));
                        ubll.SaveInviteLog(fromUserName, inviteByUserId, eventType);
                        var inviteByUser = ubll.GetUser(inviteByUserId);
                        var inviteUserName = inviteByUser == null ? "unknown" : inviteByUser.name;
                        if (inviteByUser.openId == fromUserName)
                        {
                            content = $"请将二维码发送给好友或分享至朋友圈。";
                        }
                        else
                        {
                            content += $"您的推荐人是[{inviteUserName}]。";
                        }
                        break;

                    case "c":
                        var cbll = new Wx.BLL.ChannelBLL();
                        var inviteByChannelId = int.Parse(eventKey.Replace("c=", ""));
                        cbll.SaveInviteLog(fromUserName, inviteByChannelId, eventType);
                        var inviteByChannel = cbll.GetChannel(inviteByChannelId);
                        var inviteChannelName = inviteByChannel == null ? "unknown" : inviteByChannel.name;
                        content += $"您来自推广渠道：[{inviteChannelName}]。";
                        break;


                }
            }
            return welcome;
        }
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "取消关注";
            var ubll = new Wx.BLL.UserBLL();
            var openId = requestMessage.FromUserName;
            ubll.Unsubscribe(openId);

            return responseMessage;
        }

    }



}