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
    public class UserController : ApiController
    {
        public object getList(int ps = 10, int p = 1)
        {
            using (var dc = new DC())
            {
                var q = (from a in dc.users
                         join b in dc.users on a.invite_by_userId equals b.userId into ab
                         from b in ab.DefaultIfEmpty()
                         join c in dc.user_invite_channels on a.invite_by_channelId equals c.user_invite_channelId into ac
                         from c in ac.DefaultIfEmpty()
                         orderby a.userId
                         select new
                         {
                             a.userId,
                             a.header,
                             a.golds,
                             a.openId,
                             a.last_login_time,
                             a.first_login_time,
                             a.status,
                             invite_user = b.name,
                             invite_channel = c.name
                         }) as IQueryable<object>;
                return q.ToPagedList();
            }

        }

        public object EditUserGolds(Dictionary<string, object> dict)
        {
            var openId = dict["openId"].ToString();
            var changeAmount = int.Parse(dict["changeAmount"].ToString());
            var mark = dict["mark"].ToString();

            if (string.IsNullOrWhiteSpace(mark))
            {
                return new ReturnMessage { success = false, message = "备注不能为空！" };

            }
            var gbll = new GoldsBLL();
            gbll.EditUserMoney(openId, changeAmount, mark);
            return new ReturnMessage { success = true, message = "修改成功！", data = dict };
        }

    }
}
