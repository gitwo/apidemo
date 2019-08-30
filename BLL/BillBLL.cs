using BLL.Core;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace BLL
{
    public class BillBLL:BLLBase<BillModel,BillDAL>
    {
		#region 构造
		/// <summary>
		/// 默认构造函数
		/// </summary>
		public BillBLL() : this(new DataConnection()) { }

		/// <summary>
		/// 构造函数
		/// </summary>
		public BillBLL(DataConnection con) : base(con) { }

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
		/// 
		/// </summary>
		/// <param name="member"></param>
		/// <param name="type"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
		public static List<BillModel> GetBill(string member, int type, DateTime startTime, DateTime endTime)
		{
			DataTable data = null;
			using (var conn = new DataConnection(DBType.Bill))
			{
				var dal = new BillDAL(conn.Connection, conn.Transaction);
				data =  dal.GetBill();
			}
			return data.ToList<BillModel>();
		}

		#endregion

		#endregion
	}
}
