#region Assembly System.Web.Http.Owin.dll, v5.2.3.0
// c:\users\aqi\documents\visual studio 2013\Projects\WebApplication6\packages\Microsoft.AspNet.WebApi.Owin.5.2.3\lib\net45\System.Web.Http.Owin.dll
#endregion

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wx.Common.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using System.Configuration;
using System.Web.Http.Controllers;
using System.Net.Http;
using Wx.Common;
using Wx.Common.Helpers;

namespace Wx.Web.App_Start.Filters
{
    // Summary:
    //     Represents an authentication filter that authenticates via OWIN middleware.
    public class ApiAuthFilter : System.Web.Http.AuthorizeAttribute, IFilter
    {
        SessionHelper SessionHelper = new Common.Helpers.SessionHelper();
        bool IsTestMode()
        {
			return true;
            if (System.IO.File.Exists(@"E:\evn_local.txt")) return true;
            var Request = HttpContext.Current.Request;
            var mode = Request.Cookies.Get("mode");
            if (mode != null && !string.IsNullOrWhiteSpace(mode.Value) && mode.Value == "test")
            {
                return true;
            }
            return false;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //try
            //{
            //	if (HttpContext.Current != null && SessionHelper["UserInfo"] == null)
            //	{
            //		var obj = new ReturnMessage { success = false, message = "not authed." };
            //		var json = JsonHelper.Serialize(obj);
            //		HttpContext.Current.Response.Write(json);
            //		actionContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            //	}
            //}
            //catch (Exception ex)
            //{

            //}

            if (IsTestMode() && SessionHelper["UserInfo"] == null)
            {
                var ubll = new Wx.BLL.UserBLL();
                SessionHelper["UserInfo"] = ubll.FromDbUser(ubll.TestUser);
            }

            if (HttpContext.Current != null && SessionHelper["UserInfo"] == null)
            {
                base.OnAuthorization(actionContext);
            }

        }
    }
}
