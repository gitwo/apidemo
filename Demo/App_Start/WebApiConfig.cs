using Demo.App_Start.Handler;
using Newtonsoft.Json.Converters;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiThrottle;

namespace Demo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API 配置和服务

			// Web API 路由
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional });


			//添加自定义异常处理返回
			config.MessageHandlers.Add(new CustomErrorMessageDelegatingHandler());

			var format = GlobalConfiguration.Configuration.Formatters;
			format.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter
			{
				DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
			});
			//json序列化null数据忽略
			format.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

			//清除默认xml
			format.XmlFormatter.SupportedMediaTypes.Clear();

			//通过参数设置返回格式
			format.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("format", "json", "application/json"));
			format.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("format", "xml", "application/xml"));

			//api限流实现
			config.MessageHandlers.Add(new ThrottlingHandler()
			{
				Policy = ThrottlePolicy.FromStore(new PolicyConfigurationProvider()),
				Repository = new ThrottleRepository()
			});

		}
	}
}
