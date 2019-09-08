using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Util;

namespace Demo.App_Start.Handler
{
	/// <summary>
	/// 消息管道实现限制
	/// </summary>
	public class RestrictMessageHandler : DelegatingHandler
	{
		private string whiteIp;
		private string secretKey;
		private string aesKey;
		private bool isOpen;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="whiteIp"></param>
		/// <param name="secretKey"></param>
		/// <param name="aesKey"></param>
		/// <param name="isOpen"></param>
		public RestrictMessageHandler(string whiteIp, string secretKey, string aesKey, bool isOpen)
		{
			this.whiteIp = whiteIp;
			this.secretKey = secretKey;
			this.aesKey = aesKey;
			this.isOpen = isOpen;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//判断请求头
			bool isValidRequest = request.Headers.Contains("");
			EncryptionDataModel model = null;

			if (isValidRequest)
			{
				#region 白名单验证
				if (!ValidateHelper.ValidateWhiteIp(whiteIp))
				{
					return await SetResponse(request, ApiStatusCodeEnum.MisMatchedSignature, "Ip受限");
				}
				#endregion

				#region 验证签名
				//加密数据
				string requestContent = await request.Content.ReadAsStringAsync();
				//api 请求头签名部分
				if (!ValidateHelper.ValidateSignatureHeader(request, requestContent, secretKey))
				{
					//异步方法必须返回类型，不能返回Task<HttpResponseMessage>
					return await SetResponse(request, ApiStatusCodeEnum.MisMatchedSignature, "签名错误");
				}
				//api 请求data验证签名部分			
				if (!ValidateHelper.ValidateAes(requestContent, aesKey, out model))
				{
					return await SetResponse(request, ApiStatusCodeEnum.MisMatchedSignature, "数据包签名错误");
				}
				#endregion

				#region 时间验证
				if (!ValidateHelper.ValidateTimeStamp(model.UnixTime, 3))
				{
					return await SetResponse(request, ApiStatusCodeEnum.TimeOut, "超时");
				}

				#endregion

			}
			//记录日志用
			string controller = request.GetRouteData().Values["controller"].ToString();
			string action = request.GetRouteData().Values["action"].ToString();

			//请求构造
			request.Content = new StringContent(model.AdditionInfo);
			request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				HttpError httpError = null;
				if (httpResponseMessage.TryGetContentValue(out httpError))
				{
					//记录日志
				}
				var response = new ResponseModel(ApiStatusCodeEnum.InternalError, httpResponseMessage.ReasonPhrase);
				httpResponseMessage.Content = new ObjectContent<ResponseModel>(response, new System.Net.Http.Formatting.JsonMediaTypeFormatter());

			}

			return httpResponseMessage;
		}

		/// <summary>
		/// 设置请求响应
		/// </summary>
		/// <param name="request">请求</param>
		/// <param name="code">Api状态码</param>
		/// <param name="description">api请求的描述</param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> SetResponse(HttpRequestMessage request, ApiStatusCodeEnum code, string description)
		{
			string requestStr = await request.Content.ReadAsStringAsync();
			//记录日志用
			string controller = request.GetRouteData().Values["controller"].ToString();
			string action = request.GetRouteData().Values["action"].ToString();
			if (string.IsNullOrEmpty(requestStr))
			{
				requestStr = request.RequestUri.ToString();
			}
			ResponseModel model = new ResponseModel(code, description);
			HttpResponseMessage response = request.CreateResponse(model.HttpStatusCode, model);
			TaskCompletionSource<HttpResponseMessage> tcs = new TaskCompletionSource<HttpResponseMessage>();
			tcs.SetResult(response);
			return await tcs.Task;
		}
	}
}