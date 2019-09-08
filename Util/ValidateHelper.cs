using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
	/// <summary>
	/// 验证类
	/// </summary>
	public class ValidateHelper
	{
		/// <summary>
		/// 验证请求ip是否在白名单内
		/// </summary>
		/// <param name="whiteIp"></param>
		/// <returns>在白名单内true</returns>
		public static bool ValidateWhiteIp(string whiteIp)
		{
			if (whiteIp.Contains(CommonHelper.GetRealIp()) || string.IsNullOrEmpty(whiteIp))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 请求头验证
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <param name="content"></param>
		/// <param name="secretKey"></param>
		/// <returns></returns>
		public static bool ValidateSignatureHeader(HttpRequestMessage requestMessage, string content, string secretKey)
		{
			IEnumerable<string> keys;
			requestMessage.Headers.TryGetValues(ConstClass.SIGNATURE_PARA, out keys);
			if (keys.Count() > 0)
			{   //加密方式，MD5(内容+key)
				return keys.First() == Encryptions.Md5Encryptor32(string.Concat(content, secretKey));
			}
			return false;
		}

		/// <summary>
		/// 数据包验证
		/// </summary>
		/// <param name="content"></param>
		/// <param name="aesKey"></param>
		/// <returns></returns>

		public static bool ValidateAes(string content, string aesKey,out EncryptionDataModel model)
		{
			model = null;
			try
			{
				if (!string.IsNullOrEmpty(content))
				{
					string rawContent = Encryptions.AesDecryption(content, aesKey);
					if (!string.IsNullOrEmpty(rawContent))
					{
						model= JsonConvert.DeserializeObject<EncryptionDataModel>(rawContent);
						return true;
					}
				}
			}
			catch
			{

			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticks"></param>
		/// <param name="validMins">限定时间间隔有效</param>
		/// <returns>验证通过</returns>
		public static bool ValidateTimeStamp(long ticks, double validMins)
		{
			if (ticks > 0)
			{
				DateTime t1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, 0).AddMilliseconds(ticks);
				DateTime t2 = DateTime.UtcNow;
				double mins = (t2 - t1).TotalMinutes;
				if (Math.Abs(mins) <= validMins) return true;
			}
			return false;
		}



	}
}
