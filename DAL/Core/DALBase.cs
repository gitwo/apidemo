using Model.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Util;

namespace DAL.Core
{
	/// <summary>
	/// 数据库操作基类
	/// </summary>
	/// <typeparam name="Entity"></typeparam>
	public abstract class DALBase<Entity> where Entity : EntityBase
	{
		#region DALBase
		protected DALBase(IDbConnection connection)
			: this(connection, null)
		{
		}

		protected DALBase(IDbTransaction transaction)
			: this(transaction.Connection, transaction)
		{
		}

		public DALBase(IDbConnection connection, IDbTransaction transaction)
		{
			this._Connection = connection;
			this._Transaction = transaction;
		}

		public void setConnTran(IDbConnection connection, IDbTransaction transaction)
		{
			this._Connection = connection;
			this._Transaction = transaction;
		}

		#endregion

		/// <summary>
		/// 数据库所有者
		/// </summary>
		private string db_user
		{
			get
			{
				if (HttpRuntime.Cache[ConstClass.APPLICATION_DB_USER] == null)
				{
					HttpRuntime.Cache.Insert(ConstClass.APPLICATION_DB_USER, ConfigurationManager.AppSettings[ConstClass.APPLICATION_DB_USER].ToString());
				}
				return HttpRuntime.Cache[ConstClass.APPLICATION_DB_USER].ToString();
			}
		}

		/// <summary>
		/// 数据库所有者
		/// </summary>
		public string DB_User
		{
			get { return db_user; }
		}

		#region Fields
	
		public abstract string TableName { get; }
		public abstract string PrimaryKeyName { get; }

		private IDbConnection _Connection;
		protected IDbConnection Connection
		{
			get
			{
				return _Connection;
			}
			set
			{
				_Connection = value;
			}
		}

		private IDbTransaction _Transaction;
		protected IDbTransaction Transaction
		{
			get
			{
				return this._Transaction;
			}
			set
			{
				this._Transaction = value;
			}
		}
		#endregion
		

		#region Count
		public virtual int Count()
		{
			string sql = string.Format(DALConst.Sql_Count, this.TableName, string.Empty);
			return this.Count(sql, CommandType.Text, new IDataParameter[0]);
		}

		public virtual int Count(string whereClip)
		{
			string sql = string.Format(DALConst.Sql_Count, this.TableName, SqlHelper.GetWhereClip(whereClip));
			return this.Count(sql, CommandType.Text, new IDataParameter[0]);
		}

		public int Count(string whereClip, params IDataParameter[] args)
		{
			string sql = string.Format(DALConst.Sql_Count, this.TableName, SqlHelper.GetWhereClip(whereClip));
			return Count(sql, CommandType.Text, args);
		}

		public int Count(string sql, CommandType commandType, params IDataParameter[] args)
		{
			return Convert.ToInt32(ExecuteScalar(commandType, sql, args));
		}

		public virtual int Count_nolock(string whereClip)
		{
			string sql = string.Format(DALConst.Sql_Count_nolock, this.TableName, SqlHelper.GetWhereClip(whereClip));
			return this.Count(sql, CommandType.Text, new IDataParameter[0]);
		}

		#endregion

		#region Select

		#region 返实体

		/// <summary>
		/// 查询对应实体的表中的所有数据
		/// </summary>
		/// <returns>存放对应实体的List对象</returns>
		public virtual List<Entity> Select()
		{
			return this.Select(0, null, null, null);
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <returns>存放对应实体的List对象</returns>
		public virtual List<Entity> Select(int topN)
		{
			return this.Select(topN, null, null, null);
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放对应实体的List对象</returns>
		public virtual List<Entity> Select(int topN, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			return this.Select(topN, false, whereClip, orderKeyNameList, orderByASCList);
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="islock">查询是否加with (nolock)</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放对应实体的List对象</returns>
		public virtual List<Entity> Select(int topN, bool islock, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			return this.Select(topN, islock, "*", whereClip, orderKeyNameList, orderByASCList);
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="islock">查询是否加with (nolock)</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放对应实体的List对象</returns>
		public virtual List<Entity> Select(int topN, bool islock, string schema, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			whereClip = SqlHelper.GetWhereClip(whereClip);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string sql = islock ? DALConst.Sql_Select : DALConst.Sql_Select_nolock;
			sql = string.Format(sql, topN > 0 ? "TOP " + topN : string.Empty, schema, this.TableName, whereClip, orderClip);
			return this.Select(sql, CommandType.Text, new IDataParameter[0]);
		}

		/// <summary>
		/// 查询对应实体的表中的数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="commandType">执行类型 </param>
		/// <param name="args">数据库的参数数组</param>
		/// <returns>存放对应实体的List对象</returns>
		public List<Entity> Select(string sql, CommandType commandType, params IDataParameter[] args)
		{
			IDataReader reader = null;
			List<Entity> list = new List<Entity>();
			try
			{
				reader = this.ExecuteReader(commandType, sql, args);
				if (reader == null)
				{
					return list;
				}
				Entity entity = default(Entity);
				while (reader.Read())
				{
					entity = this.Load(reader);
					if (entity != null)
					{
						list.Add(entity);
					}
				}
			}
			finally
			{
				if (reader != null && !reader.IsClosed)
				{
					reader.Close();
					reader.Dispose();
				}
			}
			return list;
		}
		#endregion

		#region 返DataTabl
		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="islock">查询是否加with (nolock)</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <param name="parameters">数据库的参数数组</param>
		/// <returns>存放查询结果的DataTable对象</returns>
		public virtual DataTable Select(int topN, string schema, bool islock, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			return this.Select(topN, schema, islock, whereClip, this.PrimaryKeyName, this.TableName, orderKeyNameList, orderByASCList, null);
		}
		public virtual DataTable Select(int topN, string schema, bool islock, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			return this.Select(topN, schema, islock, whereClip, this.PrimaryKeyName, this.TableName, orderKeyNameList, orderByASCList, parameters);
		}
		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="islock">查询是否加with (nolock)</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="primaryKeyName">主键名</param>
		/// <param name="tableName">表名或视图名</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <param name="parameters">数据库的参数数组</param>
		/// <returns>存放查询结果的DataTable对象</returns>
		public virtual DataTable Select(int topN, string schema, bool islock, string whereClip, string primaryKeyName, string tableName, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			//bool islock = false;
			schema = (schema != null && schema.Trim() != string.Empty) ? schema : "*";
			whereClip = SqlHelper.GetWhereClip(whereClip);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string sql = islock ? DALConst.Sql_Select : DALConst.Sql_Select_nolock;
			sql = string.Format(sql, topN > 0 ? "TOP " + topN : string.Empty, schema, tableName, whereClip, orderClip);
			return this.ExecuteDataTable(CommandType.Text, sql, parameters);
		}
		#endregion

		#region 多功能查询

		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent)
		{
			return Select_Spr(Distinct, topN, Percent, "");
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema)
		{
			return Select_Spr(Distinct, topN, Percent, schema, "");
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName)
		{
			return Select_Spr(Distinct, topN, Percent, schema, E_TName, "", null, null);
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, "", "", paramsName, paramsValues);
		}
		public virtual List<Entity> Select_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return Select_Spr(false, topN, false, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return Select_Spr(Distinct, topN, false, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return Select_Spr(Distinct, topN, Percent, schema, "", E_TName, Where, "", "", OrderBy, OrderBy1, GetSqlParams(paramsName, paramsValues));
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, string GroupBy, string Having, List<string> paramsName, List<object> paramsValues)
		{
			return Select_Spr(Distinct, topN, Percent, schema, "", E_TName, Where, GroupBy, Having, null, null, GetSqlParams(paramsName, paramsValues));
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return Select_Spr(Distinct, topN, Percent, schema, B_TName, E_TName, Where, GroupBy, Having, OrderBy, OrderBy1, GetSqlParams(paramsName, paramsValues));
		}
		public virtual List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues, int dqy, int mys)
		{
			return Select_Spr(Distinct, topN, Percent, schema, B_TName, E_TName, Where, GroupBy, Having, OrderBy, OrderBy1, dqy, mys, GetSqlParams(paramsName, paramsValues));
		}
		/// <summary>
		/// 查询 (基本不使用)
		/// </summary>
		/// <param name="Distinct">去重复</param>
		/// <param name="topN">多少行</param>
		/// <param name="Percent">是否top%</param>
		/// <param name="schema">字段</param>
		/// <param name="B_TName">表名前</param>
		/// <param name="E_TName">表名后</param>
		/// <param name="Where">条件</param>
		/// <param name="GroupBy">分组</param>
		/// <param name="Having">限定</param>
		/// <param name="OrderBy">排序</param>
		/// <param name="parameters">参数化</param>
		/// <returns>实体List</returns>
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, params IDataParameter[] parameters)
		{
			return Select_Spr(Distinct, topN, Percent, schema, B_TName, E_TName, Where, GroupBy, Having, OrderBy, OrderBy1, 0, 0, parameters);
		}
		/// <summary>
		/// 查询 (基本不使用)
		/// </summary>
		/// <param name="Distinct">去重复</param>
		/// <param name="topN">多少行</param>
		/// <param name="Percent">是否top%</param>
		/// <param name="schema">字段</param>
		/// <param name="B_TName">表名前</param>
		/// <param name="E_TName">表名后</param>
		/// <param name="Where">条件</param>
		/// <param name="GroupBy">分组</param>
		/// <param name="Having">限定</param>
		/// <param name="OrderBy">排序</param>
		/// <param name="dqy">当前页</param>
		/// <param name="mys">每页数</param>
		/// <param name="parameters">参数化</param>
		/// <returns>实体List</returns>
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, int dqy, int mys, params IDataParameter[] parameters)
		{			
			string _Distinct = Distinct ? "DISTINCT " : string.Empty;//去重复{0}
			string _topN = topN > 0 ? "TOP " + topN + (Percent ? " PERCENT " : " ") : string.Empty;//多少行{1} 
			string _Schema = string.IsNullOrWhiteSpace(schema) ? "* " : schema + " ";//字段{2} 
			string _B_TName = string.IsNullOrWhiteSpace(B_TName) ? string.Empty : B_TName.Trim() + " ";//表名前
			string _E_TName = string.IsNullOrWhiteSpace(E_TName) ? string.Empty : " " + E_TName.Trim();//表名后
			string _TName = _B_TName + this.TableName + _E_TName + " ";//表名{3}
			string _Where = string.IsNullOrWhiteSpace(Where) ? string.Empty : SqlHelper.GetWhereClip(Where.Trim()) + " ";//条件{4} 
			string _GroupBy = string.IsNullOrWhiteSpace(GroupBy) ? string.Empty : GroupBy.Trim() + " ";//分组{5} 
			string _Having = (string.IsNullOrWhiteSpace(GroupBy) || string.IsNullOrWhiteSpace(Having)) ? string.Empty : SqlHelper.GetHavingClip(Having.Trim()) + " ";//限定{6} 
			string _OrderBy = OrderBy == null ? string.Empty : SqlHelper.GetOrderClip(OrderBy, OrderBy1, false);//排序{7}
			string _Offset = dqy <= 0 || mys <= 0 ? string.Empty : "OFFSET " + (dqy - 1) * mys + " ROWS FETCH NEXT " + mys + " ROWS ONLY ";//分页{8}
			IDataParameter[] _parameters = string.IsNullOrWhiteSpace(_Where) ? new IDataParameter[0] : parameters;
					string sql = string.Format(DALConst.Sql_Select_All, _Distinct, _topN, _Schema, _TName, _Where, _GroupBy, _Having, _OrderBy, _Offset);
			return this.Select(sql, CommandType.Text, _parameters);
		}

		#endregion
		#endregion

		#region Single
		protected virtual Entity Single<T>(T id) where T : struct
		{
			Entity entity = default(Entity);
			if (this.PrimaryKeyName.Trim() != string.Empty)
			{
				string sql = string.Format(" SELECT TOP 1 * FROM {0} WHERE {1} = {2} ", this.TableName, this.PrimaryKeyName, id);
				entity = this.Single(sql);
			}
			return entity;
		}

		protected virtual Entity SingleById(int id)
		{
			return this.Single<int>(id);
		}

		protected virtual Entity SingleById(decimal id)
		{
			return this.Single<decimal>(id);
		}

		protected virtual Entity SingleById(double id)
		{
			return this.Single<double>(id);
		}

		protected virtual Entity SingleById(string id)
		{
			Entity entity = default(Entity);
			if (this.PrimaryKeyName.Trim() != string.Empty)
			{
				string sql = string.Format(" SELECT TOP 1 * FROM {0} WHERE {1} = '{2}' ", this.TableName, this.PrimaryKeyName, id);
				entity = this.Single(sql);
			}
			return entity;
		}

		protected virtual Entity SingleById(Guid id)
		{
			Entity entity = default(Entity);
			if (this.PrimaryKeyName.Trim() != string.Empty)
			{
				string sql = string.Format(" SELECT TOP 1 * FROM {0} WHERE {1} = '{2}' ", this.TableName, this.PrimaryKeyName, id);
				entity = this.Single(sql);
			}
			return entity;
		}

		protected virtual Entity Single(string sql, params IDataParameter[] parameters)
		{
			Entity entity = default(Entity);
			if (this.PrimaryKeyName.Trim() != string.Empty)
			{
				using (IDataReader reader = this.ExecuteReader(CommandType.Text, sql, parameters))
				{
					if (reader.Read())
					{
						return this.Load(reader);
					}
				}
			}
			return entity;
		}

		#endregion

		#region Delete
		protected virtual bool Delete1(int id)
		{
			List<int> l = new List<int>();
			l.Add(id);
			return Delete<int>(l) > 0;
		}

		protected virtual bool Delete1(decimal id)
		{
			List<decimal> l = new List<decimal>();
			l.Add(id);
			return Delete<decimal>(l) > 0;
		}

		protected virtual bool Delete1(double id)
		{
			List<double> l = new List<double>();
			l.Add(id);
			return Delete<double>(l) > 0;
		}

		protected virtual bool Delete1(string id)
		{
			bool success = false;
			if (this.PrimaryKeyName.Trim() != string.Empty)
			{
				string sql = string.Format("DELETE FROM {0} WHERE {1} = '{2}' ", this.TableName, this.PrimaryKeyName, id);
				success = (this.ExecuteSql(CommandType.Text, sql, null) >= 0);
			}
			return success;
		}

		protected virtual int Delete1(List<string> idList)
		{
			if (idList.Count == 0)
			{
				return 0;
			}
			string whereClip = SqlHelper.GetWhereClip(idList, this.PrimaryKeyName, true);
			string sql = string.Format("DELETE FROM {0} WHERE {1}", this.TableName, whereClip);
			return this.ExecuteSql(CommandType.Text, sql, null);
		}

		protected virtual bool Delete1(Guid id)
		{
			bool success = false;
			if (this.PrimaryKeyName.Trim() != string.Empty)
			{
				string sql = string.Format("DELETE FROM {0} WHERE {1} = '{2}' ", this.TableName, this.PrimaryKeyName, id);
				success = (this.ExecuteSql(CommandType.Text, sql, null) >= 0);
			}
			return success;
		}

		protected virtual int Delete<T>(List<T> idList)
		{
			if (idList == null || idList.Count == 0)
			{
				return 0;
			}
			string whereClip = SqlHelper.GetWhereClip(idList, this.PrimaryKeyName, true);
			string sql = string.Format("DELETE FROM {0} WHERE {1}", this.TableName, whereClip);
			return this.ExecuteSql(CommandType.Text, sql, null);
		}

		public int DeleteWhere(string whereClip)
		{
			string sql = string.Format("DELETE FROM {0} WHERE {1}", this.TableName, whereClip);
			return this.ExecuteSql(CommandType.Text, sql, null);
		}

		/// <summary>
		/// where条件参数化删除语句        
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int DeleteWhere(string whereClip, params IDataParameter[] parameters)
		{
			string sql = string.Format("DELETE FROM {0} WHERE {1}", this.TableName, whereClip);
			return this.ExecuteSql(CommandType.Text, sql, parameters);
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int DeleteWhere(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			string sql = string.Format("DELETE FROM {0} WHERE {1}", this.TableName, whereClip);
			return this.ExecuteSql(CommandType.Text, sql, GetSqlParams(paramsName, paramsValues));
		}

		#endregion

		#region Insert, Update, Save

		/// <summary>
		/// 根据实体中修改过的字段添加新数据,返回bool = 更新记录数>1
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>bool = 更新记录数>1</returns>
		public virtual bool Insert(Entity entity)
		{
			if (entity == null) return false;
			HashSet<string> setUpdate = entity.GetUpdatingFields();
			if (setUpdate == null || setUpdate.Count <= 0) return false;

			int i = 0;
			string sql = string.Format("INSERT INTO {0} ( ", this.TableName);
			string vals = " VALUES ( ";
			SqlParameter[] _params = new SqlParameter[setUpdate.Count];
			foreach (var field in setUpdate)
			{
				sql += string.Format("{0},", field);
				vals += string.Format("@{0},", field);
				_params[i] = GetSqlParameter(field, entity);
				i++;
			}
			sql = sql.TrimEnd(',') + ")";
			vals = vals.TrimEnd(',') + ")";
			return this.ExecuteSql(CommandType.Text, sql + vals, _params) > 0;
		}

		/// <summary>
		/// 根据实体中修改过的字段添加新数据,返回新加数据的主键
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>PK</returns>
		public virtual object InsertPk(Entity entity)
		{
			if (entity == null) return false;
			HashSet<string> setUpdate = entity.GetUpdatingFields();
			if (setUpdate == null || setUpdate.Count <= 0) return false;

			int i = 0;
			string sql = string.Format("INSERT INTO {0} ( ", this.TableName);
			string vals = " VALUES ( ";
			SqlParameter[] _params = new SqlParameter[setUpdate.Count];
			foreach (var field in setUpdate)
			{
				sql += string.Format("{0},", field);
				vals += string.Format("@{0},", field);
				_params[i] = GetSqlParameter(field, entity);
				i++;
			}
			sql = sql.TrimEnd(',') + ")";
			vals = vals.TrimEnd(',') + ")";
			sql += vals;
			sql += ";SELECT [SCOPE_IDENTITY()]=SCOPE_IDENTITY();";
			return this.ExecuteScalar(CommandType.Text, sql, _params);
		}
		/// <summary>
		/// 根据实体中修改过的字段更新数据，返回bool = 更新记录数>1 
		/// </summary>
		public virtual bool Update(Entity entity)
		{
			return Update(entity, null);
		}
		/// <summary>
		/// 根据实体中修改过的字段更新数据,可增加where条件，返回bool = 更新记录数>1
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="sWhere">尾随的where条件</param>
		/// <returns>bool = 更新记录数>1</returns>
		public virtual bool Update(Entity entity, List<string> sWhere)
		{
			return Update(entity, sWhere, null);
		}
		/// <summary>
		/// 根据实体中修改过的字段更新数据,可增加where条件，返回bool = 更新记录数>1 优化兼容where和set相同字段时 where取给定参数
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="sWhere">尾随的where条件</param>
		/// <returns>bool = 更新记录数>1</returns>
		public virtual bool Update(Entity entity, List<string> sWhere, List<object> sValue)
		{
			if (entity == null) return false;
			HashSet<string> setUpdate = entity.GetUpdatingFields();
			if (setUpdate == null || setUpdate.Count <= 0) return false;
			int i = 0;
			StringBuilder sql = new StringBuilder(string.Format("UPDATE {0} SET ", this.TableName));
			string sqlwhere = "";
			if (sWhere == null || sWhere.Count <= 0)
			{
				sqlwhere = string.Format(" WHERE {0}=@{0}", this.PrimaryKeyName);
				i++;
			}
			else
			{
				sqlwhere = " WHERE 1=1 ";
				if (sValue != null && sValue.Count > 0 && sWhere.Count == sValue.Count)
				{
					for (int ii = 0; ii < sWhere.Count; ii++)
					{
						sqlwhere += string.Format(" AND {0}=@{0}_w", sWhere[ii]);
						i++;
					}
				}
				else
				{
					foreach (var field in sWhere)
					{
						sqlwhere += string.Format(" AND {0}=@{0}", field);
						if (!setUpdate.Contains(field))
						{
							i++;
						}
					}
				}
			}
			foreach (var field in setUpdate)
			{
				sql.Append(string.Format(" {0}=@{0},", field));
			}
			sql.Remove(sql.Length - 1, 1).Append(sqlwhere);
			SqlParameter[] _params = new SqlParameter[i + setUpdate.Count];
			int j = 0;
			if (sWhere == null || sWhere.Count <= 0)
			{
				_params[j] = GetSqlParameter(this.PrimaryKeyName, entity);
				j++;
			}
			else
			{
				if (sValue != null && sValue.Count > 0 && sWhere.Count == sValue.Count)
				{
					for (int ii = 0; ii < sWhere.Count; ii++)
					{
						_params[j] = new SqlParameter(string.Format("@{0}_w", sWhere[ii]), sValue[ii]);
						j++;
					}
				}
				else
				{
					foreach (var field in sWhere)
					{
						if (!setUpdate.Contains(field))
						{
							_params[j] = GetSqlParameter(field, entity);
							j++;
						}
					}
				}
			}
			foreach (var field in setUpdate)
			{
				_params[j] = GetSqlParameter(field, entity);
				j++;
			}
			return this.ExecuteSql(CommandType.Text, sql.ToString(), _params) > 0;
		}

		/// <summary>
		/// 批量修改 自定义set where 并参数化 (适用于set a=a+1 where id in (...)) 
		/// </summary>
		/// <param name="c_set"></param>
		/// <param name="c_where"></param>
		/// <param name="sKey"></param>
		/// <param name="sValue"></param>
		/// <returns>返回bool = 更新记录数>1</returns>
		public virtual bool Update(string c_set, string c_where, List<string> sKey, List<object> sValue)
		{
			string sql = string.Format("UPDATE {0} SET ", this.TableName);
			if (string.IsNullOrWhiteSpace(c_set)) return false;
			sql += c_set;
			if (!string.IsNullOrWhiteSpace(c_where))
				sql += " where " + c_where;
			return this.ExecuteSql(CommandType.Text, sql.ToString(), GetSqlParams(sKey, sValue)) > 0;
		}
		public virtual bool Save(Entity entity)
		{
			if (entity.IsNew)
			{
				if (this.Insert(entity))
				{
					entity.IsNew = false;
					return true;
				}
				return false;
			}
			return this.Update(entity);			
		}

		#endregion

		protected virtual SqlParameter GetSqlParameter(string fieldName, Entity entity)
		{
			return null;
		}
		public abstract Entity Create();
		protected abstract Entity Load_all(IDataReader reader);
		protected abstract Entity Load(IDataReader reader);

		#region Execute

		/// <summary> 
		/// SqlDataAdapter
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns>DataTable</returns>
		protected DataTable ExecuteDataTable(CommandType commandType, string commandText, params IDataParameter[] parameters)
		{
			DataTable re = SqlHelper.ExecuteDataTable(this._Connection, this._Transaction, commandType, commandText, parameters);
			this.Close();//关闭连接
			return re;
		}

		/// <summary>
		/// 不关闭连接的SqlDataAdapter
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns>DataTable</returns>
		protected DataTable ExecuteDataTableOn(CommandType commandType, string commandText, params IDataParameter[] parameters)
		{
			DataTable re = SqlHelper.ExecuteDataTable(this._Connection, this._Transaction, commandType, commandText, parameters);
			return re;
		}

		/// <summary>
		/// SqlDataAdapter
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns>DataSet</returns>
		protected DataSet ExecuteDataSet(CommandType commandType, string commandText, params IDataParameter[] parameters)
		{
			DataSet re = SqlHelper.ExecuteDataSet(this._Connection, this._Transaction, commandType, commandText, parameters);
			this.Close();//关闭连接
			return re;
		}
		/// <summary>
		/// 查询返回 IDataReader
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns>IDataReader</returns>
		protected IDataReader ExecuteReader(CommandType commandType, string commandText, params IDataParameter[] parameters)
		{
			return SqlHelper.ExecuteReader(this._Connection, this._Transaction, commandType, commandText, parameters);
		}

		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		protected object ExecuteScalar(CommandType commandType, string commandText, params IDataParameter[] parameters)
		{
			object re = SqlHelper.ExecuteScalar(this._Connection, this._Transaction, commandType, commandText, parameters);
			re = re == DBNull.Value ? null : re;
			this.Close();//关闭连接
			return re;
		}

		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="commandType">SQL命令类型</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns>受影响行数</returns>
		protected int ExecuteSql(CommandType commandType, string commandText, params IDataParameter[] parameters)
		{
			int re = SqlHelper.ExecuteNonQuery(this._Connection, this._Transaction, commandType, commandText, parameters);
			this.Close();//关闭连接
			return re;
		}

		/// <summary>
		/// 关闭连接 若开启事务 不关闭链接
		/// </summary>
		public void Close()
		{
			if (this._Connection.State == ConnectionState.Open && this._Transaction == null)
				this._Connection.Close();
		}
		#endregion

		#region CheckPageIndex 分页计算方法

		/// <summary>
		/// 分页计算方法
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="itemCount"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		private int CheckPageIndex(int pageIndex, int itemCount, int pageSize)
		{
			int pageCount = (itemCount / pageSize) + (itemCount % pageSize == 0 ? 0 : 1);
			if (pageIndex < 1)
			{
				pageIndex = 1;
			}
			if (pageCount > 0 && pageIndex > pageCount)
			{
				pageIndex = pageCount;
			}
			return pageIndex;
		}
		#endregion

		#region PagingDataReader
		/// <summary>
		/// 查询返回 IDataReader 并计算填入分页对象中的各个数值
		/// </summary>
		/// <param name="pagerInfo">分页对象</param>
		/// <param name="whereClip">查询的条件语句</param>
		/// <param name="orderKeyNameList">排序的字段名</param>
		/// <param name="orderByASCList">排序方式</param>
		/// <returns></returns>
		public IDataReader PagingDataReader(PagerInfo pagerInfo, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			string commandText = string.Empty;
			whereClip = SqlHelper.GetWhereClip(whereClip);
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			orderKeyNameList.Add(this.PrimaryKeyName);
			if (orderByASCList == null)
			{
				orderByASCList = new List<bool>(0);
			}
			orderByASCList.Add(false);
			string orderClip0 = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string orderClip1 = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, true);
			//
			if (pagerInfo.ItemCount < 1)
			{
				commandText = string.Format(DALConst.Sql_Count, this.TableName, whereClip);
				pagerInfo.ItemCount = (int)this.ExecuteScalar(CommandType.Text, commandText, null);
			}
			if (pagerInfo.ItemCount < 1)
			{
				return null;
			}
			//  分页语句
			commandText = String.Format(DALConst.Sql_Paging, "*", this.TableName, this.PrimaryKeyName, pagerInfo.PageItemCount, pagerInfo.PageIndex * pagerInfo.PageSize, "*", whereClip, orderClip0, orderClip1);
			return this.ExecuteReader(CommandType.Text, commandText, null);
		}
		#endregion

		#region PagingDataTable(分页控件)

		/// <summary>
		/// 分页控件所用到的查询方法(查询出指定页上的数据)
		/// </summary>
		/// <param name="pagerInfo">分页对象</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <param name="parameters">数据库的参数数组</param>
		/// <returns>存放查询结果的DataTable对象</returns>
		public DataTable PagingDataTable(PagerInfo pagerInfo, string schema, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			return this.PagingDataTable(pagerInfo, schema, whereClip, this.PrimaryKeyName, this.TableName, orderKeyNameList, orderByASCList, parameters);
		}

		/// <summary>
		/// 分页控件所用到的查询方法(查询出指定页上的数据)
		/// </summary>
		/// <param name="pagerInfo">分页对象</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <param name="parameters">数据库的参数数组</param>
		/// <returns>存放查询结果的List对象</returns>
		public List<Entity> PagingList(PagerInfo pagerInfo, string schema, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			return this.PagingList(pagerInfo, schema, whereClip, this.PrimaryKeyName, this.TableName, orderKeyNameList, orderByASCList, parameters);
		}

		/// <summary>
		/// 分页控件所用到的查询方法(查询出指定页上的数据)
		/// </summary>
		/// <param name="pagerInfo">分页对象</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="primaryKeyName">主键名</param>
		/// <param name="tableName">表名或视图名</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <param name="parameters">数据库的参数数组</param>
		/// <returns>存放查询结果的DataTable对象</returns>
		public DataTable PagingDataTable(PagerInfo pagerInfo, string schema, string whereClip, string primaryKeyName, string tableName, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			string commandText = string.Empty;
			schema = (schema != null && schema.Trim() != string.Empty) ? schema : "*";
			whereClip = SqlHelper.GetWhereClip(whereClip);
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			orderKeyNameList.Add(primaryKeyName);
			if (orderByASCList == null)
			{
				orderByASCList = new List<bool>(0);
			}
			orderByASCList.Add(false);
			string schemaClip = SqlHelper.GetSchemaClip(schema, orderKeyNameList);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string orderClip1 = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, true);

			if (pagerInfo.ItemCount < 1)
			{
				commandText = string.Format(DALConst.Sql_Count, tableName, whereClip);
				pagerInfo.ItemCount = (int)this.ExecuteScalar(CommandType.Text, commandText, parameters);
			}
			if (pagerInfo.ItemCount < 1)
			{
				return null;
			}
			if (pagerInfo.PageIndex == pagerInfo.PageCount)
			{
				int u = (pagerInfo.PageIndex - 1) * pagerInfo.PageSize;
				int number = pagerInfo.ItemCount - u;
				commandText = String.Format(DALConst.Sql_Paging, schema, tableName, primaryKeyName, number, pagerInfo.PageIndex * pagerInfo.PageSize, schemaClip, whereClip, orderClip, orderClip1);
				return this.ExecuteDataTable(CommandType.Text, commandText, parameters);
			}
			//  分页语句{0} Schema, {1} Table, {2} PKName, {3} PageSize, {4} PageIndex * PageSize, {5} SchemaClip, {6} WhereClip, {7} OderClip0, {8} OderClip1, 
			int itemStart = (pagerInfo.PageIndex - 1) * pagerInfo.PageSize;
			//commandText = String.Format(DALConst.Sql_Paging, schema, schemaClip, orderClip, tableName, whereClip, itemStart, itemStart + pagerInfo.PageItemCount);

			commandText = String.Format(DALConst.Sql_Paging, schema, tableName, primaryKeyName, pagerInfo.PageSize, pagerInfo.PageIndex * pagerInfo.PageSize, schemaClip, whereClip, orderClip, orderClip1);
			return this.ExecuteDataTable(CommandType.Text, commandText, parameters);
		}

		#endregion

		#region PagingList 分页控件所用到的查询方法(查询出指定页上的数据)

		/// <summary>
		/// 分页控件所用到的查询方法(查询出指定页上的数据)
		/// </summary>
		/// <param name="pagerInfo">分页对象</param>
		/// <param name="schema">查询结果字段</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="primaryKeyName">主键名</param>
		/// <param name="tableName">表名或视图名</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <param name="parameters">数据库的参数数组</param>
		/// <returns>存放查询结果的List对象</returns>
		public List<Entity> PagingList(PagerInfo pagerInfo, string schema, string whereClip, string primaryKeyName, string tableName, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			string commandText = string.Empty;
			schema = (schema != null && schema.Trim() != string.Empty) ? schema : "*";
			whereClip = SqlHelper.GetWhereClip(whereClip);
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			orderKeyNameList.Add(primaryKeyName);
			if (orderByASCList == null)
			{
				orderByASCList = new List<bool>(0);
			}
			orderByASCList.Add(false);
			string schemaClip = SqlHelper.GetSchemaClip(schema, orderKeyNameList);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string orderClip1 = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, true);

			if (pagerInfo.ItemCount < 1)
			{
				commandText = string.Format(DALConst.Sql_Count, tableName, whereClip);
				pagerInfo.ItemCount = (int)this.ExecuteScalar(CommandType.Text, commandText, parameters);
			}
			if (pagerInfo.ItemCount < 1)
			{
				return null;
			}
			if (pagerInfo.PageIndex == pagerInfo.PageCount)
			{
				int u = (pagerInfo.PageIndex - 1) * pagerInfo.PageSize;
				int number = pagerInfo.ItemCount - u;
				commandText = String.Format(DALConst.Sql_Paging, schema, tableName, primaryKeyName, number, pagerInfo.PageIndex * pagerInfo.PageSize, schemaClip, whereClip, orderClip, orderClip1);
				return this.Select(commandText, CommandType.Text, parameters);
			}
			//  分页语句{0} Schema, {1} Table, {2} PKName, {3} PageSize, {4} PageIndex * PageSize, {5} SchemaClip, {6} WhereClip, {7} OderClip0, {8} OderClip1, 
			int itemStart = (pagerInfo.PageIndex - 1) * pagerInfo.PageSize;
			//commandText = String.Format(DALConst.Sql_Paging, schema, schemaClip, orderClip, tableName, whereClip, itemStart, itemStart + pagerInfo.PageItemCount);

			commandText = String.Format(DALConst.Sql_Paging, schema, tableName, primaryKeyName, pagerInfo.PageSize, pagerInfo.PageIndex * pagerInfo.PageSize, schemaClip, whereClip, orderClip, orderClip1);
			return this.Select(commandText, CommandType.Text, parameters);
		}
		#endregion

		#region PagingSelect （支持ASPxGridView的） 分页控件所用到的查询方法(查询出指定页上的数据)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startRecord">起始行</param>
		/// <param name="maxRecords">一页最大条数</param>
		/// <param name="sortColumns">排序字段</param>
		/// <param name="whereClip">条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放查询结果的List对象</returns>
		public List<Entity> PagingSelect(int startRecord, int maxRecords, string sortColumns, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			int startRecordNext = (startRecord + 1);
			int pageIndex = (startRecordNext / maxRecords) + ((startRecordNext % maxRecords > 0) ? 1 : 0);
			string commandText = string.Empty;
			string schema = "*";
			whereClip = SqlHelper.GetWhereClip(whereClip);
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			orderKeyNameList.Add(this.PrimaryKeyName);
			if (orderByASCList == null)
			{
				orderByASCList = new List<bool>(0);
			}
			orderByASCList.Add(false);

			string schemaClip = SqlHelper.GetSchemaClip(schema, orderKeyNameList);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string orderClip1 = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, true);

			string orderClip2 = (sortColumns != string.Empty) ? orderClip.ToLowerInvariant().Replace("order by", string.Format("order by {0},", sortColumns)) : orderClip;

			commandText = string.Format(DALConst.Sql_Count, this.TableName, whereClip);
			int itemCount = (int)this.ExecuteScalar(CommandType.Text, commandText);

			if (itemCount < 1)
			{
				List<Entity> listT = new List<Entity>();
				return listT;
			}
			int pageCount = (itemCount / maxRecords) + (itemCount % maxRecords == 0 ? 0 : 1);
			if (pageIndex == pageCount)
			{
				int u = (pageIndex - 1) * maxRecords;
				int number = itemCount - u;
				commandText = string.Format(DALConst.Sql_Paging_Obj, schema, this.TableName, this.PrimaryKeyName, number, pageIndex * maxRecords, schemaClip, whereClip, orderClip, orderClip1, orderClip2);
				return this.Select(commandText, CommandType.Text);
			}

			commandText = string.Format(DALConst.Sql_Paging_Obj, schema, this.TableName, this.PrimaryKeyName, maxRecords, pageIndex * maxRecords, schemaClip, whereClip, orderClip, orderClip1, orderClip2);
			return this.Select(commandText, CommandType.Text);

		}

		public int SelectCount(string whereClip)
		{
			string commandText = string.Empty;
			whereClip = SqlHelper.GetWhereClip(whereClip);
			commandText = string.Format(DALConst.Sql_Count, this.TableName, whereClip);
			return (int)this.ExecuteScalar(CommandType.Text, commandText);
		}

		#endregion

		#region PagingSelect （支持ExtGrid的） 分页控件所用到的视图查询方法(查询出指定页上的数据)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startRecord">起始行</param>
		/// <param name="maxRecords">一页最大条数</param>
		/// <param name="sortColumns">排序字段</param>
		/// <param name="whereClip">条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放查询结果的List对象</returns>
		public DataTable ExtPagingSelect(int startRecord, int maxRecords, string sortColumns, string whereClip, string tableName, string keyName, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			int startRecordNext = (startRecord + 1);
			int pageIndex = (startRecordNext / maxRecords) + ((startRecordNext % maxRecords > 0) ? 1 : 0);
			string commandText = string.Empty;
			string schema = "*";
			whereClip = SqlHelper.GetWhereClip(whereClip);
			if (orderKeyNameList == null)
			{
				orderKeyNameList = new List<string>(0);
			}
			orderKeyNameList.Add(keyName);
			if (orderByASCList == null)
			{
				orderByASCList = new List<bool>(0);
			}
			orderByASCList.Add(false);

			string schemaClip = SqlHelper.GetSchemaClip(schema, orderKeyNameList);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string orderClip1 = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, true);
			string orderClip2 = (sortColumns != string.Empty) ? orderClip.ToLowerInvariant().Replace("order by", string.Format("order by {0},", sortColumns)) : orderClip;

			commandText = string.Format(DALConst.Sql_Count, tableName, whereClip);
			int itemCount = (int)this.ExecuteScalar(CommandType.Text, commandText);

			if (itemCount < 1)
			{
				return null;
			}

			int pageCount = (itemCount / maxRecords) + (itemCount % maxRecords == 0 ? 0 : 1);

			if (pageIndex == pageCount)
			{
				int u = (pageIndex - 1) * maxRecords;
				int number = itemCount - u;
				commandText = string.Format(DALConst.Sql_Paging_Obj, schema, tableName, keyName, number, pageIndex * maxRecords, schemaClip, whereClip, orderClip, orderClip1, orderClip2);
				return this.ExecuteDataTable(CommandType.Text, commandText, parameters);
			}

			commandText = string.Format(DALConst.Sql_Paging_Obj, schema, tableName, keyName, maxRecords, pageIndex * maxRecords, schemaClip, whereClip, orderClip, orderClip1, orderClip2);
			return this.ExecuteDataTable(CommandType.Text, commandText, parameters);

		}

		public int SelectViewCount(string whereClip, string tableName)
		{
			string commandText = string.Empty;
			whereClip = SqlHelper.GetWhereClip(whereClip);
			commandText = string.Format(DALConst.Sql_Count, tableName, whereClip);
			return (int)this.ExecuteScalar(CommandType.Text, commandText);
		}

		#endregion

		#region 参数化查询
		/// <summary>
		/// 拼接参数
		/// </summary>
		/// <param name="paramsName"></param>
		/// <param name="paramsValues"></param>
		/// <returns></returns>
		public static IDataParameter[] GetSqlParams(List<string> paramsName, List<object> paramsValues)
		{
			if (paramsName == null || paramsValues == null || paramsName.Count != paramsValues.Count || paramsName.Count == 0)
				return new IDataParameter[0];
			IDataParameter[] param = new IDataParameter[paramsName.Count];
			for (int i = 0; i < paramsName.Count; i++)
			{
				param[i] = new SqlParameter(paramsName[i], paramsValues[i]);
			}
			return param;
		}

		/// <summary>
		/// 参数化查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放对应实体的List对象</returns>
		public virtual List<Entity> SelectUsingParams(int topN, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList, List<string> paramsName, List<object> paramsValues)
		{
			whereClip = SqlHelper.GetWhereClip(whereClip);
			string orderClip = SqlHelper.GetOrderClip(orderKeyNameList, orderByASCList, false);
			string sql = string.Format(DALConst.Sql_Select, topN > 0 ? "TOP " + topN : string.Empty, "*", this.TableName, whereClip, orderClip);
			return this.Select(sql, CommandType.Text, GetSqlParams(paramsName, paramsValues));
		}
		#endregion
	}
}
