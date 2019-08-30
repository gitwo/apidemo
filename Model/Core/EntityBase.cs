using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core
{
	/// <summary>
	/// 实体类基类
	/// </summary>
	[Serializable]
	public class EntityBase
	{
		#region 基类功能
		/// <summary>
		/// 实体类基类
		/// </summary>
		protected EntityBase()
		{
		}

		/// <summary>
		/// 重置(到未改变)
		/// </summary>
		public void Reset()
		{
			this._HasChanged = false;
			this._UpdatingFields = null;
			this._UpdatingWhereFields = null;
		}

		protected bool _HasChanged = false;
		/// <summary>
		/// 是否改变
		/// </summary>
		public bool HasChanged
		{
			get
			{
				return this._HasChanged;
			}
		}

		private bool _IsNew = true;
		/// <summary>
		/// 是否为新
		/// </summary>
		public bool IsNew
		{
			get
			{
				return this._IsNew;
			}
			set
			{
				if (this._IsNew != value)
				{
					this._IsNew = value;
				}
			}
		}

		private HashSet<string> _UpdatingFields;
		/// <summary>
		/// 获取待更新的字段信息
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetUpdatingFields()
		{
			return _UpdatingFields;
		}

		/// <summary>
		/// 新增待更新的字段信息 
		/// </summary>
		/// <param name="field"></param>
		public void AddUpdatingField(string field)
		{
			if (_UpdatingFields == null)
				_UpdatingFields = new HashSet<string>();
			if (!_UpdatingFields.Contains(field))
				_UpdatingFields.Add(field);
		}

		private HashSet<string> _UpdatingWhereFields;
		/// <summary>
		/// 获取待更新的where条件的字段信息
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetUpdatingWhereFields()
		{
			return _UpdatingWhereFields;
		}

		/// <summary>
		/// 新增待更新的where条件的字段信息(注：field要求与字段名称一郅，包括大小写)
		/// </summary>
		/// <param name="field"></param>
		public void AddUpdatingWhereField(string field)
		{
			if (_UpdatingWhereFields == null)
				_UpdatingWhereFields = new HashSet<string>();
			_UpdatingWhereFields.Add(field);
			if (_UpdatingFields != null)
				_UpdatingFields.Remove(field);
		}


		private HashSet<string> _NullFields;
		/// <summary>
		/// 获取null的字段信息
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetNullFields()
		{
			return _NullFields;
		}

		/// <summary>
		/// 新增null的字段信息 
		/// </summary>
		/// <param name="field"></param>
		public void AddNullField(string field)
		{
			if (_NullFields == null)
				_NullFields = new HashSet<string>();
			if (!_NullFields.Contains(field))
				_NullFields.Add(field);
		}
		#endregion
	}
}
