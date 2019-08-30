namespace Model.Core
{
	/// <summary>
	/// 实体消息类
	/// </summary>
	public class EntityMsg
	{
		/// <summary>
		/// 执行状态
		/// </summary>
		public int ExState
		{
			get;
			set;
		}

		/// <summary>
		/// 错误消息
		/// </summary>
		public string ErrorMsg
		{
			get;
			set;
		}
	}
}
