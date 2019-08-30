using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core
{
	internal static class DALConst
	{
		
		/// <summary>
		/// 完整select语句
		/// {0}Distinct去重复, 
		/// {1}TopClip多少行, 
		/// {2} SchemaClip字段, 
		/// {3} TName表名, 
		/// {4} Where条件, 
		/// {5} GroupBy分组, 
		/// {6} Having限定, 
		/// {7} OrderBy排序
		/// {8} OFFSET分页
		/// </summary>
		public const string Sql_Select_All = "SELECT {0}{1}{2}FROM {3}{4}{5}{6}{7}{8}";


		#region 通用SQL语句

		/// <summary>
		/// SELECT COUNT(*) FROM {0} {1}    0:TableName, 1:WhereClip
		/// </summary>
		public const string Sql_Count = "SELECT COUNT(*) FROM {0} {1}";
		public const string Sql_Count_nolock = "SELECT COUNT(*) FROM {0} with (nolock) {1}";

		/// <summary>
		/// SELECT {0} {1} FROM {2} {3} {4}   0:TopClip, 1:SchemaClip, 2:TableName, 3:WhereClip, 4:OrderByClip
		/// </summary>
		public const string Sql_Select = "SELECT {0} {1} FROM {2} {3} {4}";
		public const string Sql_Select_nolock = "SELECT {0} {1} FROM {2} with (nolock) {3} {4}";

		//  PKName 和 传入字段 双字段排序 {0} Schema, {1} Table, {2} PKName, {3} PageSize, {4} PageIndex * PageSize, {5} SchemaClip, {6} WhereClip, {7} OderClip0, {8} OderClip1, 
		public const string Sql_Paging = "SELECT {0} FROM {1} WHERE {2} IN ( SELECT TOP {3} {2} FROM ( SELECT TOP {4} {5} FROM {1} {6} {7}) TI {8} ) {7} ";

		public const string Sql_Paging_Obj = "SELECT {0} FROM {1} WHERE {2} IN ( SELECT TOP {3} {2} FROM ( SELECT TOP {4} {5} FROM {1} {6} {7}) TI {8} ) {9} ";

		//  PKName 和 传入字段 多字段排序 {0} Schema, {1} schemaRowNumber, {2} OderClip0, {3} TableName, {4} WhereClip, {5} StartRowNumber, {6} EndRowNumber
		//public const string Sql_Paging = "SELECT {0} FROM( SELECT {1}, ROW_NUMBER() OVER ( {2} ) AS RowNumber FROM {3} {4})AS T1 WHERE T1.RowNumber >= {5} AND T1.RowNumber <= {6} {2}";

		/// <summary>
		/// INSERT INTO {0} ( {1} ) Values ( {2} )
		/// </summary>
		public const string Sql_Insert = "INSERT INTO {0} ( {1} ) Values ( {2} )";

		/// <summary>
		/// UPDATE {0} SET {1} {2}
		/// </summary>
		public const string SQl_Update = "UPDATE {0} SET {1} {2}";
		#endregion


		/// <summary>
		/// SQL语句类型
		/// </summary>
		[Serializable]
		public enum SqlType : int
		{			
			Insert = 1,			
			Update = 2,			
			Delete = 3,
		}
		
	}
}
