using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	/// <summary>
	/// API状态码
	/// </summary>
	public enum ApiStatusCodeEnum
	{
		/// <summary>
		/// 请求成功
		/// </summary>
		OK=0,
		/// <summary>
		/// 请求失败
		/// </summary>
		Fail=1,
		/// <summary>
		/// 超时
		/// </summary>
		TimeOut=3,
		/// <summary>
		/// 数据库异常
		/// </summary>
		DbException=4,
		/// <summary>
		/// 无效用户名
		/// </summary>
		InvalidMember=5,
		/// <summary>
		/// 冻结用户
		/// </summary>
		FrozenMember=6,
		/// <summary>
		/// 金额无效
		/// </summary>
		InvalidMoney=7,
		/// <summary>
		/// 内部错误
		/// </summary>
		InternalError=8,
		/// <summary>
		/// 请求签名错误
		/// </summary>
		MisMatchedSignature=9,
		/// <summary>
		/// 缺少验证头
		/// </summary>
		MissingSignatureHeader=10,
		/// <summary>
		/// api请求受限
		/// </summary>
		ApiLimit=11

	}
}
