using System;
using System.Web.Mvc;
using System.Configuration;
using Wx.Common.Helpers;
using Wx.BLL;

namespace Wx.Web.Controllers
{
    public class SysController : Controller
    {

        [AllowAnonymous]
        public ActionResult ClearCache()
        {
            Wx.Common.Helpers.CacheHelper.ClearAll();
            return Content("success");


        }
    }
}