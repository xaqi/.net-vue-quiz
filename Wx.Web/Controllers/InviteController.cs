using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wx.BLL;

namespace Wx.Web.Controllers
{
    public class InviteController : Controller
    {
        //[AllowAnonymous]
        public ActionResult Index(int userId)
        {
            var ubll = new UserBLL();
            var user = ubll.GetUser(userId);
            var inviteQrCode = new UserBLL().GetInviteQrCode(userId);
            ViewBag.inviteQrCode = inviteQrCode;
            return View();
            //return Content(userId.ToString());
        }
    }
}