using System;
using System.Linq;
using System.Threading.Tasks;
using Util;
using WebApiThrottle;

namespace Demo.App_Start.Handler
{
	/// <summary>
	/// Throttle限流存储数据自定义实现
	/// </summary>
	public class ThrottleRepository:IThrottleRepository
	{
		/// <summary>
		/// redis异常状态 true 异常， false 正常
		/// </summary>
		private bool CatchStatus
		{
			get
			{
				var result = DataCache.GetCache("Throttle_Catch");
				if (result != null)
					return (bool)result;
				return false;
			}
			set
			{
				DataCache.SetCache("Throttle_Catch", value, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
			}
		}

		/// <summary>
		/// 初始化执行以下  判断redis是否异常
		/// </summary>
		public ThrottleRepository()
		{
			Task.Run(() =>
			{
				Any("test");
			});
		}

		/// <summary>
		/// 组装 redis key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private string MergeKey(string key)
		{
			return "Throttle:" + key;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Any(string id)
		{
			var result = false;
			try
			{
				if (!CatchStatus)
					result = RedisHelper.Exists(MergeKey(id));
			}
			catch
			{
				CatchStatus = true;
				result = false;
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			try
			{
				if (!CatchStatus)
				{
					var keys = RedisHelper.Instance.GetServer(RedisHelper.Instance.GetEndPoints().FirstOrDefault()).Keys(pattern: "CoreAPI:Throttle:*");
					RedisHelper.RemoveKeys(keys.ToArray());
				}
			}
			catch
			{
				CatchStatus = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ThrottleCounter? FirstOrDefault(string id)
		{
			ThrottleCounter? result = null;
			try
			{
				if (!CatchStatus)
					result = RedisHelper.Get<ThrottleCounter>(MergeKey(id));
			}
			catch
			{
				CatchStatus = true;
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public void Remove(string id)
		{
			try
			{
				if (!CatchStatus)
					RedisHelper.Remove(MergeKey(id));
			}
			catch
			{
				CatchStatus = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="throttleCounter"></param>
		/// <param name="expirationTime"></param>
		public void Save(string id, ThrottleCounter throttleCounter, TimeSpan expirationTime)
		{
			try
			{
				if (!CatchStatus)
					RedisHelper.Set(MergeKey(id), throttleCounter, expirationTime);
			}
			catch
			{
				CatchStatus = true;
			}
		}
	}
}