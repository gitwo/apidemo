using Model.Core;
using System;

namespace Model
{

	[Serializable]
    public class BillModel:EntityBase
    {
		#region BillModel 构造函数
		/// <summary>
		/// BillModel 实体类对象
		/// </summary>
		public BillModel()
		{
			this._Id = 0;
			this._member = string.Empty;
			this._money = 0;
			this._billTime = DateTime.MinValue;		
			this._type = 0;
			this._goodsNo = string.Empty;
			this._goodsCount = 0;
			this._goodsName = string.Empty;
		}
		#endregion

		#region Properties 与数据库对应的各个属性

		private decimal _Id;
		/// <summary>
		/// 
		/// </summary>
		public decimal Id
		{
			get { return _Id; }
			set { _Id = value; AddUpdatingField("Id"); }
		}

		private int _type;
		/// <summary>
		/// 会员类型
		/// </summary>
		public int type
		{
			get { return _type; }
			set { _type = value; AddUpdatingField("type"); }
		}

		private DateTime _billTime;
		/// <summary>
		/// 
		/// </summary>
		public DateTime billTime
		{
			get { return _billTime; }
			set { _billTime = value; AddUpdatingField("billTime"); }
		}


		private string _member;
		/// <summary>
		/// 
		/// </summary>
		public string member
		{
			get { return _member; }
			set { _member = value; AddUpdatingField("member"); }
		}

		private decimal _money;
		/// <summary>
		/// 
		/// </summary>
		public decimal money
		{
			get { return _money; }
			set { _money = value; AddUpdatingField("money"); }
		}
		
		private string _goodsNo ;
		/// <summary>
		/// 商品编号
		/// </summary>
		public string goodsNo
		{
			get { return _goodsNo; }
			set { _goodsNo = value; AddUpdatingField("goodsNo"); }
		}

		private int _goodsCount;
		/// <summary>
		/// 商品数量
		/// </summary>
		public int goodsCount
		{
			get { return _goodsCount; }
			set { _goodsCount = value; AddUpdatingField("goodsCount"); }
		}

		private string _goodsName;
		/// <summary>
		/// 商品名称
		/// </summary>
		public string goodsName
		{
			get { return _goodsName; }
			set { _goodsName = value; AddUpdatingField("goodsName"); }
		}


		#endregion
	}

}
