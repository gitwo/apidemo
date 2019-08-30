using System;
using System.Runtime.Serialization;

namespace Demo.Models
{
	/// <summary>
	/// 用户账单请求模型
	/// </summary>
	[DataContract]
	public class BillReqModel
	{
		/// <summary>
		/// 用户账号 
		/// </summary>
		[DataMember(Name = "member")]
		public string Member { get; set; }
		/// <summary>
		/// 用户类型 （普通 金 钻石 
		/// </summary>
		[DataMember(Name = "type")]
		public int Type { get; set; }

		/// <summary>
		/// 开始时间 
		/// </summary>
		[DataMember(Name = "starttime")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 截至时间 
		/// </summary>
		[DataMember(Name = "endtime")]
		public DateTime EndTime { get; set; }
	}

	/// <summary>
	/// 用户账单数据
	/// </summary>
	[DataContract]
	public class BillResModel
	{
		/// <summary>
		/// 序号
		/// </summary>
		[DataMember(Name = "id")]
		public decimal Id { get; set; }

		/// <summary>
		/// 用户账号 
		/// </summary>
		[DataMember(Name = "member")]
		public string Member { get; set; }

		/// <summary>
		/// 账单时间
		/// </summary>
		[DataMember(Name = "billtime")]
		public DateTime BillTime { get; set; }

		/// <summary>
		/// 金额
		/// </summary>
		[DataMember(Name = "money")]
		public decimal Money { get; set; }

		/// <summary>
		/// 商品编号
		/// </summary>
		[DataMember(Name = "goodsno")]
		public decimal GoodsNo { get; set; }

		/// <summary>
		/// 归属类别
		/// </summary>
		[DataMember(Name = "goodstype")]
		public string GoodsType { get; set; }

		/// <summary>
		/// 商品数量
		/// </summary>
		[DataMember(Name = "goodscount")]
		public int GoodsCount { get; set; }

		/// <summary>
		/// 商品名称
		/// </summary>
		[DataMember(Name = "goodsname")]
		public decimal GoodsName { get; set; }
	}
}