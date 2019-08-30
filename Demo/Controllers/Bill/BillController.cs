using BLL;
using Demo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Demo.Controllers.Bill
{
	/// <summary>
	/// 测试账单数据
	/// </summary>
	public class BillController : ApiBaseController
	{
		/// <summary>
		/// 获取账单
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		public ApiResult<List<BillResModel>> GetBill(BillReqModel req)
		{
			//查询开始时间和结束时间必须30天内
			if (req == null || req.StartTime >= req.EndTime || (req.EndTime - req.StartTime).TotalDays > 30)
			{
				return Error<List<BillResModel>>(ApiResultCode.BadRequest);
			}
			var data = BillBLL.GetBill(req.Member, req.Type, req.StartTime, req.EndTime);
			var result = data.Select(s =>
			{
				var temp = new BillResModel();				
				temp.Id = s.Id;
				temp.Member = s.member;
				return temp;
			}).ToList();
			return Success(result);
		}
	}
}
