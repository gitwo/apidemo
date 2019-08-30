using Demo.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.App_Start.Handler
{
	/// <summary>
	/// API自定义错误消息处理委托类。
	/// 用于处理访问不到对应API地址的情况，对错误进行自定义操作。
	/// </summary>
	public class CustomErrorMessageDelegatingHandler : DelegatingHandler
	{
		// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return base.SendAsync(request, cancellationToken).ContinueWith((responseToCompleteTask) =>
			{
				HttpResponseMessage response = responseToCompleteTask.Result;
				if (!response.IsSuccessStatusCode)
				{
					//添加自定义错误处理
					//error.Message = "Your Customized Error Message";
					var formatKv = request.GetQueryNameValuePairs().Where(i => i.Key == "format").FirstOrDefault();
					var resModel = new ApiResult() { ResultCode = (ApiResultCode)response.StatusCode };
					MediaTypeFormatter mediaType;
					if (formatKv.Value == null || formatKv.Value.ToLower() == "json")
						mediaType = new JsonMediaTypeFormatter();
					else
						mediaType = new XmlMediaTypeFormatter();
					var responseError = request.CreateResponse(HttpStatusCode.OK, resModel, mediaType);
					return responseError;
				}
				else
				{
					return response;
				}
			});
		}
	}
}