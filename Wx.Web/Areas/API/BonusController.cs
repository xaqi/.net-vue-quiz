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
    public class BonusController : BaseApiController
    {
        public object GetNewUserGolds()
        {
            var bbll = new BonusBLL();
            return bbll.GetNewUserGolds(CurrentUser.userId);
        }

        public object GetDailyLoginGolds()
        {
            var bbll = new BonusBLL();
            return bbll.GetDailyLoginGolds(CurrentUser.userId);
        }

    }

}
