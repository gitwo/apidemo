using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Core
{
	/// <summary>
	/// 分页对象
	/// </summary>
	public sealed class PagerInfo : System.ComponentModel.Component
	{
		/// <summary>
		/// 分页对象构造函数
		/// </summary>
		/// <param name="pageIndexParam">页数参数(可为空,默认: page)</param>
		public PagerInfo(string pageIndexParam)
		{
			if (string.IsNullOrEmpty(pageIndexParam))
			{
				pageIndexParam = "page";
			}
			this._PageIndexParam = pageIndexParam;
		}


		#region Methods
		private void Compute()
		{
			if (this._ItemCount == 0 || this._PageSize == 0)
			{
				this._ItemCount = 0;
				this._PageCount = 0;
				this._PageItemCount = 0;
			}
			else
			{
				this._PageCount = (this._ItemCount / this._PageSize) + (this._ItemCount % this._PageSize == 0 ? 0 : 1);
				this._PageIndex = this.CheckIndex(this.GetQueryStringValue(this.PageIndexParam, this._FirstPageIndex));
				int pageItemCount = this._PageIndex * this._PageSize - this._ItemCount;
				this._PageItemCount = pageItemCount > 0 ? this._PageSize - pageItemCount : this._PageSize;				
				this._PageItemBegin = (this._PageIndex - 1) * this.PageSize + 1;
				this._PageItemEnd = this._PageItemBegin + this._PageItemCount - 1;
			}
		}

		private int CheckIndex(int pageIndex)
		{
			if (pageIndex < this._FirstPageIndex)
			{
				pageIndex = this._FirstPageIndex;
			}
			if (this._PageCount > 0 && pageIndex > this._PageCount)
			{
				pageIndex = this._PageCount;
			}
			return pageIndex;
		}


		#region GetQueryStringValue
		/// <summary>
		/// 获取地址栏传递参数(仅限 string, int, long 类型)
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="keyname">键名称</param>
		/// <param name="defaultValue">默认值</param>
		public T GetQueryStringValue<T>(string keyname, T defaultValue)
		{
			object getValue = defaultValue;
			if (HttpContext.Current.Request.QueryString[keyname] != null)
			{
				getValue = HttpContext.Current.Request.QueryString[keyname];
				if (defaultValue is string)
				{
					if (getValue.ToString() == string.Empty)
					{
						getValue = defaultValue;
					}
				}
				if (defaultValue is int)
				{
					try
					{
						getValue = int.Parse(getValue.ToString());
					}
					catch (Exception)
					{
						getValue = defaultValue;
					}
				}
				if (defaultValue is byte)
				{
					try
					{
						getValue = byte.Parse(getValue.ToString());
					}
					catch (Exception)
					{
						getValue = defaultValue;
					}
				}
				if (defaultValue is long)
				{
					try
					{
						getValue = long.Parse(getValue.ToString());
					}
					catch (Exception)
					{
						getValue = defaultValue;
					}
				}
			}
			return (T)getValue;
		}
		#endregion

		#endregion


		#region Properties
		private int _ItemCount;
		/// <summary>
		/// 总记录数
		/// </summary>
		public int ItemCount
		{
			get
			{
				return this._ItemCount;
			}
			set
			{
				if (this._ItemCount != value)
				{
					this._ItemCount = value;
					this.Compute();
				}
			}
		}

		private int _FirstPageIndex = 1;

		private int _PageCount;
		/// <summary>
		/// 总页数(只读)
		/// </summary>
		public int PageCount
		{
			get
			{
				return this._PageCount;
			}
		}

		private int _PageIndex;
		/// <summary>
		/// 当前页数(只读)
		/// </summary>
		public int PageIndex
		{
			get
			{
				return this._PageIndex;
			}
		}

		private string _PageIndexParam;
		/// <summary>
		/// 页数参数(如: page)
		/// </summary>
		public string PageIndexParam
		{
			get
			{
				return this._PageIndexParam;
			}
			set
			{
				if (this._PageIndexParam != value)
					this._PageIndexParam = value;
			}
		}

		private int _PageItemCount;
		/// <summary>
		/// 当前页显示记录数(只读)
		/// </summary>
		public int PageItemCount
		{
			get
			{
				return this._PageItemCount;
			}
		}

		private int _PageItemBegin;
		/// <summary>
		/// 当前页显示记录数开始值(只读)
		/// </summary>
		public int PageItemBegin
		{
			get
			{
				return this._PageItemBegin;
			}
		}

		private int _PageItemEnd;
		/// <summary>
		/// 当前页显示记录数结束值(只读)
		/// </summary>
		public int PageItemEnd
		{
			get
			{
				return this._PageItemEnd;
			}
		}

		private int _PageSize = 10;
		/// <summary>
		/// 每页显示记录数(默认为: 10)
		/// </summary>
		public int PageSize
		{
			get
			{
				return this._PageSize;
			}
			set
			{
				if (this._PageSize != value)
					this._PageSize = value;
			}
		}
		#endregion

	}
}
