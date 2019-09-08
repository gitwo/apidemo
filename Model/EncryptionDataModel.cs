using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class EncryptionDataModel
	{
		/// <summary>
		/// 待加密的模型数据
		/// </summary>
		public object DataModel { get; set; }
		/// <summary>
		/// unix时间
		/// </summary>
		public long UnixTime { get; set; }
		/// <summary>
		/// 附加信息
		/// </summary>
		[JsonIgnore]
		public string AdditionInfo { get; set; }
	}
}
