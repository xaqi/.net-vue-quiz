using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wx.Portal.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.version = DateTime.Now.ToString("MMddhhmmss");
            ViewBag.UserInfo = Session["UserInfo"];
            return View();
        }
        public ActionResult Sample()
        {
            ViewBag.version = DateTime.Now.ToString("MMddhhmmss");
            ViewBag.UserInfo = Session["UserInfo"];
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var userId = int.Parse(Request.Form["username"]);
            var password = Request.Form["password"];
            if (password == "admin++")
            {
                var user = new Wx.BLL.UserBLL().GetUser(userId);
                if (user == null)
                {
                    return View();
                }
                Session["UserInfo"] = new UserInfo { header = user.header, openId = user.openId, userId = user.userId, name = user.name };
                Response.Redirect("~/");
                return null;
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session["UserInfo"] = null;
            Response.Redirect("~/");
            return null;
        }
    }
}