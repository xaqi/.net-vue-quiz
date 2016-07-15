using System;
using System.Web.Mvc;
using System.Configuration;
using Wx.Common.Helpers;
using Wx.BLL;

namespace Wx.Web.Controllers
{
    public class HomeController : Controller
    {
        SessionHelper SessionHelper = new SessionHelper();
        public UserInfo CurrentUser
        {
            get
            {
                var sessionUser = SessionHelper["UserInfo"];
                if (sessionUser == null) return null;
                var userInfo = (UserInfo)sessionUser;
                return userInfo;
            }
        }

        // GET: Home
        public ActionResult GameIndex()
        {
            ViewBag.version = ConfigurationManager.AppSettings["version"].ToString();
            ViewBag.bundleVersion = ConfigurationManager.AppSettings["bundleVersion"].ToString();
            //ViewBag.version = DateTime.Now.ToString("MMddHHmmss");
            ViewBag.gameType = Request.Url.ToString().ToLower().Contains("lol") ? "lol" : "dota";
            ViewBag.baseHref = "/" + ViewBag.gameType + "/";
            var cssPath = "/content/front/design/hero/style/css";
            var imgPath = "/content/front/design/hero/images";
            if (ViewBag.gameType == "dota")
            {
                cssPath = "/content/front/design/style/css";
                imgPath = "/content/front/design/images";
            }
            ViewBag.cssPath = cssPath;
            ViewBag.imgPath = imgPath;

            ViewBag.loginInfo = new UserBLL().GetLoginInfo(CurrentUser.userId);
            ViewBag.DailyLoginBonusConfigs = new UserBLL().GetDailyLoginBonusConfigs(CurrentUser.userId);
            ViewBag.IsShowDailyPop = true;
            return View("Vue");
        }

        [AllowAnonymous]
        public ActionResult Public_Login()
        {
            ViewBag.version = System.Configuration.ConfigurationManager.AppSettings["version"].ToString();
            ViewBag.version = DateTime.Now.ToString("MMddHHmmss");
            ViewBag.baseHref = "/";
            var cssPath = "/content/front/design/hero/style/css";
            var imgPath = "/content/front/design/images";
            ViewBag.cssPath = cssPath;
            ViewBag.imgPath = imgPath;
            return View("Vue");
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
			Response.Redirect("/lol");
			return null;


        }
    }
}