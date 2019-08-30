using DAL.Core;
using Model.Core;
using System.Collections.Generic;
using System.Data;

namespace BLL.Core
{
	public abstract class BLLBase<Entity, Dal> : IBLL
		where Entity : EntityBase
		where Dal : DALBase<Entity>, new()
	{
		#region 成员, 构造, 方法
		/// <summary>
		/// 数据库连接
		/// </summary>
		protected DataConnection _conn = null;
		/// <summary>
		/// 泛型dal对象
		/// </summary>
		protected Dal _dal = null;
		public BLLBase(DataConnection con)
		{
			_conn = con;
			_dal = getDal(con);
		}

		/// <summary>
		/// 获取泛型dal对象实例
		/// </summary>
		/// <param name="con"></param>
		/// <returns></returns>
		private static Dal getDal(DataConnection con)
		{
			Dal re = new Dal();
			re.setConnTran(con.Connection, con.Transaction);
			return re;
		}
		#endregion

		#region 非静态

		/// <summary>
		/// 查询对应实体的表中的所有数据
		/// </summary>
		/// <param name="topN"></param>
		public List<Entity> Select()
		{
			return _dal.Select();
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <returns>存放对应实体的List对象</returns>
		public List<Entity> Select(int topN)
		{
			return _dal.Select(topN, null, null, null);
		}

		/// <summary>
		/// 根据条件查询对应实体的表中的数据
		/// </summary>
		/// <param name="whereClip">条件</param>
		/// <returns>存放对应实体的List对象</returns>
		public List<Entity> Select(string whereClip)
		{
			return _dal.Select(0, whereClip, null, null);
		}

		/// <summary>
		/// 根据条件查询对应实体的表中的数据
		/// </summary>
		/// <param name="whereClip">条件字符串</param>
		/// <param name="args">条件参数</param>
		/// <returns>存放对应实体的List对象</returns>
		public List<Entity> Select(string whereClip, params object[] args)
		{
			string whereClip2 = string.Format(whereClip, args);
			return _dal.Select(0, whereClip2, null, null);
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放对应实体的List对象</returns>
		public List<Entity> Select(int topN, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			return _dal.Select(topN, whereClip, orderKeyNameList, orderByASCList);
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
		public List<Entity> Select(int topN, bool islock, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			return _dal.Select(topN, islock, whereClip, orderKeyNameList, orderByASCList);
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
		public List<Entity> Select(int topN, bool islock, string schema, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			return _dal.Select(topN, islock, schema, whereClip, orderKeyNameList, orderByASCList);
		}

		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool Insert(Entity entity)
		{
			return _dal.Insert(entity);
		}

		/// <summary>
		/// 根据实体中修改过的字段添加新数据,新加数据的主键赋给实体 
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <returns>PK</returns>
		public object InsertPk(Entity entity)
		{
			object pkobj = _dal.InsertPk(entity);
			//if (!Convert.IsDBNull(pkobj))
			//entity.id = (int)Convert.ChangeType(pkobj, Convert.GetTypeCode(entity.id));
			return pkobj;
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool Update(Entity entity)
		{
			return _dal.Update(entity);
		}

		/// <summary>
		/// 按给定字段更新 (不默认主键)
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="sWhere"></param>
		/// <returns></returns>
		public bool Update(Entity entity, List<string> sWhere)
		{
			return _dal.Update(entity, sWhere);
		}

		/// <summary>
		/// 按给定字段更新 (不默认主键) 兼容where和set相同字段 where取sValue值
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="sWhere">where的字段名</param>
		/// <param name="sValue">where的值</param>
		/// <returns></returns>
		public bool Update(Entity entity, List<string> sWhere, List<object> sValue)
		{
			return _dal.Update(entity, sWhere, sValue);
		}

		/// <summary>
		/// 批量修改 自定义set where 并参数化 (适用于set a=a+1 where id in (...))
		/// </summary>
		/// <param name="c_set"></param>
		/// <param name="c_where"></param>
		/// <param name="sKey"></param>
		/// <param name="sValue"></param>
		/// <returns>返回bool = 更新记录数>1</returns>
		public bool Update(string c_set, string c_where, List<string> sKey, List<object> sValue)
		{
			return _dal.Update(c_set, c_where, sKey, sValue);
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int Delete(string whereClip, params IDataParameter[] parameters)
		{
			return _dal.DeleteWhere(whereClip, parameters);
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int Delete(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.DeleteWhere(whereClip, paramsName, paramsValues);
		}
		#endregion

		#region 静态

		/// <summary>
		/// 查询对应实体的表中的所有数据
		/// </summary>
		/// <param name="topN"></param>
		public static List<Entity> SelectObj()
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select();
			}
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <returns>存放对应实体的List对象</returns>
		public static List<Entity> SelectObj(int topN)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select(topN, null, null, null);
			}
		}

		/// <summary>
		/// 根据条件查询对应实体的表中的数据
		/// </summary>
		/// <param name="whereClip">条件</param>
		/// <returns>存放对应实体的List对象</returns>
		public static List<Entity> SelectObj(string whereClip)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select(0, whereClip, null, null);
			}
		}

		/// <summary>
		/// 根据条件查询对应实体的表中的数据
		/// </summary>
		/// <param name="whereClip">条件字符串</param>
		/// <param name="args">条件参数</param>
		/// <returns>存放对应实体的List对象</returns>
		public static List<Entity> SelectObj(string whereClip, params object[] args)
		{
			string whereClip2 = string.Format(whereClip, args);
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select(0, whereClip2, null, null);
			}
		}

		/// <summary>
		/// 查询对应实体的表中的前topN行数据
		/// </summary>
		/// <param name="topN">行数</param>
		/// <param name="whereClip">查询条件</param>
		/// <param name="orderKeyNameList">所按照的排序字段List对象</param>
		/// <param name="orderByASCList">上个参数的排序方式:true-对应字段按asc方式排序,false-对应字段按desc方式排序</param>
		/// <returns>存放对应实体的List对象</returns>
		public static List<Entity> SelectObj(int topN, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select(topN, whereClip, orderKeyNameList, orderByASCList);
			}
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
		public static List<Entity> SelectObj(int topN, bool islock, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select(topN, islock, whereClip, orderKeyNameList, orderByASCList);
			}
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
		public static List<Entity> SelectObj(int topN, bool islock, string schema, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select(topN, islock, schema, whereClip, orderKeyNameList, orderByASCList);
			}
		}

		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <returns></returns>
		public static bool InsertObj(Entity entity)
		{
			BeforeInsert(entity);
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Insert(entity);
			}
		}

		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <param name="c">数据库链接对象</param>
		/// <returns></returns>
		public static bool InsertObj(Entity entity, DataConnection c)
		{
			BeforeInsert(entity);
			return getDal(c).Insert(entity);
		}

		/// <summary>
		/// 根据实体中修改过的字段添加新数据,新加数据的主键赋给实体
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <returns>PK</returns>
		public static object InsertPkObj(Entity entity)
		{
			using (DataConnection c = new DataConnection())
			{
				object pkobj = getDal(c).InsertPk(entity);		
				return pkobj;
			}
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <returns></returns>
		public static bool UpdateObj(Entity entity)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Update(entity);
			}
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <param name="c">数据库链接对象</param>
		/// <returns></returns>
		public static bool UpdateObj(Entity entity, DataConnection c)
		{
			return getDal(c).Update(entity);
		}

		/// <summary>
		/// 按给定字段更新 (不默认主键)
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="sWhere"></param>
		/// <returns></returns>
		public static bool UpdateObj(Entity entity, List<string> sWhere)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Update(entity, sWhere);
			}
		}

		/// <summary>
		/// 按给定字段更新 (不默认主键) 兼容where和set相同字段 where取sValue值
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="sWhere">where的字段名</param>
		/// <param name="sValue">where的值</param>
		/// <returns></returns>
		public static bool UpdateObj(Entity entity, List<string> sWhere, List<object> sValue)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Update(entity, sWhere, sValue);
			}
		}

		/// <summary>
		/// 批量修改 自定义set where 并参数化 (用于set a=a+1 where id in (...))
		/// </summary>
		/// <param name="c_set"></param>
		/// <param name="c_where"></param>
		/// <param name="sKey"></param>
		/// <param name="sValue"></param>
		/// <returns>返回bool = 更新记录数>1</returns>
		public static bool UpdateObj(string c_set, string c_where, List<string> sKey, List<object> sValue)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Update(c_set, c_where, sKey, sValue);
			}
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public static int DeleteObj(string whereClip, List<string> paramsName, List<object> paramsValues, DataConnection c)
		{
			return getDal(c).DeleteWhere(whereClip, paramsName, paramsValues);
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public static int DeleteObj(string whereClip, params IDataParameter[] parameters)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).DeleteWhere(whereClip, parameters);
			}
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public static int DeleteObj(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).DeleteWhere(whereClip, paramsName, paramsValues);
			}
		}

		#endregion

		#region 功能
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="startRecord">查询开始的记录数号</param>
		/// <param name="maxRecords">一页最大行数</param>
		/// <param name="sortColumns">排序列组成的字符串</param>
		/// <returns></returns>
		public static List<Entity> PagingSelect(int startRecord, int maxRecords, string sortColumns)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).PagingSelect(startRecord, maxRecords, sortColumns, GetWhereClip(), null, null);
			}
		}

		/// <summary>
		/// 分页查询用到的总记录数
		/// </summary>
		/// <returns></returns>
		public static int CountAll()
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).SelectCount(GetWhereClip());
			}
		}

		/// <summary>
		/// 组成where条件的字符串方法
		/// </summary>
		/// <returns></returns>
		public static string GetWhereClip()
		{
			string result = "";
			return result;
		}

		/// <summary>
		/// 插入前的处理方法（处理主键生成等）
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns></returns>
		public static void BeforeInsert(Entity entity)
		{
		}

		#region Count

		/// <summary>
		/// 按条件查询Count(*)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int Count(string whereClip)
		{
			return _dal.Count(whereClip);
		}
		/// <summary>
		/// 按条件查询Count(*)(参数化)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int Count(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Count(whereClip, CommandType.Text, GetSqlParams(paramsName, paramsValues));
		}
		/// <summary>
		/// 
		/// </summary>
		public int Count(string sql, CommandType commandType, params IDataParameter[] args)
		{
			return _dal.Count(sql, commandType, args);
		}

		/// <summary>
		/// 按条件查询Count(*)(静态)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public static int CountObj(string whereClip)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Count(whereClip);
			}
		}

		/// <summary>
		/// 按条件查询Count(*)(静态参数化)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <param name="paramsName"></param>
		/// <param name="paramsValues"></param>
		/// <returns></returns>
		public static int CountObj(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Count(whereClip, CommandType.Text, GetSqlParams(paramsName, paramsValues));
			}
		}

		#endregion

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
		public DataTable Select(int topN, string schema, bool islock, string whereClip, List<string> orderKeyNameList, List<bool> orderByASCList, params IDataParameter[] parameters)
		{
			return _dal.Select(topN, schema, islock, whereClip, orderKeyNameList, orderByASCList, parameters);
		}

		/// <summary>
		/// 根据条件查询对应实体的表中的数据
		/// </summary>
		/// <param name="whereClip">条件</param>
		/// <returns>存放对应实体的List对象</returns>
		public static List<Entity> SelectByParams(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).SelectUsingParams(0, whereClip, null, null, paramsName, paramsValues);
			}
		}

		#endregion

		#region  自定义SPR
		#region  非静态SPR

		/// <summary>
		/// 多功能查询
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
		public List<Entity> Select_Spr(int topN)
		{
			return _dal.Select_Spr(false, topN, false);
		}

		/// <summary>
		/// 多功能查询
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
		public List<Entity> Select_Spr(string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(false, 0, false, "", "", Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(int topN, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(false, topN, false, "", "", Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(int topN, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(false, topN, false, schema, E_TName, Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent)
		{
			return _dal.Select_Spr(Distinct, topN, Percent);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(topN, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, string GroupBy, string Having, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, GroupBy, Having, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public List<Entity> Select_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, B_TName, E_TName, Where, GroupBy, Having, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		#endregion

		#region  静态SPR

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(int topN)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(false, topN, false);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(string Where, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(false, 0, false, "", "", Where, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(int topN, string Where, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(false, topN, false, "", "", Where, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(int topN, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(false, topN, false, schema, E_TName, Where, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent, string schema)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent, schema);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent, schema, E_TName);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(topN, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, string GroupBy, string Having, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, GroupBy, Having, paramsName, paramsValues);
			}
		}

		/// <summary>
		/// 查询
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
		public static List<Entity> SelectObj_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			using (DataConnection c = new DataConnection())
			{
				return getDal(c).Select_Spr(Distinct, topN, Percent, schema, B_TName, E_TName, Where, GroupBy, Having, OrderBy, OrderBy1, paramsName, paramsValues);
			}
		}

		#endregion
		#endregion

		#region  接口

		#region Count
		/// <summary>
		/// 按条件查询Count(*)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int ICount(string whereClip)
		{
			return _dal.Count(whereClip);
		}
		/// <summary>
		/// 按条件查询Count(*)(参数化)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int ICount(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Count(whereClip, GetSqlParams(paramsName, paramsValues));
		}

		/// <summary>
		/// 按条件查询Count(*)
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int ICount_nolock(string whereClip)
		{
			return _dal.Count_nolock(whereClip);
		}
		#endregion

		/// <summary>
		/// 多功能查询
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
		public object ISelect_Spr(int topN)
		{
			return _dal.Select_Spr(false, topN, false);
		}

		/// <summary>
		/// 多功能查询
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
		public object ISelect_Spr(string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(false, 0, false, "", "", Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(int topN, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(false, topN, false, "", "", Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(false, topN, false, schema, E_TName, Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询 分页
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
		public object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues, int dqy, int mys)
		{
			return _dal.Select_Spr(false, topN, false, schema, "", E_TName, Where, "", "", null, null, paramsName, paramsValues, dqy, mys);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(topN, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues, int dqy, int mys)
		{
			return _dal.Select_Spr(false, topN, false, schema, "", E_TName, Where, "", "", OrderBy, OrderBy1, paramsName, paramsValues, dqy, mys);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent)
		{
			return _dal.Select_Spr(Distinct, topN, Percent);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, string GroupBy, string Having, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, E_TName, Where, GroupBy, Having, paramsName, paramsValues);
		}

		/// <summary>
		/// 查询
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
		public object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.Select_Spr(Distinct, topN, Percent, schema, B_TName, E_TName, Where, GroupBy, Having, OrderBy, OrderBy1, paramsName, paramsValues);
		}

		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool IInsert(object entity)
		{
			return _dal.Insert(entity as Entity);
		}

		/// <summary>
		/// 根据实体中修改过的字段添加新数据,返回新数据的主键
		/// </summary>
		/// <param name="entity">实体对象</param>
		/// <returns>PK</returns>
		public object IInsertPk(object entity)
		{
			return _dal.InsertPk(entity as Entity);
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool IUpdate(object entity)
		{
			return _dal.Update(entity as Entity);
		}

		/// <summary>
		/// 按给定字段更新 (不默认主键)
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="dateTime">原修改时间</param>
		/// <returns></returns>
		public bool IUpdate(object entity, List<string> sWhere)
		{
			return _dal.Update(entity as Entity, sWhere);
		}

		/// <summary>
		/// 按给定字段更新 (不默认主键) 兼容where和set相同字段 where取sValue值
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="sWhere">where的字段名</param>
		/// <param name="sValue">where的值</param>
		/// <returns></returns>
		public bool IUpdate(object entity, List<string> sWhere, List<object> sValue)
		{
			return _dal.Update(entity as Entity, sWhere, sValue);
		}

		/// <summary>
		/// 批量修改 自定义set where 并参数化 (适用于set a=a+1 where id in (...))
		/// </summary>
		/// <param name="c_set"></param>
		/// <param name="c_where"></param>
		/// <param name="sKey"></param>
		/// <param name="sValue"></param>
		/// <returns>返回bool = 更新记录数>1</returns>
		public bool IUpdate(string c_set, string c_where, List<string> sKey, List<object> sValue)
		{
			return _dal.Update(c_set, c_where, sKey, sValue);
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int IDelete(string whereClip, params IDataParameter[] parameters)
		{
			return _dal.DeleteWhere(whereClip, parameters);
		}

		/// <summary>
		/// where条件参数化删除语句
		/// </summary>
		/// <param name="whereClip"></param>
		/// <returns></returns>
		public int IDelete(string whereClip, List<string> paramsName, List<object> paramsValues)
		{
			return _dal.DeleteWhere(whereClip, paramsName, paramsValues);
		}

		#endregion

		/// <summary>
		/// 拼接参数
		/// </summary>
		/// <param name="paramsName"></param>
		/// <param name="paramsValues"></param>
		/// <returns></returns>
		public static IDataParameter[] GetSqlParams(List<string> paramsName, List<object> paramsValues)
		{
			return DALBase<Entity>.GetSqlParams(paramsName, paramsValues);
		}
	}
}
