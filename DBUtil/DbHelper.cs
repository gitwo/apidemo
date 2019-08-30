using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtil
{
	public class DbHelper
	{
		//数据库连接字符串(web.config来配置)，多数据库可使用DbHelperSQLP来实现.

		public static string connectionString = DBConnections.Connection_Bill;
		public static string connectionStringMember = DBConnections.Connection_Member;

		#region 公用方法

		/// <summary>
		/// 判断表是否存在
		/// </summary>
		/// <param name="TableName"></param>
		/// <returns>true:存在  false:不存在</returns>
		public static bool TabExists(string TableName)
		{
			string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
			object obj = GetSingle(strsql);
			int cmdresult;
			if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
			{
				cmdresult = 0;
			}
			else
			{
				cmdresult = int.Parse(obj.ToString());
			}
			return cmdresult != 0;
		}

		/// <summary>
		/// 判断表的某个字段是否存在
		/// </summary>
		/// <param name="tableName">表名称</param>
		/// <param name="columnName">列名称</param>
		/// <returns>true:存在  false:不存在</returns>
		public static bool ColumnExists(string tableName, string columnName)
		{
			string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
			object res = GetSingle(sql);
			if (res == null)
			{
				return false;
			}
			return Convert.ToInt32(res) > 0;
		}

		public static bool Exists(string strSql)
		{
			object obj = GetSingle(strSql);
			int cmdresult;
			if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
			{
				cmdresult = 0;
			}
			else
			{
				cmdresult = int.Parse(obj.ToString()); //可能0
			}

			return cmdresult != 0;
		}

		public static bool Exists(string strSql, params SqlParameter[] cmdParms)
		{
			object obj = GetSingle(strSql, cmdParms);
			int cmdresult;
			if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
			{
				cmdresult = 0;
			}
			else
			{
				cmdresult = int.Parse(obj.ToString());
			}
			return cmdresult != 0;
		}

		public static int GetMaxID(string FieldName, string TableName)
		{
			string strsql = "select max(" + FieldName + ")+1 from " + TableName;
			object obj = GetSingle(strsql);
			if (obj == null)
			{
				return 1;
			}
			return int.Parse(obj.ToString());
		}

		#endregion


		#region  执行简单SQL语句

		/// <summary>
		/// ExecuteNonQuery封装
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string SQLString)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						int rows = cmd.ExecuteNonQuery();
						return rows;
					}
					catch (SqlException e)
					{
						connection.Close();
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// ExecuteNonQuery封装
		/// </summary>
		/// <param name="SQLString"></param>
		/// <param name="Times"></param>
		/// <returns></returns>
		public static int ExecuteSqlByTime(string SQLString, int Times)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						cmd.CommandTimeout = Times;
						int rows = cmd.ExecuteNonQuery();
						return rows;
					}
					catch (SqlException e)
					{
						connection.Close();
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// 数据库事务中执行多条SQL语句。
		/// </summary>
		/// <param name="SQLStringList">多条SQL语句</param>		
		public static int ExecuteSqlTran(List<string> SQLStringList)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = conn;
				SqlTransaction tx = conn.BeginTransaction();
				cmd.Transaction = tx;
				try
				{
					int count = 0;
					for (int n = 0; n < SQLStringList.Count; n++)
					{
						string strsql = SQLStringList[n];
						if (strsql.Trim().Length > 1)
						{
							cmd.CommandText = strsql;
							count += cmd.ExecuteNonQuery();
						}
					}
					tx.Commit();
					return count;
				}
				catch
				{
					tx.Rollback();
					return 0;
				}
			}
		}

		/// <summary>
		/// 执行特殊的的SQL语句
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <param name="content">参数内容,一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string SQLString, string content)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand cmd = new SqlCommand(SQLString, connection);
				SqlParameter myParameter = new SqlParameter("@content", SqlDbType.NText);
				myParameter.Value = content;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows = cmd.ExecuteNonQuery();
					return rows;
				}
				catch (SqlException e)
				{
					throw e;
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}
			}
		}

		/// <summary>
		/// 执行特殊的的SQL语句
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <param name="content">参数内容,一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
		/// <returns>首行首列</returns>
		public static object ExecuteSqlGet(string SQLString, string content)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand cmd = new SqlCommand(SQLString, connection);
				SqlParameter myParameter = new SqlParameter("@content", SqlDbType.NText);
				myParameter.Value = content;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					object obj = cmd.ExecuteScalar();
					if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
					{
						return null;
					}
					return obj;
				}
				catch (SqlException e)
				{
					throw e;
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}
			}
		}

		/// <summary>
		/// 向数据库里插入图像格式的字段
		/// </summary>
		/// <param name="strSQL">SQL语句</param>
		/// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand cmd = new SqlCommand(strSQL, connection);
				SqlParameter myParameter = new SqlParameter("@fs", SqlDbType.Image);
				myParameter.Value = fs;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows = cmd.ExecuteNonQuery();
					return rows;
				}
				catch (SqlException e)
				{
					throw e;
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}
			}
		}

		/// <summary>
		/// ExecuteScalar封装
		/// </summary>
		/// <param name="SQLString"></param>
		/// <returns>首行首列</returns>
		public static object GetSingle(string SQLString)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						object obj = cmd.ExecuteScalar();
						if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
						{
							return null;
						}
						return obj;
					}
					catch (SqlException e)
					{
						connection.Close();
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// ExecuteScalar封装
		/// </summary>
		/// <param name="SQLString"></param>
		/// <param name="Times"></param>
		/// <returns></returns>
		public static object GetSingle(string SQLString, int Times)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(SQLString, connection))
				{
					try
					{
						connection.Open();
						cmd.CommandTimeout = Times;
						object obj = cmd.ExecuteScalar();
						if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
						{
							return null;
						}
						return obj;
					}
					catch (SqlException e)
					{
						connection.Close();
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// ExecuteReader封装，返回SqlDataReader (SqlDataReader需要Close )
		/// </summary>
		/// <param name="strSQL">查询语句</param>
		/// <returns>SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string strSQL)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand(strSQL, connection);
			try
			{
				connection.Open();
				SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				return myReader;
			}
			catch (SqlException e)
			{
				throw e;
			}
		}

		/// <summary>
		/// SqlDataAdapter封装
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				DataSet ds = new DataSet();
				try
				{
					connection.Open();
					SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
					command.Fill(ds, "ds");
				}
				catch (SqlException ex)
				{
					throw new Exception(ex.Message);
				}
				return ds;
			}
		}

		/// <summary>
		/// SqlDataAdapter封装
		/// </summary>
		/// <param name="SQLString"></param>
		/// <param name="Times"></param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString, int Times)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				DataSet ds = new DataSet();
				try
				{
					connection.Open();
					SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
					command.SelectCommand.CommandTimeout = Times;
					command.Fill(ds, "ds");
				}
				catch (SqlException ex)
				{
					throw new Exception(ex.Message);
				}
				return ds;
			}
		}

		#endregion


		#region 带参数的SQL语句

		/// <summary>
		/// 带可选参数的ExecuteNonQuery封装
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					try
					{
						PrepareCommand(cmd, connection, null, SQLString, cmdParms);
						int rows = cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();
						return rows;
					}
					catch (SqlException e)
					{
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// 数据库事务中执行带参数带多条SQL语句
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql，value是SqlParameter[]）</param>
		public static bool ExecuteSqlTran(Hashtable SQLStringList)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						foreach (DictionaryEntry myDE in SQLStringList)
						{
							string cmdText = myDE.Key.ToString();
							SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
							PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
							int val = cmd.ExecuteNonQuery();
							cmd.Parameters.Clear();
						}
						trans.Commit();
						return true;
					}
					catch
					{
						trans.Rollback();
						return false;
					}
				}
			}
		}

		/// <summary>
		/// 数据库事务中执行带参数带多条SQL语句
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql，value是SqlParameter[]）</param>
		public static int ExecuteSqlTran(List<CommandInfo> cmdList)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						int count = 0;
						foreach (CommandInfo myDE in cmdList)
						{
							string cmdText = myDE.CommandText;
							SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
							PrepareCommand(cmd, conn, trans, cmdText, cmdParms);

							if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
							{
								if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
								{
									trans.Rollback();
									return 0;
								}

								object obj = cmd.ExecuteScalar();
								bool isHave = false;
								if (obj == null && obj == DBNull.Value)
								{
									isHave = false;
								}
								isHave = Convert.ToInt32(obj) > 0;

								if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
								{
									trans.Rollback();
									return 0;
								}
								if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
								{
									trans.Rollback();
									return 0;
								}
								continue;
							}
							int val = cmd.ExecuteNonQuery();
							count += val;
							if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
							{
								trans.Rollback();
								return 0;
							}
							cmd.Parameters.Clear();
						}
						trans.Commit();
						return count;
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// 执行多条SQL语句，多个数据库，数据库事务
		/// </summary>
		/// <param name="myDE1"></param>
		/// <param name="myDE2"></param>
		public static int TwoDBExecuteSqlTranNo1(CommandInfo myDE1, CommandInfo myDE2)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlTransaction trans1 = conn.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						string cmdText = myDE1.CommandText;
						SqlParameter[] cmdParms = (SqlParameter[])myDE1.Parameters;
						PrepareCommand(cmd, conn, trans1, cmdText, cmdParms);
						cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();

						return TwoDBExecuteSqlTranNo2(myDE2, trans1);
					}
					catch
					{
						trans1.Rollback();
						return 0;
					}
				}
			}
		}

		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="myDE2"></param>
		/// <param name="trans1"></param>
		/// <returns></returns>
		static int TwoDBExecuteSqlTranNo2(CommandInfo myDE2, SqlTransaction trans1)
		{
			using (SqlConnection conn = new SqlConnection(connectionStringMember))
			{
				conn.Open();
				using (SqlTransaction trans2 = conn.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						string cmdText = myDE2.CommandText;
						SqlParameter[] cmdParms = (SqlParameter[])myDE2.Parameters;
						PrepareCommand(cmd, conn, trans2, cmdText, cmdParms);
						cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();

						trans1.Commit();
						trans2.Commit();
						return 1;
					}
					catch
					{
						trans1.Rollback();
						trans2.Rollback();
						return 0;
					}
				}
			}
		}

		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql，value是SqlParameter[]）</param>
		public static void ExecuteSqlTranWithIndentity(List<CommandInfo> SQLStringList)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						int indentity = 0;
						foreach (CommandInfo myDE in SQLStringList)
						{
							string cmdText = myDE.CommandText;
							SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
							foreach (SqlParameter q in cmdParms)
							{
								if (q.Direction == ParameterDirection.InputOutput)
								{
									q.Value = indentity;
								}
							}
							PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
							int val = cmd.ExecuteNonQuery();
							foreach (SqlParameter q in cmdParms)
							{
								if (q.Direction == ParameterDirection.Output)
								{
									indentity = Convert.ToInt32(q.Value);
								}
							}
							cmd.Parameters.Clear();
						}
						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql，value是SqlParameter[]）</param>
		public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					SqlCommand cmd = new SqlCommand();
					try
					{
						int indentity = 0;
						foreach (DictionaryEntry myDE in SQLStringList)
						{
							string cmdText = myDE.Key.ToString();
							SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
							foreach (SqlParameter q in cmdParms)
							{
								if (q.Direction == ParameterDirection.InputOutput)
								{
									q.Value = indentity;
								}
							}
							PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
							int val = cmd.ExecuteNonQuery();
							foreach (SqlParameter q in cmdParms)
							{
								if (q.Direction == ParameterDirection.Output)
								{
									indentity = Convert.ToInt32(q.Value);
								}
							}
							cmd.Parameters.Clear();
						}
						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// ExecuteScalar封装
		/// </summary>
		/// <param name="SQLString"></param>
		/// <returns>查询结果（object）</returns>
		public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					try
					{
						PrepareCommand(cmd, connection, null, SQLString, cmdParms);
						object obj = cmd.ExecuteScalar();
						cmd.Parameters.Clear();
						if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
						{
							return null;
						}
						else
						{
							return obj;
						}
					}
					catch (SqlException e)
					{
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// SqlDataAdapter封装
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand cmd = new SqlCommand();
				PrepareCommand(cmd, connection, null, SQLString, cmdParms);
				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					DataSet ds = new DataSet();
					try
					{
						da.Fill(ds, "ds");
						cmd.Parameters.Clear();
					}
					catch (SqlException ex)
					{
						throw new Exception(ex.Message);
					}
					return ds;
				}
			}
		}

		private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
				cmd.Transaction = trans;
			cmd.CommandType = CommandType.Text;//cmdType;
			if (cmdParms != null)
			{
				foreach (SqlParameter parameter in cmdParms)
				{
					if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					cmd.Parameters.Add(parameter);
				}
			}
		}

		#endregion


		#region 存储过程操作

		/// <summary>
		/// 执行存储过程，返回SqlDataReader (调用该方法后，要对SqlDataReader进行Close)
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlDataReader</returns>
		public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
			command.CommandType = CommandType.StoredProcedure;
			SqlDataReader returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
			return returnReader;
		}

		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="tableName">DataSet结果中的表名</param>
		/// <returns>DataSet</returns>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				SqlDataAdapter sqlDA = new SqlDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
				sqlDA.Fill(dataSet, tableName);
				connection.Close();
				return dataSet;
			}
		}

		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcName"></param>
		/// <param name="parameters"></param>
		/// <param name="tableName"></param>
		/// <param name="Times"></param>
		/// <returns></returns>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				SqlDataAdapter sqlDA = new SqlDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
				sqlDA.SelectCommand.CommandTimeout = Times;
				sqlDA.Fill(dataSet, tableName);
				connection.Close();
				return dataSet;
			}
		}

		/// <summary>
		/// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlCommand</returns>
		private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand command = new SqlCommand(storedProcName, connection);
			command.CommandType = CommandType.StoredProcedure;
			foreach (SqlParameter parameter in parameters)
			{
				if (parameter != null)
				{
					// 检查未分配值的输出参数,将其分配以DBNull.Value.
					if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}

			return command;
		}

		/// <summary>
		/// 执行存储过程，返回影响的行数		
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="rowsAffected">影响的行数</param>
		/// <returns></returns>
		public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
				rowsAffected = command.ExecuteNonQuery();
				int result = (int)command.Parameters["ReturnValue"].Value;
				return result;
			}
		}

		/// <summary>
		/// 创建 SqlCommand 对象实例(用来返回一个整数值)	
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlCommand 对象实例</returns>
		private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
			command.Parameters.Add(new SqlParameter("ReturnValue",
				SqlDbType.Int, 4, ParameterDirection.ReturnValue,
				false, 0, 0, string.Empty, DataRowVersion.Default, null));
			return command;
		}
			   		 	  	  	   	
		#endregion
	}
}
