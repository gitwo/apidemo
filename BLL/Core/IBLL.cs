using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Core
{
	/// <summary>
	/// BLL接口
	/// </summary>
	public interface IBLL
	{
		#region  成员方法

		object ISelect_Spr(int topN);
		object ISelect_Spr(string Where, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(int topN, string Where, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(bool Distinct, int topN, bool Percent);
		object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema);
		object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName);
		object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(bool Distinct, int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string E_TName, string Where, string GroupBy, string Having, List<string> paramsName, List<object> paramsValues);
		object ISelect_Spr(bool Distinct, int topN, bool Percent, string schema, string B_TName, string E_TName, string Where, string GroupBy, string Having, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues);
		bool IInsert(object entity);
		object IInsertPk(object entity);
		bool IUpdate(object entity);
		bool IUpdate(object entity, List<string> sWhere);
		bool IUpdate(object entity, List<string> sWhere, List<object> sValue);
		bool IUpdate(string c_set, string c_where, List<string> sKey, List<object> sValue);
		int IDelete(string whereClip, params IDataParameter[] parameters);
		int IDelete(string whereClip, List<string> paramsName, List<object> paramsValues);
		int ICount(string whereClip);
		int ICount(string whereClip, List<string> paramsName, List<object> paramsValues);
		int ICount_nolock(string whereClip);
		object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> paramsName, List<object> paramsValues, int dqy, int mys);
		object ISelect_Spr(int topN, string schema, string E_TName, string Where, List<string> OrderBy, List<bool> OrderBy1, List<string> paramsName, List<object> paramsValues, int dqy, int mys);

		#endregion  成员方法
	}
	public interface IBLL<T>
	{
		#region  成员方法
		#endregion  成员方法
	}
}
