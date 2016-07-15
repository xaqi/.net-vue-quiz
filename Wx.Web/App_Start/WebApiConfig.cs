using System;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Web.Http.Cors;
using Wx.Web.App_Start.Filters;

namespace Wx.Web
{
    public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// OPTION Verb Support
			var cors = new EnableCorsAttribute("*", "*", "*");
			config.EnableCors(cors);
			// Web API configuration and services

			config.Filters.Add(new ApiAuthFilter());

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{gameType}/{controller}/{action}",
				defaults: new { }
			);
			var jsonFormatter = new JsonMediaTypeFormatter();
			config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));
		}
		public class JsonContentNegotiator : IContentNegotiator
		{
			private readonly JsonMediaTypeFormatter _jsonFormatter;

			public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
			{
				_jsonFormatter = formatter;
			}
			public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, System.Collections.Generic.IEnumerable<MediaTypeFormatter> formatters)
			{
				var result = new ContentNegotiationResult(_jsonFormatter, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
				return result;
			}
		}
	}
}
