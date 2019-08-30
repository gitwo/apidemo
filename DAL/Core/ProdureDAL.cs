using Model.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core
{
	/// <summary>
	/// 存储过程数据库操作类
	/// </summary>
	public sealed partial class ProcedureDAL : DALBase<ProcedureETY>
	{
		#region Generate
		private string _tableName = string.Empty;

		public ProcedureDAL(IDbConnection connection, IDbTransaction transaction)
			: base(connection, transaction)
		{
		}

		public ProcedureDAL(IDbConnection connection, IDbTransaction transaction, string tableName)
			: base(connection, transaction)
		{
			this._tableName = tableName;
		}

		public override string TableName
		{
			get
			{
				if (!string.IsNullOrEmpty(this._tableName))
				{
					return this._tableName;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		public override string PrimaryKeyName
		{
			get { throw new NotImplementedException(); }
		}

		public override bool Insert(ProcedureETY entity)
		{
			throw new NotImplementedException();
		}

		public override bool Update(ProcedureETY entity)
		{
			throw new NotImplementedException();
		}

		public override ProcedureETY Create()
		{
			throw new NotImplementedException();
		}

		protected override ProcedureETY Load_all(IDataReader reader)
		{
			throw new NotImplementedException();
		}
		protected override ProcedureETY Load(IDataReader reader)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Exec 执行存储过程

		/// <summary>
		/// 执行单行单字段查询
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		/// <summary>
		/// 执行存储过程（适应：参数无返回值的存储过程）
		/// </summary>
		/// <param name="sql">sql参考：exec pro_Test 1,1(pro_Test 1,1)</param>
		/// <returns>存储过程返回的对象</returns>
		public DataTable Exec(string sql)
		{
			return this.ExecuteDataTable(CommandType.Text, sql);
		}

		/// <summary>
		/// 执行单行单字段查询
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		public object ExecuteScalar(string sql)
		{
			return this.ExecuteScalar(CommandType.Text, sql);
		}

		/// <summary>
		/// 执行参数无返回值的存储过程
		/// </summary>
		/// <param name="sql">sql参考：exec pro_Test 1,1(pro_Test 1,1)</param>
		/// <returns>存储过程返回的对象</returns>
		public DataSet ExecDataSet(string sql)
		{
			return this.ExecuteDataSet(CommandType.Text, sql);
		}

		/// <summary>
		/// 执行单行单字段查询 参数化
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		public object ExecuteScalar(string sql, List<string> parameters, List<object> parametersValues)
		{
			if (parameters.Count <= 0 || parametersValues.Count <= 0 || parametersValues.Count != parameters.Count)
			{
				return null;
			}
			IDataParameter[] param = new IDataParameter[parameters.Count];
			for (int i = 0; i < parameters.Count; i++)
			{
				param[i] = new SqlParameter(parameters[i], parametersValues[i]);
			}

			return this.ExecuteScalar(CommandType.Text, sql, param);
		}

		/// <summary>
		/// 执行存储过程（适应：参数无返回值的存储过程）
		/// </summary>
		/// <param name="sql">sql参考：exec pro_Test 1,1(pro_Test 1,1)</param>
		/// <returns>存储过程返回的对象</returns>
		public bool ExecUpdata(string sql)
		{
			return this.ExecuteSql(CommandType.Text, sql) > 0;
		}
		#endregion

		/// <summary>
		/// SQL 参数化查询 
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <param name="parameters">参数</param>
		/// <param name="parametersValues">参数值</param>
		/// <returns></returns>
		public DataTable GetDataUsingParams(string sql, List<string> parameters, List<object> parametersValues)
		{
			if (parameters.Count <= 0 || parametersValues.Count <= 0 || parametersValues.Count != parameters.Count)
			{
				return null;
			}
			IDataParameter[] param = new IDataParameter[parameters.Count];
			for (int i = 0; i < parameters.Count; i++)
			{
				param[i] = new SqlParameter(parameters[i], parametersValues[i]);
			}
			return ExecuteDataTable(CommandType.Text, sql, param);
		}

		/// <summary>
		/// 非查询 参数化方法 
		/// </summary>
		/// <param name="sql">查询条件</param>
		/// <param name="parameters">参数</param>
		/// <param name="parametersValues">参数值</param>
		/// <returns></returns>
		public bool GetBoolUsingParams(string sql, IList<string> parameters, List<object> parametersValues)
		{
			if (parameters.Count <= 0 || parametersValues.Count <= 0 || parametersValues.Count != parameters.Count)
			{
				return false;
			}
			IDataParameter[] param = new IDataParameter[parameters.Count];
			for (int i = 0; i < parameters.Count; i++)
			{
				param[i] = new SqlParameter(parameters[i], parametersValues[i]);
			}
			return ExecuteSql(CommandType.Text, sql, param) > 0 ? true : false;

		}
		/// <summary>
		/// SQL 参数化查询  返回DataSet
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <param name="parameters">参数</param>
		/// <param name="parametersValues">参数值</param>
		/// <returns></returns>
		public DataSet GetDataSetUsingParams(string sql, List<string> parameters, List<object> parametersValues)
		{
			if (parameters.Count <= 0 || parametersValues.Count <= 0 || parametersValues.Count != parameters.Count)
			{
				return null;
			}
			IDataParameter[] param = new IDataParameter[parameters.Count];
			for (int i = 0; i < parameters.Count; i++)
			{
				param[i] = new SqlParameter(parameters[i], parametersValues[i]);
			}
			return ExecuteDataSet(CommandType.Text, sql, param);
		}

		/// <summary>
		/// SQL 参数化查询 
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <param name="parameters">参数</param>
		/// <param name="parametersValues">参数值</param>
		/// <returns></returns>
		public int ExecSqlParams(string sql, List<string> parameters, List<object> parametersValues)
		{
			if (parameters == null && parametersValues == null)
			{
				return ExecuteSql(CommandType.Text, sql, null);
			}
			else
			{
				IDataParameter[] param = new IDataParameter[parameters.Count];
				for (int i = 0; i < parameters.Count; i++)
				{
					param[i] = new SqlParameter(parameters[i], parametersValues[i]);
				}
				return ExecuteSql(CommandType.Text, sql, param);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="count">应返回的数据行数</param>
		/// <returns></returns>
		public bool ExecSQL(string sql, int count)
		{
			return this.ExecuteSql(CommandType.Text, sql) == count;
		}
	}
}
