using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wx.Portal.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo CurrentUser
        {
            get
            {
                return (UserInfo)Session["UserInfo"];
            }
        }
    }


    public class UserInfo
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string header { get; set; }
        public string openId { get; set; }
    }
}