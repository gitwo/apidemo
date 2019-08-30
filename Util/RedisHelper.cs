using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Util
{
	/// <summary>
	/// stackexchange.redis 操作类[Install-Package StackExchange.Redis]
	/// </summary>
	public static class RedisHelper
	{
		private static readonly string Coonstr = ConfigHelper.GetConfigString("RedisConnStr");

		/// <summary>
		/// redis是否存在错误，存在错误 5分钟不使用redis
		/// </summary>
		public static bool IsError
		{
			private set
			{
				DataCache.SetCache("RedisError", value, DateTime.Now.AddMinutes(5), TimeSpan.Zero);
			}
			get
			{
				object redisError = DataCache.GetCache("RedisError");
				if (redisError == null) return false;
				return (bool)redisError;
			}
		}

		/// <summary>
		/// 使用的是Lazy，在真正需要连接时创建连接。
		/// 延迟加载技术
		/// 微软azure中的配置 连接模板       
		/// </summary>
		private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
		{
			ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Coonstr);
			muxer.ConnectionFailed += MuxerConnectionFailed;
			muxer.ConnectionRestored += MuxerConnectionRestored;
			muxer.ErrorMessage += MuxerErrorMessage;
			muxer.ConfigurationChanged += MuxerConfigurationChanged;
			muxer.HashSlotMoved += MuxerHashSlotMoved;
			muxer.InternalError += MuxerInternalError;
			return muxer;
		});

		public static ConnectionMultiplexer Instance
		{
			get
			{
				return lazyConnection.Value;
			}
		}

		/// <summary>
		/// 获得redis数据库
		/// </summary>
		/// <returns></returns>
		public static IDatabase GetDatabase()
		{
			return Instance.GetDatabase();
		}

		/// <summary>
		/// 这里的 MergeKey 用来拼接 Key 的前缀，具体不同的业务模块使用不同的前缀。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static string MergeKey(string key)
		{
			return "BillApi:" + key;
		}

		/// <summary>
		/// 根据key获取缓存对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T Get<T>(string key)
		{
			var result = Get(key);
			if (string.IsNullOrEmpty(result))
				return default(T);
			return JsonConvert.DeserializeObject<T>(result);
		}

		/// <summary>
		/// 根据key获取缓存对象
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Get(string key)
		{
			if (IsError) return string.Empty;
			try
			{
				key = MergeKey(key);
				return GetDatabase().StringGet(key);
			}
			catch
			{
				IsError = true;
				return string.Empty;
			}
		}

		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set(string key, object value)
		{
			Set(key, value, null);
		}

		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="exp"></param>
		public static void Set(string key, object value, TimeSpan? exp = null)
		{
			if (IsError) return;
			try
			{
				key = MergeKey(key);
				var json = JsonConvert.SerializeObject(value);
				GetDatabase().StringSet(key, json, exp);
			}
			catch
			{
				IsError = true;
			}
		}

		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set(string key, string value)
		{
			if (IsError) return;
			try
			{
				key = MergeKey(key);
				GetDatabase().StringSet(key, value);
			}
			catch
			{
				IsError = true;
			}
		}

		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="exp"></param>
		public static void Set(string key, string value, TimeSpan? exp = null)
		{
			if (IsError) return;
			try
			{
				key = MergeKey(key);
				GetDatabase().StringSet(key, value, exp);
			}
			catch
			{
				IsError = true;
			}
		}

		/// <summary>
		/// 判断在缓存中是否存在该key的缓存数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Exists(string key)
		{
			if (IsError) return false;
			try
			{
				key = MergeKey(key);
				return GetDatabase().KeyExists(key);  //可直接调用
			}
			catch
			{
				IsError = true;
				return false;
			}
		}

		/// <summary>
		/// 移除指定key的缓存
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Remove(string key)
		{
			if (IsError) return false;
			try
			{
				key = MergeKey(key);
				return GetDatabase().KeyDelete(key);
			}
			catch
			{
				IsError = true;
				return false;
			}
		}

		/// <summary>
		/// 删除一组缓存
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public static bool RemoveKeys(RedisKey[] keys)
		{
			return GetDatabase().KeyDelete(keys) > 0;
		}

		/// <summary>
		/// 设置或者获取redis缓存
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="ts"></param>
		/// <param name="fun"></param>
		/// <returns></returns>
		public static T SetOrGetValue<T>(string key, TimeSpan? ts = null, Func<T> fun = null)
		{
			if (Exists(key))
			{
				return Get<T>(key);
			}
			else
			{
				T t = fun.Invoke();
				Set(key, t, ts);
				return t;
			}
		}

		/// <summary>
		/// 配置更改时
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
		{
		}

		/// <summary>
		/// 发生错误时
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
		{
		}

		/// <summary>
		/// 重新建立连接之前的错误
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
		{
		}

		/// <summary>
		/// 连接失败 ， 如果重新连接成功你将不会收到这个通知
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
		{
		}

		/// <summary>
		/// 更改集群
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
		{
		}

		/// <summary>
		/// redis类库错误
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
		{
		}
	}
}
