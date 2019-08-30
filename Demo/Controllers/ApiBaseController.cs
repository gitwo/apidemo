using Demo.App_Start.Handler;
using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Demo.Controllers
{
	[HawkNet.WebApi.HawkAuthentication(typeof(HawkCredentialRepository))]
	[Authorize]
	public class ApiBaseController : ApiController
	{
		/// <summary>
		/// 额外头部参数
		/// </summary>
		protected int hdo;
		protected static int[] hdos = { 1, 2, 3, 4, 5 };
		/// <summary>
		/// 公共进入方法
		/// </summary>
		/// <param name="controllerContext"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
		{
			try
			{
				var headerSite = controllerContext.Request.Headers.FirstOrDefault(q => q.Key == "hdo");
				if (headerSite.Value != null && headerSite.Value.Count() > 0)
				{
					hdo = Convert.ToInt32(headerSite.Value.First());
				}
				else
				{
					var thdo = controllerContext.Request.GetQueryNameValuePairs().FirstOrDefault(k => k.Key == "hdo");
					hdo = Convert.ToInt32(thdo.Value);
				}

			}
			catch (Exception)
			{
				hdo = 1;
			}

			System.Runtime.Remoting.Messaging.CallContext.SetData("hdo", hdo);
			//设置语言
			string lang = "zh-CN";
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
			//判断头参是否在指定范围
			if (!hdos.Contains(hdo))
			{
				return Task.FromResult(controllerContext.Request.CreateResponse(HttpStatusCode.NotFound));
			}
			return base.ExecuteAsync(controllerContext, cancellationToken);
		}

		/// <summary>
		/// 公共返回成功方式
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[NonAction]
		public ApiResult<T> Success<T>(T data)
		{
			return new ApiResult<T>() { ResultCode = ApiResultCode.Success, Data = data };
		}

		/// <summary>
		/// 公共返回成功方式
		/// </summary>
		/// <returns></returns>
		[NonAction]
		public ApiResult Success()
		{
			return new ApiResult() { ResultCode = ApiResultCode.Success };
		}

		/// <summary>
		/// 公共错误返回方式
		/// </summary>
		/// <param name="resultCode"></param>
		/// <param name="message"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		[NonAction]
		public ApiResult<T> Error<T>(ApiResultCode resultCode, string message = null, dynamic data = null)
		{
			return new ApiResult<T>() { ResultCode = resultCode, Message = message, Data = data };
		}

		/// <summary>
		/// 公共错误返回方式
		/// </summary>
		/// <param name="resultCode"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[NonAction]
		public ApiResult Error(ApiResultCode resultCode, string message = null)
		{
			return new ApiResult() { ResultCode = resultCode, Message = message };
		}
	}
}
