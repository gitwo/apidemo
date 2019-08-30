using System.ComponentModel;
using System.Runtime.Serialization;
using Util;

namespace Demo.Models
{
	[DataContract]
	public class ApiResult
	{
		/// <summary>
		/// 初始化
		/// </summary>
		public ApiResult()
		{
			ResultCode = ApiResultCode.Success;
		}

		/// <summary>
		/// 状态码
		/// </summary>
		[DataMember]
		public int Code
		{
			private set { }
			get
			{
				return (int)ResultCode;
			}
		}

		/// <summary>
		/// 状态
		/// </summary>        
		public ApiResultCode ResultCode { set; get; }

		private string _message;
		/// <summary>
		/// 错误信息
		/// </summary>
		[DataMember]
		public string Message
		{
			set
			{
				_message = value;
			}
			get
			{
				if (string.IsNullOrEmpty(_message))
				{
					_message = ResultCode.GetDescriptionOriginal();					
				}
				return _message;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DataContract]
	public class ApiResult<T> : ApiResult
	{
		/// <summary>
		/// 数据包
		/// </summary>
		[DataMember]
		public T Data { set; get; }
	}


	/// <summary>
	/// api返回code编码
	/// </summary>
	public enum ApiResultCode
	{
		//公共返回代码
		/// <summary>
		/// 请求成功
		/// </summary>
		[Description("Success")]
		Success = 0,
		/// <summary>
		/// 错误的请求
		/// </summary>
		[Description("错误的请求")]
		BadRequest = 400,
		/// <summary>
		/// 授权验证失败
		/// </summary>
		[Description("授权验证失败")]
		Unauthorized = 401,
		/// <summary>
		/// 无法找到请求的资源
		/// </summary>
		[Description("无法找到请求的资源")]
		NotFound = 404,
		/// <summary>
		/// 请求超时
		/// </summary>
		[Description("请求超时")]
		RequestTimeout = 408,
		/// <summary>
		/// 太多请求
		/// </summary>
		[Description("请求太过频繁")]
		TooManyRequests = 429,
		/// <summary>
		/// 服务器错误
		/// </summary>
		[Description("服务器错误")]
		InternalServerError = 500,
		/// <summary>
		/// 不支持请求类型
		/// </summary>
		[Description("不支持请求类型")]
		NotImplemented = 501,

		//会员相关返回代码
		/// <summary>
		/// 操作失败
		/// </summary>
		[Description("操作失败")]
		OperatFail = 1000,
		/// <summary>
		/// 会员不存在
		/// </summary>
		[Description("帐号不存在")]
		UserNotExist = 1001,
		/// <summary>
		/// Token已过期
		/// </summary>
		[Description("Token已过期")]
		TokenExpired = 1002,
		/// <summary>
		/// Token不存在
		/// </summary>
		[Description("Token不存在")]
		TokenNotExist = 1003,
	}
}