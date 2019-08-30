using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace BLL.Core
{
	/// <summary>
	/// 消息模板
	/// </summary>
	public class MessageTemple
	{
		/// <summary>
		/// 消息标题模板
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 消息模板约束参数
		/// </summary>
		public string Args { get; set; }

		/// <summary>
		/// 消息内容模板
		/// </summary>
		public string Msg { get; set; }
	}

	/// <summary>
	/// 消息模板接口
	/// </summary>
	public interface IMessageTemple
	{
		List<MessageTemple> GetTemple(string msgFlag);
	}

	/// <summary>
	/// Xml消息模板
	/// </summary>
	public class XmlMessageTemple : IMessageTemple
	{
		
		private static Hashtable hashMsg = Hashtable.Synchronized(new Hashtable());
		private static readonly object lock_msg = new object();
		/// <summary>
		/// 获取消息模板
		/// </summary>
		/// <param name="msgFlag"></param>
		/// <returns></returns>
		public List<MessageTemple> GetTemple(string msgFlag)
		{
			lock (lock_msg)
			{
				List<MessageTemple> msgTemples = new List<MessageTemple>();
				hashMsg.Clear();
				if (hashMsg.ContainsKey(msgFlag))
				{
					msgTemples = hashMsg[msgFlag] as List<MessageTemple>;
				}
				else
				{
					#region 从嵌入的资源中获取
					//Assembly asm = Assembly.GetExecutingAssembly();//读取嵌入式资源
					//Stream sm = asm.GetManifestResourceStream("NbTender.BLL.Core.MessageTemple.xml");
					//XmlDocument xmlDoc = new XmlDocument();
					//xmlDoc.Load(sm);
					#endregion
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/MessageTemple.xml"));
					XmlNodeList nodes = xmlDoc.SelectNodes(string.Format("root/{0}", msgFlag));
					if (nodes != null && nodes.Count > 0)
					{
						foreach (XmlNode node in nodes)
						{
							MessageTemple msgTemple = new MessageTemple();
							XmlNode nodeTitle = node.SelectSingleNode("Title");
							if (nodeTitle != null)
							{
								msgTemple.Title = nodeTitle.InnerText;
							}
							XmlNode nodeArgs = node.SelectSingleNode("Args");
							if (nodeArgs != null)
							{
								msgTemple.Args = nodeArgs.InnerText;
							}
							XmlNode nodeContent = node.SelectSingleNode("Content");
							if (nodeContent != null)
							{
								msgTemple.Msg = nodeContent.InnerText;
							}
							msgTemples.Add(msgTemple);
						}
					}

					hashMsg.Add(msgFlag, msgTemples);
				}
				return msgTemples;
			}
		}
		
	}

}
