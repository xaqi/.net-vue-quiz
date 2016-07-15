using System.Web.Mvc;

namespace Wx.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Admin_default",
				"Admin/{controller}/{action}/{id}",
				new { controller = "home", action = "Index", id = UrlParameter.Optional },
				namespaces: new string[] { "Wx.Web.Areas.Admin.Controllers" }
			);
		}
	}
}