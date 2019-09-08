using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class ResponseModel
	{
		public ResponseModel()
		{

		}
		public ResponseModel(ApiStatusCodeEnum status,string des)
		{
			this.ApiStatusCode = status;
			this.Description = des;
			this.HttpStatusCode = HttpStatusCode.OK;
		}
		/// <summary>
		/// Api状态
		/// </summary>
		[JsonIgnore]
		public ApiStatusCodeEnum ApiStatusCode { get; set; }
		public HttpStatusCode HttpStatusCode { get; set; }
		/// <summary>
		/// 返回数据的各种结果码
		/// </summary>
		[JsonProperty(PropertyName = "code")]
		public int Code { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

	}

	public class ResponseModel<T> : ResponseModel
	{
		/// <summary>
		/// 返回主体数据
		/// </summary>
		public T Data { get; set; }
	}
}
