using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace Util
{
	/// <summary>
	/// 扩展方法
	/// </summary>
	public static class ExtendHelper
	{
		static ExtendHelper()
		{
			jSetting.Formatting = Formatting.None;
			jSetting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
			jSetting.NullValueHandling = NullValueHandling.Ignore;
			jSetting.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
		}

		//排除字段值 NULL 的属性
		private static JsonSerializerSettings jSetting = new JsonSerializerSettings();

		/// <summary>
		/// 将对象转换成json字符串
		/// </summary>
		/// <param name="obj">需要转换的对象</param>
		/// <param name="ignoreNull">是否排除NULL属性，默认排除</param>
		/// <returns></returns>
		public static string ToJson(this object obj, bool ignoreNull = true)
		{
			try
			{
				if (ignoreNull)
				{
					jSetting.NullValueHandling = NullValueHandling.Ignore;			
				}
				else
				{
					jSetting.NullValueHandling = NullValueHandling.Include;					
				}
				return JsonConvert.SerializeObject(obj, jSetting);
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// 将json字符串序列化成对象
		/// </summary>
		/// <typeparam name="T">序列化的目标对象</typeparam>
		/// <param name="jsonstr">json字符串</param>
		/// <returns></returns>
		public static T ToObject<T>(this string jsonstr)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(jsonstr);
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// 获取枚举描述
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static string GetDescriptionOriginal(this Enum @this)
		{
			var name = @this.ToString();
			var field = @this.GetType().GetField(name);
			if (field == null) return name;
			var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
			return att == null ? field.Name : ((DescriptionAttribute)att).Description;
		}

		/// <summary>
		/// 判断字符串是否存在值
		/// </summary>
		/// <param name="_this"></param>
		/// <returns></returns>
		public static bool HasValue(this string _this)
		{
			if (string.IsNullOrWhiteSpace(_this))
				return false;
			return _this.Length > 0;
		}
	}
}
