using Demo.App_Start.Handler;
using Newtonsoft.Json.Converters;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiThrottle;

namespace Demo
{
	/// <summary>
	/// 
	/// </summary>
	public static class WebApiConfig
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="config"></param>
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
				Policy = ThrottlePolicy.FromStore(new PolicyConfigurationProvider()),//在config中定义限制策略
				Repository = new ThrottleRepository()
			});

			/*
			 * 直接配置形式
			new ThrottlePolicy(perSecond: 1, perMinute: 20, perHour: 200)
			{
				IpThrottling = true,
				IpRules = new System.Collections.Generic.Dictionary<string, RateLimits> {
					{ "192.168.1.1", new RateLimits { PerSecond = 2 } },
					{ "192.168.2.0/24", new RateLimits { PerMinute = 30, PerHour = 30*60, PerDay = 30*60*24 } }
				},
				ClientThrottling = true,
				ClientRules = new System.Collections.Generic.Dictionary<string, RateLimits>
				{
					 { "api-client-key-1", new RateLimits { PerMinute = 40, PerHour = 400 } },
					 { "api-client-key-9", new RateLimits { PerDay = 2000 } }
				},
				EndpointThrottling = true,
				EndpointRules = new System.Collections.Generic.Dictionary<string, RateLimits>
				{
					 { "api/search", new RateLimits { PerSecond = 10, PerMinute = 100, PerHour = 1000 } }
				},
				StackBlockedRequests = true,//把被拒绝的请求也计算到其他的计数器里(分钟、小时、天)
			};
			*/
		}
	}
}
