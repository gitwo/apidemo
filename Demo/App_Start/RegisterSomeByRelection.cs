using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Demo.App_Start
{
	/// <summary>
	/// 反射获取控制器特性
	/// </summary>
	public static class RegisterSomeByRelection
	{
		/// <summary>
		/// 此扩展方法
		/// </summary>
		/// <param name="httpConfiguration"></param>
		public static void RegisterAssembly(this HttpConfiguration httpConfiguration)
		{
			IEnumerable<Assembly> assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(x => x.GetName().Name.StartsWith("BLL"));
			foreach (Assembly assembly in assemblies)
			{  //查找拥有特定特性路由的控制器类
				List<Type> types = assembly.GetTypes().Where(x => x.IsDefined(typeof(RegRouteHandlerAttribute))).ToList();
				foreach (Type type in types)
				{
					//获取特性路由对象
					RegRouteHandlerAttribute attribute = type.GetCustomAttribute<RegRouteHandlerAttribute>();
					//创建handler对象
					DelegatingHandler handler = Activator.CreateInstance(type) as DelegatingHandler;
					//创建自定义消息管道
					HttpMessageHandler httpHandlers = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(httpConfiguration), new DelegatingHandler[] { handler });
					//注册路由
					httpConfiguration.Routes.MapHttpRoute(
						name: attribute.ControllerName,
						routeTemplate: attribute.Template,
						defaults: new { controller = attribute.ControllerName },
						constraints: null,
						handler: httpHandlers
						);
				}
			}
		}



	}

	/// <summary>
	/// 自定义路由特性
	/// </summary>
	public class RegRouteHandlerAttribute : Attribute
	{
		/// <summary>
		/// 控制器名称
		/// </summary>
		public string ControllerName { get; set; }
		/// <summary>
		/// 路径模板【api/{controller}/{action}】
		/// </summary>
		public string Template { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="controllerName"></param>
		/// <param name="template"></param>
		public RegRouteHandlerAttribute(string controllerName, string template = null)
		{
			ControllerName = controllerName;
			Template = template ?? string.Concat("api/", controllerName, "/{action}");
		}
	}
}