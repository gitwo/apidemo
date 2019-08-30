using Model.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DAL.Core
{
	public abstract class SqlHelper
	{
		#region SqlHelper

		private static Hashtable _ParamsCache = Hashtable.Synchronized(new Hashtable());
		private static int _tmeout = 400;

		protected SqlHelper()
		{
		}

		/// <summary>
		/// 查询时间长时可自定义数据库连接时间
		/// </summary>
		public static int Tmeout
		{
			get { return _tmeout; }
			set { _tmeout = value; }
		}
				
		public static void CacheParameters(string cacheKey, params IDataParameter[] commandParameters)
		{
			_ParamsCache[cacheKey] = commandParameters;
		}

		#endregion

		#region 定义

		/// <summary>
		/// 自定义SqlCommand (带等待时)
		/// </summary>
		/// <returns></returns>
		public static SqlCommand GetSqlCommand()
		{
			SqlCommand cmd = new SqlCommand();
			if (_tmeout >= 0)
			{
				cmd.CommandTimeout = _tmeout;
			}
			return cmd;
		}

		#endregion

		#region ExecuteNonQuery
		public static int ExecuteNonQuery(IDbConnection connection, IDbTransaction trans, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, connection, trans, cmdType, cmdText, commandParameters);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		public static int ExecuteNonQuery(IDbConnection connection, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		public static int ExecuteNonQuery(IDbTransaction trans, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
				int val = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return val;
			}
		}
		#endregion


		#region ExecuteReader
		public static SqlDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, connection, transaction, cmdType, cmdText, commandParameters);
			SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			cmd.Parameters.Clear();
			return rdr;
		}

		private static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlDataReader test;
			SqlCommand cmd = GetSqlCommand();
			SqlConnection conn = new SqlConnection(connectionString);
			try
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				test = rdr;
			}
			catch
			{
				conn.Close();
				throw;
			}
			return test;
		}
		#endregion


		#region ExecuteScalar

		private static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
				object val = cmd.ExecuteScalar();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static object ExecuteScalar(IDbConnection connection, IDbTransaction transaction, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, connection, transaction, cmdType, cmdText, commandParameters);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		public static object ExecuteScalar(IDbTransaction trans, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		#endregion


		#region ExecuteDataTable

		public static DataTable ExecuteDataTable(IDbConnection connection, IDbTransaction transaction, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, connection, transaction, cmdType, cmdText, commandParameters);
			SqlDataAdapter ap = new SqlDataAdapter();
			ap.SelectCommand = cmd;
			DataSet st = new DataSet();
			ap.Fill(st, "Result");
			cmd.Parameters.Clear();
			return st.Tables["Result"];
		}

		public static DataSet ExecuteDataSet(IDbConnection connection, IDbTransaction transaction, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			PrepareCommand(cmd, connection, transaction, cmdType, cmdText, commandParameters);
			SqlDataAdapter ap = new SqlDataAdapter();
			ap.SelectCommand = cmd;
			DataSet st = new DataSet();
			ap.Fill(st);
			cmd.Parameters.Clear();
			return st;
		}

		private static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params IDataParameter[] commandParameters)
		{
			SqlCommand cmd = GetSqlCommand();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
				SqlDataAdapter ap = new SqlDataAdapter();
				ap.SelectCommand = cmd;
				DataSet st = new DataSet();
				ap.Fill(st, "Result");
				cmd.Parameters.Clear();
				return st.Tables["Result"];
			}
		}

		#endregion


		#region ExecuteReaderPage

		public static SqlDataReader ExecuteReaderPage(string connectionString, string sqlAllFields, string sqlTablesAndWhere, string indexField, string groupClause, string orderFields, int pageIndex, int pageSize, out int recordCount, out int pageCount, params IDataParameter[] commandParameters)
		{
			SqlDataReader test;
			SqlCommand cmd = GetSqlCommand();
			SqlConnection conn = new SqlConnection(connectionString);
			try
			{
				conn.Open();
				recordCount = 0;
				pageCount = 0;
				if (pageSize <= 0)
				{
					pageSize = 10;
				}
				string SqlCount = "SELECT COUNT(*) FROM " + sqlTablesAndWhere;
				cmd.Connection = conn;
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = SqlCount;
				if (commandParameters != null)
				{
					foreach (IDataParameter parm in commandParameters)
					{
						cmd.Parameters.Add(parm);
					}
				}
				recordCount = (int)cmd.ExecuteScalar();
				if ((recordCount % pageSize) == 0)
				{
					pageCount = recordCount / pageSize;
				}
				else
				{
					pageCount = (recordCount / pageSize) + 1;
				}
				if (pageIndex > pageCount)
				{
					pageIndex = pageCount;
				}
				if (pageIndex < 1)
				{
					pageIndex = 1;
				}
				string sql = null;
				if (pageIndex == 1)
				{
					sql = string.Concat(new object[] { "SELECT TOP ", pageSize, " ", sqlAllFields, " FROM ", sqlTablesAndWhere, " ", groupClause, " ", orderFields });
				}
				else
				{
					sql = string.Concat(new object[] { "SELECT TOP ", pageSize, " ", sqlAllFields, " FROM " });
					if (sqlTablesAndWhere.ToLower().IndexOf(" WHERE ") > 0)
					{
						string _where = Regex.Replace(sqlTablesAndWhere, @"\ WHERE\ ", " WHERE (", RegexOptions.Compiled | RegexOptions.IgnoreCase);
						sql = sql + _where + ") and (";
					}
					else
					{
						sql = sql + sqlTablesAndWhere + " WHERE (";
					}
					object obj = sql;
					string sqlStr = string.Concat(new object[] { obj, indexField, " not in (SELECT TOP ", (pageIndex - 1) * pageSize, " ", indexField, " FROM ", sqlTablesAndWhere, " ", orderFields });
					sql = sqlStr + ")) " + groupClause + " " + orderFields;
				}
				cmd.CommandText = sql;
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				test = rdr;
			}
			catch
			{
				conn.Close();
				throw;
			}
			return test;
		}

		#endregion


		#region GetCachedParameters
		public static IDataParameter[] GetCachedParameters(string cacheKey)
		{
			IDataParameter[] cachedParms = (IDataParameter[])_ParamsCache[cacheKey];
			if (cachedParms == null)
			{
				return null;
			}
			IDataParameter[] clonedParms = new IDataParameter[cachedParms.Length];
			int i = 0;
			int j = cachedParms.Length;
			while (i < j)
			{
				clonedParms[i] = (IDataParameter)((ICloneable)cachedParms[i]).Clone();
				i++;
			}
			return clonedParms;
		}

		#endregion


		#region PrepareCommand

		private static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, IDataParameter[] cmdParms)
		{
			if (conn.State != ConnectionState.Open)
			{
				conn.Open();
			}
			if (trans != null)
			{
				cmd.Transaction = trans;
			}
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			cmd.CommandType = cmdType;
			if (cmdParms != null)
			{
				foreach (IDataParameter parm in cmdParms)
				{
					cmd.Parameters.Add(parm);
				}
			}
		}

		#endregion


		#region GetSchemaClip, GetWhereClip, GetHavingClip, GetOrderClip


		#region GetSchemaClip
		/// <summary>
		/// 返回查询字段(如: * 或者 ID, Name)
		/// </summary>
		/// <param name="schema">要查询字段</param>
		/// <param name="orderKeyNameList">排序字段列表</param>
		/// <returns>返回查询字段(如: * 或者 ID, Name)</returns>
		public static string GetSchemaClip(string schema, List<string> orderKeyNameList)
		{
			if (string.IsNullOrEmpty(schema))
			{
				schema = "*";
			}
			if (schema.Trim() == "*")
			{
				return schema;
			}
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			List<string> schemaKeyList = new List<string>(0);
			schemaKeyList.AddRange(schema.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries));
			foreach (string keyname in orderKeyNameList)
			{
				if (keyname != null && keyname.Trim() != string.Empty && !schemaKeyList.Contains(keyname.Trim()))
				{
					schema += String.Format(", {0}", keyname);
				}
			}
			return schema.Trim().TrimStart(',');
		}

		#endregion

		#region GetWhereClip

		/// <summary>
		/// 返回Where片段(不包含WHERE关键字)
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="valueList">值数组</param>
		/// <param name="keyName">字段名</param>
		/// <param name="hasQuote">是否需要加单引号(如: varchar, datetime等类型)</param>
		/// <returns>Where片段(不包含WHERE关键字)</returns>
		public static string GetWhereClip<T>(List<T> valueList, string keyName, bool hasQuote)
		{
			return GetWhereClip<T>(valueList, keyName, hasQuote, true);
		}

		/// <summary>
		/// 返回Where片段(不包含WHERE关键字)
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="valueList">值数组</param>
		/// <param name="keyName">字段名</param>
		/// <param name="equal">是等于( = ),还是( <![CDATA[ <> ]]>)</param>
		/// <param name="hasQuote">是否需要加单引号(如: varchar, datetime等类型)</param>
		/// <returns>Where片段(不包含WHERE关键字)</returns>
		public static string GetWhereClip<T>(List<T> valueList, string keyName, bool hasQuote, bool equal)
		{
			string clip = string.Empty;
			if (valueList.Count > 0)
			{
				if (valueList.Count == 1)
				{
					if (equal)
					{
						clip = String.Format(hasQuote ? "{0} = '{1}'" : "{0} = {1}", keyName, valueList[0]);
					}
					else
					{
						clip = String.Format(hasQuote ? "{0} <> '{1}'" : "{0} <> {1}", keyName, valueList[0]);
					}
				}
				else if (valueList.Count > 1)
				{
					foreach (T val in valueList)
					{
						clip += String.Format(hasQuote ? ",'{0}'" : ",{0}", val);
					}
					if (equal)
					{
						clip = String.Format("{0} IN ( {1} )", keyName, clip.TrimStart(','));
					}
					else
					{
						clip = String.Format("{0} NOT IN ( {1} )", keyName, clip.TrimStart(','));
					}
				}
			}
			return clip;
		}

		/// <summary>
		/// 不管是否包含WHERE关键字,均返回含有Where关键字的片段
		/// </summary>
		/// <param name="whereClip">Where片段</param>
		/// <returns>Where片段(包含WHERE关键字)</returns>
		public static string GetWhereClip(string whereClip)
		{
			string clip = string.Empty;
			if (whereClip != null && whereClip.Trim() != string.Empty)
			{
				if (whereClip.Length > 5)
					clip = whereClip.Trim().Substring(0, 5).ToLower() != "where" ? " WHERE " + whereClip : whereClip;
				else
					clip = "where " + whereClip;
			}
			return clip;
		}

		#endregion

		#region GetOrderClip

		/// <summary>
		/// 返回 Order By 片段(包含 ORDER BY 关键字,例: ORDER BY ID DESC)
		/// </summary>
		/// <param name="orderKeyNameList">排序字段列表</param>
		/// <param name="orderByASCList">排序方式列表</param>
		/// <param name="reverse">是否反转</param>
		/// <returns>返回 Order By 片段(包含 ORDER BY 关键字,例: ORDER BY ID DESC)</returns>
		public static string GetOrderClip(List<string> orderKeyNameList, List<bool> orderByASCList, bool reverse)
		{
			string clip = string.Empty;
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			if (orderByASCList == null)
			{
				orderByASCList = new List<bool>(0);
			}
			int nameListCount = orderKeyNameList.Count;
			int ascListCount = orderByASCList.Count;
			if (nameListCount > ascListCount)
			{
				for (int i = ascListCount; i < nameListCount - ascListCount; i++)
				{
					orderByASCList.Add(true);
				}
			}
			for (int i = 0; i < nameListCount; i++)
			{
				clip += String.Format(", {0} {1} ", orderKeyNameList[i], orderByASCList[i] == reverse ? "DESC" : "ASC");
			}
			if (clip.Trim() != string.Empty)
			{
				clip = " ORDER BY " + clip.Trim().TrimStart(',');
			}
			return clip;
		}

		#endregion

		#region GetHavingClip

		/// <summary>
		/// 不管是否包含Having关键字,均返回含有Having关键字的片段
		/// </summary>
		/// <param name="whereClip">Having片段</param>
		/// <returns>Having片段(包含Having关键字)</returns>
		public static string GetHavingClip(string HavingClip)
		{
			string clip = string.Empty;
			if (!string.IsNullOrWhiteSpace(HavingClip))
			{
				if (HavingClip.Length > 6)
					clip = HavingClip.Trim().Substring(0, 6).ToLower() != "having" ? "HAVING " + HavingClip : HavingClip;
				else
					clip = "Having " + HavingClip;
			}
			return clip;
		}

		#endregion

		#endregion


		#region GetTypeValue

		#region GetBoolean
		public static bool GetBoolean(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return false;
			}
			return reader.GetBoolean(index);
		}

		public static bool GetBoolean(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? false : Convert.ToBoolean(value);
		}

		#endregion

		#region GetByte

		public static byte GetByte(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return (byte)0;
			}
			return reader.GetByte(index);
		}

		public static byte GetByte(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? (byte)0 : Convert.ToByte(value);
		}

		#endregion

		#region GetDateTime

		public static DateTime GetDateTime(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return new DateTime(1900, 1, 1);
			}
			return reader.GetDateTime(index);
		}

		public static DateTime GetDateTime(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(value);
		}

		#endregion

		#region GetDecimal

		public static decimal GetDecimal(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return 0;
			}
			return reader.GetDecimal(index);
		}

		public static decimal GetDecimal(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? 0 : Convert.ToDecimal(value);
		}

		#endregion

		#region GetDouble

		public static double GetDouble(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return 0;
			}
			return reader.GetDouble(index);
		}

		public static double GetDouble(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? 0 : Convert.ToDouble(value);
		}

		#endregion

		#region GetFloat

		public static float GetFloat(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return 0;
			}
			return reader.GetFloat(index);
		}

		public static float GetFloat(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? 0 : Convert.ToSingle(value);
		}

		#endregion

		#region GetGuid

		public static Guid GetGuid(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return Guid.Empty;
			}
			return reader.GetGuid(index);
		}

		public static Guid GetGuid(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? Guid.Empty : new Guid(value.ToString());
		}

		#endregion

		#region GetInt16

		public static short GetInt16(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return 0;
			}
			return reader.GetInt16(index);
		}

		public static short GetInt16(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? (short)0 : Convert.ToInt16(value);
		}

		#endregion

		#region GetInt32

		public static int GetInt32(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return 0;
			}
			return reader.GetInt32(index);
		}

		public static int GetInt32(IDataReader reader, string name)
		{
			object value = reader.GetOrdinal(name);
			return Convert.IsDBNull(value) ? 0 : Convert.ToInt32(value);
		}

		#endregion

		#region GetInt64

		public static long GetInt64(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return (long)0;
			}
			return reader.GetInt64(index);
		}

		public static long GetInt64(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? 0 : Convert.ToInt64(value);
		}

		#endregion

		#region GetString

		public static string GetString(IDataReader reader, int index, EntityBase Entity)
		{
			if (reader.IsDBNull(index))
			{
				Entity.AddNullField(reader.GetName(index));
				return string.Empty;
			}
			return reader.GetString(index);
		}

		public static string GetString(IDataReader reader, string name)
		{
			object value = reader[name];
			return Convert.IsDBNull(value) ? string.Empty : value.ToString();
		}

		#endregion

		#endregion


		#region GetListParam

		/// <summary>
		/// 反射获取SQL参数
		/// </summary>
		/// <typeparam name="T">实体类型</typeparam>
		/// <param name="t">实体类型</param>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		/// <returns>键值对</returns>
		public static Dictionary<string, SqlParameter> GetListParam<T>(T t, string key, string value)
		{
			Type type = t.GetType();
			Dictionary<string, SqlParameter> dicParam = new Dictionary<string, SqlParameter>();
			for (int i = 0; i < type.GetProperties().Length; i++)
			{
				PropertyInfo p = type.GetProperties()[i];
				SqlParameter _params = new SqlParameter();
				_params.ParameterName = "@" + p.Name;
				if (_params != null)
				{
					if (key == p.Name)
					{
						_params.Value = value;
					}
					else
					{
						_params.Value = p.GetValue(t, null);
					}
					dicParam.Add(p.Name, _params);
				}
			}
			return dicParam;
		}

		public static Dictionary<string, SqlParameter> GetListParam<T>(T t)
		{
			Type type = t.GetType();
			Dictionary<string, SqlParameter> dicParam = new Dictionary<string, SqlParameter>();
			for (int i = 0; i < type.GetProperties().Length; i++)
			{
				PropertyInfo p = type.GetProperties()[i];
				SqlParameter _params = new SqlParameter();
				_params.ParameterName = "@" + p.Name;
				if (_params != null)
				{
					_params.Value = p.GetValue(t, null);
					dicParam.Add(p.Name, _params);
				}
			}
			return dicParam;
		}

		#endregion


		#region DictionaryToArray
		public static T[] DictionaryToArray<T>(Dictionary<string, T> dic)
		{
			List<T> list = new List<T>();
			foreach (KeyValuePair<string, T> kv in dic)
			{
				list.Add(kv.Value);
			}
			return list.ToArray();
		}
		#endregion
	}
}
