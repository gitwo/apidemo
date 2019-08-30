using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace BLL.Core
{
	public class MessageFactory
	{
		/// <summary>
		/// MessageTemple 路径
		/// </summary>
		private static readonly string MessageTempleConfig = ConfigurationManager.AppSettings["MessageTemple"];
		private static IMessageTemple IMsgTemple = null;
		private static readonly object lock_temple = new object();
		/// <summary>
		/// 创建DAL实例
		/// </summary>
		/// <typeparam name="T">接口</typeparam>
		/// <returns>DAL实例</returns>
		public static List<MessageTemple> GetMessage(string msgFlag)
		{
			lock (lock_temple)
			{
				try
				{
					if (IMsgTemple == null)
					{
						string[] assembleInfos = MessageTempleConfig.Split(',');
						IMsgTemple = Assembly.Load(assembleInfos[1]).CreateInstance(assembleInfos[0]) as IMessageTemple;
					}
					return IMsgTemple == null ? null : IMsgTemple.GetTemple(msgFlag);
				}
				catch (Exception)
				{					
					return null;
				}
			}
		}
	}
}
