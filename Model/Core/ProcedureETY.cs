using System.Collections;

namespace Model.Core
{
	/// <summary>
	/// 存储过程实体密封部分类
	/// </summary>
	public sealed partial class ProcedureETY : EntityBase
	{
		public ProcedureETY()
		{
			this.ProcedureName = "";
			this.Values = new ArrayList();
			this.Result = new ArrayList();
		}

		/// <summary>
		/// 存储过程名称
		/// </summary>
		public string ProcedureName
		{
			get;
			set;
		}

		/// <summary>
		/// 存储过程的参数值数组对象（参数的先后与数据库中存储过程的实际实际参数的先后一致）
		/// </summary>
		public ArrayList Values
		{
			get;
			set;
		}

		/// <summary>
		/// 存储过程的返回参数的值对象（参数的先后与数据库中存储过程的实际实际参数的先后一致）
		/// </summary>
		public ArrayList Result
		{
			get;
			set;
		}
	}
}
