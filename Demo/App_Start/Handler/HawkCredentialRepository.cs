using HawkNet;
using HawkNet.WebApi;
using System.Threading.Tasks;
using Util;

namespace Demo.App_Start.Handler
{
	/// <summary>
	/// [Install-Package HawkNet]  [Install-Package HawkNet.WebApi]
	/// </summary>
	public class HawkCredentialRepository: IHawkCredentialRepository
	{
		/// <summary>
		/// 根据appid 查询相应用户的Key和加密方式
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<HawkCredential> GetCredentialsAsync(string id)
		{		
			var coreApiAppId = ConfigHelper.GetConfigString("BillAppId");
			var coreApiAppSecret = ConfigHelper.GetConfigString("BillAppSecret");


			if (coreApiAppId.Equals(id))
			{
				return Task.FromResult(new HawkCredential
				{
					Id = id,
					Key = coreApiAppSecret,
					Algorithm = "sha256",
					User = id
				});
			}
			return null;
		}
	}
}