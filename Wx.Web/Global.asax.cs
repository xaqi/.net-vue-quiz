using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using Senparc.Weixin.MP.TenPayLib;
using Senparc.Weixin.MP.TenPayLibV3;

namespace Wx.Web
{
	public class Global : HttpApplication
	{
		public override void Init()
		{
			PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
			base.Init();
		}
		void Application_Start(object sender, EventArgs e)
		{
			// Code that runs on application startup
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			RegisterGlobalFilters(GlobalFilters.Filters);

			this.RegisterWeixinPay();
		}
		void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
		{
			HttpContext.Current.SetSessionStateBehavior(
				SessionStateBehavior.Required);
		}
		void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new Wx.Web.App_Start.Filters.AuthFilter());
		}

		private void RegisterWeixinPay()
		{
			//提供微信支付信息
			var weixinPay_PartnerId = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_PartnerId"];
			var weixinPay_Key = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_Key"];
			var weixinPay_AppId = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_AppId"];
			var weixinPay_AppKey = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_AppKey"];
			var weixinPay_TenpayNotify = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_TenpayNotify"];

			var tenPayV3_MchId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"];
			var tenPayV3_Key = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_Key"];
			var tenPayV3_AppId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"];
			var tenPayV3_AppSecret = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppSecret"];
			var tenPayV3_TenpayNotify = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_TenpayNotify"];

			var weixinPayInfo = new TenPayInfo(weixinPay_PartnerId, weixinPay_Key, weixinPay_AppId, weixinPay_AppKey, weixinPay_TenpayNotify);
			TenPayInfoCollection.Register(weixinPayInfo);
			var tenPayV3Info = new TenPayV3Info(tenPayV3_AppId, tenPayV3_AppSecret, tenPayV3_MchId, tenPayV3_Key,
												tenPayV3_TenpayNotify);
			TenPayV3InfoCollection.Register(tenPayV3Info);
		}
	}
}