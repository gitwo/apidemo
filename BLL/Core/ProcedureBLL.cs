using DAL.Core;
using System.Collections.Generic;
using System.Data;

namespace BLL.Core
{
	/// <summary>
	/// 存储过程
	/// </summary>
	public class ProcedureBLL : BLLBase1
	{
		/// <summary>
		/// 数据库操作层
		/// </summary>
		private ProcedureDAL _dal = null;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <returns></returns>
		public ProcedureBLL(DataConnection con)
			: base(con)
		{
			_dal = new ProcedureDAL(_conn.Connection, _conn.Transaction);
		}

		/// <summary>
		/// 参数化查询 返回DataSet
		/// </summary>
		/// <returns></returns>
		public static DataSet GetDataSetParas(string sql, List<string> parameters, List<object> parametersValues)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.GetDataSetUsingParams(sql, parameters, parametersValues);
			}
		}

		/// <summary>
		/// 执行存储过程（适应：参数无返回值的存储过程）
		/// </summary>
		/// <param name="sql">sql参考：exec pro_Test 1,1(pro_Test 1,1)</param>
		/// <returns>存储过程返回的对象</returns>
		public DataTable Exec(string sql)
		{
			DataTable result = _dal.Exec(sql);
			if (result == null)
				return new DataTable();
			return result;
		}

		/// <summary>
		/// 执行存储过程（参数无返回值）
		/// </summary>
		/// <param name="sql">sql参考：exec pro_Test 1,1(pro_Test 1,1)</param>
		/// <param name="c">数据库连接对象</param>
		/// <returns></returns>
		public static DataTable ExecObj(string sql, DataConnection c)
		{
			ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
			return dal.Exec(sql);
		}

		//更新附件状态
		public static bool ExecUpdataObj(string sql)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.ExecUpdata(sql);
			}
		}

		//更新附件状态
		public static bool ExecUpdataObj(string sql, DataConnection c)
		{
			//  using (c)
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.ExecUpdata(sql);
			}
		}

		//更新
		public static bool ExecNoObj(string sql)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.ExecUpdata(sql);
			}
		}

		/// <summary>
		/// 执行存储过程（参数无返回值）
		/// </summary>
		/// <param name="sql">sql参考：exec pro_Test 1,1(pro_Test 1,1)</param>
		/// <returns>存储过程返回的对象</returns>
		public static DataSet ExecObjSet(string sql)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				DataSet result = dal.ExecDataSet(sql);
				if (result == null)
					return new DataSet();
				return result;
			}
		}

		/// <summary>
		/// 执行单行单字段查询
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		public static object ExecuteScalar(string sql, List<string> parameters, List<object> parametersValues)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				object result = dal.ExecuteScalar(sql, parameters, parametersValues);
				return result;
			}
		}

		/// <summary>
		/// 执行单行单字段查询 参数化
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		public static object ExecuteScalar(string sql)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				object result = dal.ExecuteScalar(sql);
				return result;
			}
		}

		/// <summary>
		/// SQL参数化查询
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parameters">参数名称</param>
		/// <param name="parametersValues">参数值</param>
		/// <returns></returns>
		public static DataTable GetDataTableByParas(string sql, List<string> parameters, List<object> parametersValues)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.GetDataUsingParams(sql, parameters, parametersValues);
			}
		}

		/// <summary>
		/// 非查询 参数化方法 
		/// </summary>
		/// <param name="sql">查询条件</param>
		/// <param name="parameters">参数</param>
		/// <param name="parametersValues">参数值</param>
		/// <returns></returns>
		public static bool GetBoolByParas(string sql, List<string> parameters, List<object> parameterValues)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.GetBoolUsingParams(sql, parameters, parameterValues);
			}
		}

		/// <summary>
		/// 非查询 参数化方法 
		/// </summary>
		/// <param name="sql">查询条件</param>
		/// <param name="parameters">参数</param>
		/// <param name="parametersValues">参数值</param>
		/// <param name="c">连接对象</param>
		/// <returns></returns>
		public static bool GetBoolByParas(string sql, List<string> parameters, List<object> parameterValues, DataConnection c)
		{
			ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
			return dal.GetBoolUsingParams(sql, parameters, parameterValues);
		}

		/// <summary>
		/// 执行单行单字段查询 参数化
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		public static object ExecuteScalar(string sql, DataConnection con)
		{
			ProcedureDAL dal = new ProcedureDAL(con.Connection, con.Transaction);
			object result = dal.ExecuteScalar(sql);
			return result;
		}

		/// <summary>
		/// 参数化查询
		/// </summary>
		/// <returns></returns>
		public static int ExecSqlParams(string sql, List<string> parameters, List<object> parametersValues)
		{
			using (DataConnection c = new DataConnection())
			{
				ProcedureDAL dal = new ProcedureDAL(c.Connection, c.Transaction);
				return dal.ExecSqlParams(sql, parameters, parametersValues);
			}
		}

		/// <summary>
		/// 参数化查询 返回DataSet
		/// </summary>
		/// <returns></returns>
		public static DataSet GetDataSetParas(string sql, List<string> parameters, List<object> parametersValues, DataConnection con)
		{
			ProcedureDAL dal = new ProcedureDAL(con.Connection, con.Transaction);
			return dal.GetDataSetUsingParams(sql, parameters, parametersValues);
		}

		/// <summary>
		/// 参数化查询 
		/// </summary>
		/// <returns>DataTable</returns>
		public static DataTable GetDataTableParas(string sql, List<string> parameters, List<object> parametersValues, DataConnection con)
		{
			ProcedureDAL dal = new ProcedureDAL(con.Connection, con.Transaction);
			return dal.GetDataUsingParams(sql, parameters, parametersValues);
		}
	}

	public class BLLBase1
	{
		/// <summary>
		/// 数据库连接
		/// </summary>
		protected DataConnection _conn = null;

		public BLLBase1(DataConnection con)
		{
			_conn = con;
		}

	}
}
