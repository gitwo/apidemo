using System;
using System.Web;
using System.Xml;

namespace Util
{
	public class CommonHelper
	{
		/// <summary>
		/// 获取用户的IP地址
		/// </summary>
		public static string GetRealIp()
		{
			string user_IP = HttpContext.Current.Request.Headers["X-Forwarded-For"];
			if (user_IP != null && user_IP.ToLower() != "unknown")
			{
				//X-Forwarded-For: client1, proxy1, proxy2    
				string[] arrIp = user_IP.Split(',');
				user_IP = arrIp[0];
				if (arrIp.Length > 1)
				{    //如果第一组IP是10和168开头还有172.16-172.31（第二码区间在16-31之间）的话，就取第二组IP
					if (user_IP.IndexOf("10.") == 0 || user_IP.IndexOf("192.168.") == 0 || (user_IP.IndexOf("172.") == 0 && (user_IP.Split('.').Length > 1 && Convert.ToInt32(user_IP.Split('.')[1]) > 15 && Convert.ToInt32(user_IP.Split('.')[1]) < 32)))
					{
						user_IP = arrIp[1];
					}
				}
			}
			else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null && HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToLower() != "unknown")
			{
				user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			else
			{
				user_IP = HttpContext.Current.Request.UserHostAddress;
			}
			if (user_IP.Length > 15)
			{
				user_IP = user_IP.Substring(0, 15);
			}
			return RegexUtil.GetIP(user_IP);
		}

		/// <summary>
		/// 获取XmlDocument
		/// </summary>
		/// <param name="urlPath"></param>
		/// <returns></returns>
		public static XmlDocument GetXmlData(string urlPath)
		{
			try
			{
				if (urlPath.Contains("/"))
				{
					urlPath = HttpContext.Current.Server.MapPath(urlPath);
				}
				XmlDocument doc = new XmlDocument();
				doc.Load(urlPath);
				return doc;
			}
			catch
			{
				return null;
			}
		}
	}
}
