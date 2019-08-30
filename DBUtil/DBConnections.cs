using System.Configuration;

namespace DBUtil
{
	/// <summary>
	/// 数据库连接字符串类
	/// </summary>
	public class DBConnections
	{
		/// <summary>
		/// 账单数据库连接字符串
		/// </summary>
		public static string Connection_Bill
		{
			get
			{
				string connectionString = ConfigurationManager.ConnectionStrings["Connection_Bill"].ConnectionString;
				string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
				if (ConStringEncrypt == "true")
				{
					connectionString = Util.Encryptions.DESEncrypt(connectionString);
				}
				return connectionString;
			}
		}

		/// <summary>
		/// 会员数据库连接字符串
		/// </summary>
		public static string Connection_Member
		{
			get
			{
				string connectionString = ConfigurationManager.ConnectionStrings["Connection_Member"].ConnectionString;
				string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
				if (ConStringEncrypt == "true")
				{
					connectionString = Util.Encryptions.DESEncrypt(connectionString);
				}
				return connectionString;
			}
		}
	}
}
