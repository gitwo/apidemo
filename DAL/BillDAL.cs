using DAL.Core;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
	public sealed partial class BillDAL:DALBase<BillModel>
    {
		#region BillDAL 构造函数
		public BillDAL()
			: this(null, null) { }
		public BillDAL(IDbConnection connection, IDbTransaction transaction) : base(connection, transaction) { }
		#endregion

		#region TableName,PrimaryKeyName 表名与主键
		/// <summary>
		/// 表名(只读)
		/// </summary>
		public override string TableName
		{
			get { return DB_User + ".[Bill]"; }
		}

		/// <summary>
		/// 表主键名(只读)
		/// </summary>
		public override string PrimaryKeyName
		{
			get { return "id"; }
		}
		#endregion

		#region Create 创建实体对象 ( 等同于 new Bill() )
		/// <summary>
		/// 创建实体对象( 等同于 new t_loginInfo() )
		/// </summary>
		/// <returns>实体对象</returns>
		public override BillModel Create()
		{
			return new BillModel();
		}
		#endregion

		#region Load 从IDataReader加载实体对象
		/// <summary>
		/// 从IDataReader加载实体对象
		/// </summary>
		///	<param name="IDataReader">IDataReader</param>
		/// <returns>t_Area 实体对象</returns>
		protected override BillModel Load_all(IDataReader reader)
		{
			BillModel entity = new BillModel();
			entity.Id = SqlHelper.GetInt32(reader, "id");
			entity.billTime = SqlHelper.GetDateTime(reader, "billTime");		
			entity.member = SqlHelper.GetString(reader, "member");
			entity.money = SqlHelper.GetDecimal(reader, "money");	
			entity.Reset();
			entity.IsNew = false;
			return entity;
		}

		/// <summary>
		/// 从IDataReader加载实体对象 按查询到的字段加载实体
		/// </summary>
		///	<param name="IDataReader">IDataReader</param>
		/// <returns>t_loginInfoMOD 实体对象</returns>
		protected override BillModel Load(IDataReader reader)
		{
			BillModel entity = new BillModel();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				switch (reader.GetName(i))
				{
					case "Id":
						entity.Id = SqlHelper.GetDecimal(reader, i, entity);
						break;
					case "billTime":
						entity.billTime = SqlHelper.GetDateTime(reader, i, entity);
						break;					
					case "member":
						entity.member = SqlHelper.GetString(reader, i, entity);
						break;
					case "money":
						entity.money = SqlHelper.GetDecimal(reader, i, entity);
						break;	
				}
			}
			entity.Reset();
			entity.IsNew = false;
			return entity;
		}
		#endregion

		#region GetSqlParameter 根据字段名获取相应的SqlParameter对象
		/// <summary>
		/// 根据字段名获取相应的SqlParameter对象
		/// </summary>
		/// <param name="fieldName">字段名称</param>
		/// <param name="entity">实体对象</param>
		/// <returns></returns>
		protected override SqlParameter GetSqlParameter(string fieldName, BillModel entity)
		{
			SqlParameter _param = null;
			switch (fieldName)
			{
				case "Id":
					_param = new SqlParameter { ParameterName = "@Id", SqlDbType = SqlDbType.Decimal, Value = entity.Id };
					break;
				case "billTime":
					_param = new SqlParameter { ParameterName = "@billTime", SqlDbType = SqlDbType.DateTime, Value = entity.billTime };
					break;				
				case "member":
					_param = new SqlParameter { ParameterName = "@member", SqlDbType = SqlDbType.VarChar, Value = entity.member };
					break;
				case "money":
					_param = new SqlParameter { ParameterName = "@money", SqlDbType = SqlDbType.Decimal, Value = entity.money };
					break;	
			}
			return _param;
		}
		#endregion

		#region Single 根据主键查询实体对象
		/// <summary>
		/// 根据主键查询实体对象
		/// </summary>
		/// <param name="id">主键ID</param>
		/// <returns>实体对象</returns>
		public BillModel Single(decimal id)
		{
			return this.SingleById(id);
		}
		#endregion

		#region Delete 根据主键,删除单条记录

		/// <summary>
		/// 根据主键ID,删除单条记录
		/// </summary>
		/// <param name="id">主键ID</param>
		/// <returns>成功与否</returns>
		public bool Delete(decimal id)
		{
			return this.Delete1(id);
		}
		/// <summary>
		/// 根据主键ID List,删除多条记录
		/// </summary>
		/// <param name="idList">主键ID List</param>
		/// <returns>受影响行数</returns>
		public int Delete(List<decimal> idList)
		{
			return this.Delete<decimal>(idList);
		}
		#endregion

		#region 自定义函数

		#region insert区域
		#endregion

		#region update区域
		#endregion

		#region delete区域
		#endregion

		#region select区域

		/// <summary>
		/// 查账单
		/// </summary>
		/// <returns></returns>
		public DataTable GetBill()
		{
			string sql = "select distinct Id,member,money,starttime,endtime from bill with(nolock);";
			return ExecuteDataTable(CommandType.Text, sql);
		}			

		#endregion

		#endregion
	}
}
