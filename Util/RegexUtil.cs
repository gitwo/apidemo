using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Util
{
	/// <summary>
	/// 正则封装方法类
	/// </summary>
	public static class RegexUtil
	{
		#region 正则匹配
		/// <summary>
		/// 匹配整数
		/// </summary>
		public static bool MatchInt(string num)
		{
			return Regex.IsMatch(num, @"^[0-9]+$");
		}

		/// <summary>
		/// 匹配字母
		/// </summary>
		public static bool MatchWords(string str)
		{
			return Regex.IsMatch(str, @"^[A-Za-z]+$");
		}

		/// <summary>
		/// 匹配密码格式
		/// </summary>
		public static bool MatchPwdFormat(string Pwd)
		{
			return Regex.IsMatch(Pwd, "^[A-Za-z0-9]{6,10}$");
		}

		/// <summary>
		/// 邮箱验证
		/// </summary>
		public static bool MatchEmail(string source)
		{
			return Regex.IsMatch(source, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// 匹配IP
		/// </summary>
		public static bool MatchIP(string ip)
		{
			return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
		}

		/// <summary>
		/// 匹配手机号码格式
		/// </summary>
		public static bool MatchPhoneNum(string phoneNum)
		{
			return Regex.IsMatch(phoneNum, @"^1(3[0-35-9]|(34[0-8])|4[56789]|5[0-35-9]|6[6]|7[345678]|8[0-9]|9[89])(\d{7}|\d{8})$");
		}

		/// <summary>
		/// 匹配中文名[正常2-4个汉字 特殊10·10个汉字]
		/// </summary>
		public static bool MatchChaneseName(string str)
		{
			return Regex.IsMatch(str, "^([\u4E00-\u9FA5]{2,4})$") || Regex.IsMatch(str, "^([\u4E00-\u9FA5]{1,10}·[\u4E00-\u9FA5]{1,10})$");
		}

		/// <summary>
		/// 匹配手机或移动设备
		/// </summary>
		public static bool MatchMobile()
		{
			bool flag = false;
			string u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
			Regex b = new Regex(@"(android|bb\d+|meego).+mobile|android|miuibrowser|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od|ad)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			if (string.IsNullOrEmpty(u))
			{
				flag = true;
			}
			else
			{
				if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
					flag = true;
			}
			return flag;
		}
		#endregion

		#region 正则提取
		/// <summary>
		/// 提取数字
		/// </summary>
		public static string GetNum(string str)
		{
			return Regex.Replace(str, @"[^\d]*", "");
		}

		/// <summary>
		/// 提取英文字母
		/// </summary>
		public static string GetWords(string str)
		{
			Regex r = new Regex("^[A-Za-z]+$");
			Match c = r.Match(str);

			return c.Value;
		}

		/// <summary>
		/// 提取英文字母+数字
		/// </summary>
		public static string GetWordsAndNums(string str)
		{
			return Regex.Replace(str, @"[^a-zA-Z0-9]", "");
		}

		/// <summary>
		/// 多个空格替换为一个空格
		/// </summary>
		public static string GetOneSpace(string str)
		{
			return Regex.Replace(str, @"\s{2,}", " ");
		}

		/// <summary>
		/// 截取IP格式
		/// </summary>
		public static string GetIP(string ip)
		{
			Regex reip = new Regex("(\\d+).(\\d+).(\\d+).(\\d+)");
			return reip.Replace(ip, "$1.$2.$3.$4");
		}

		/// <summary>
		/// 截取英文字母,数字和汉字[可保留一些特殊字符]
		/// </summary>
		/// <param name="str">需要截取的原始字符串</param>
		/// <param name="SpecialText">特殊的中文汉字</param>
		public static string GetWordsAndNumsAndText(string str, string SpecialText)
		{
			return Regex.Replace(str, string.Format(@"[^a-zA-Z0-9\u4E00-\u9FA5{0}]", SpecialText), "");
		}

		/// <summary>
		/// 提取URL地址中Host
		/// </summary>
		public static string GetHost(string url)
		{
			string text, pattern, s;
			MatchCollection mc;

			text = url;
			pattern = @"(?<=http://)[\w\.]+[^/]";
			mc = Regex.Matches(text, pattern);
			s = "";
			foreach (Match m in mc)
			{
				s = m.ToString();
			}
			return s;
		}

		/// <summary>
		/// 通过name使用正则获取input的value值
		/// </summary>
		/// <param name="name">input标签的name</param>
		/// <param name="html">页面</param>
		/// <returns>value</returns>
		public static string GetValueByName(string name, string html)
		{
			string regex = "<input id=\"" + name + "\" name=\"" + name + "\" type=\"hidden\" value=\"(.*?)\" />";
			Regex r = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			if (!r.IsMatch(html ?? ""))
				return "";
			return r.Match(html).Result("$1");
		}

		/// <summary>
		/// 获取url 参数值
		/// </summary>
		/// <param name="url">URl</param>
		/// <param name="paraNmae">参数名称</param>
		public static string RegGetQueryString(string url, string paraNmae)
		{
			Regex urlRegex = new Regex(@"(?:^|/?|&)" + paraNmae + "=([^&]*)(?:&|$)");
			Match m = urlRegex.Match(url.ToLower());
			string paraVue = string.Empty;
			if (m.Success)
				paraVue = m.Groups[1].Value;

			return paraVue;
		}

		/// <summary>
		/// 过滤所有HTML 标签
		/// </summary>
		public static string NoHTML(string Htmlstring)
		{
			Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
			Htmlstring = Htmlstring.Replace("<", "");
			Htmlstring = Htmlstring.Replace(">", "");
			Htmlstring = Htmlstring.Replace("\r\n", "");

			return Htmlstring;
		}

		/// <summary>
		/// 防sql注入
		/// </summary>
		/// <param name="source"></param>
		public static string filterSql(this string source)
		{
			if (source == null) return source;
			//单引号替换成两个单引号
			source = source.Replace("'", "''");
			source = Regex.Replace(source, "delete", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "drop", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "update", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "insert", " ", RegexOptions.IgnoreCase);

			source = Regex.Replace(source, "select", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "create", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "union", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "hex", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "alter", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "master", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "truncate", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "declare", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "having", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "group", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "sysobjects", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "syscolumns", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "systypes", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "sysdatabases", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "between", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, " or ", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "replace", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "where", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "set", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "join", "", RegexOptions.IgnoreCase);
			//source = Regex.Replace(source,"inner", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "from", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "like", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "exists", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "ascii", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "user", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "disable", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "enable", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "fetch", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "backup", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "cursor", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "script", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "length", "", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "length", "", RegexOptions.IgnoreCase);

			source = source.Replace("--", "");
			source = source.Replace("\"", "“");
			//source = source.Replace("|", "｜");
			//半角封号替换为全角封号，防止多语句执行
			source = source.Replace(";", "；");
			//半角括号替换为全角括号
			source = source.Replace("(", "（");
			source = source.Replace(")", "）");
			//去除执行存储过程的命令关键字
			source = Regex.Replace(source, "exec", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "execute", " ", RegexOptions.IgnoreCase);
			//去除系统存储过程或扩展存储过程关键字
			source = Regex.Replace(source, "xp_", "x p_", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "sp_", "s p_", RegexOptions.IgnoreCase);
			//防止16进制注入
			source = Regex.Replace(source, "0x", "0 x", RegexOptions.IgnoreCase);
			//防止脚本注入
			source = Regex.Replace(source, "<script", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<link", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<applet", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<embed", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<object", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<form", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<frame", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<iframe", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<body", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<style", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</script", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</link", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</applet", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</embed", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</object", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</form", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</frame", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</iframe", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</body", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "</style", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "<?php", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "javascript:", " ", RegexOptions.IgnoreCase);
			source = Regex.Replace(source, "%3c", " ", RegexOptions.IgnoreCase);
			return source;
		}

		#endregion

		#region 返回正则表达式
		/// <summary>
		/// 英文开头+数字
		/// </summary>
		public static Regex RegexWordsAndNum
		{
			get
			{
				return new Regex(@"([a-zA-Z]+)[0-9]");
			}
		}

		/// <summary>
		/// 非数字开头+数字
		/// </summary>
		public static Regex RegexNonnumAndNum
		{
			get
			{
				return new Regex("([^0-9]+)\\d");
			}
		}

		/// <summary>
		/// 非(字母空格.')
		/// </summary>
		public static Regex RegexNon
		{
			get
			{
				return new Regex("[^(a-zA-Z|\\s.’)]");
			}
		}

		#endregion
	}
}
