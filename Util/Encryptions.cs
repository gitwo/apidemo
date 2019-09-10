using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
	/// <summary>
	/// 只实现类DES加解密部分
	/// </summary>
	public class Encryptions
	{
		#region ========DES加密======== 

		/// <summary>
		/// DES加密
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public static string DESEncrypt(string Text)
		{
			return DESEncrypt(Text, "abCDeGREtgeASEFG");
		}
		/// <summary> 
		/// DES加密数据 
		/// </summary> 
		/// <param name="Text"></param> 
		/// <param name="sKey"></param> 
		/// <returns></returns> 
		public static string DESEncrypt(string Text, string sKey)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			byte[] inputByteArray;
			inputByteArray = Encoding.Default.GetBytes(Text);
			//des.Key = Encoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			//des.IV = Encoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			des.Key = Encoding.UTF8.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
			des.IV = Encoding.UTF8.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			StringBuilder ret = new StringBuilder();
			foreach (byte b in ms.ToArray())
			{
				ret.AppendFormat("{0:X2}", b);
			}
			des.Clear();
			ms.Close();
			cs.Close();
			return ret.ToString();
		}

		#endregion

		#region ========DES解密========

		/// <summary>
		/// DES解密
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public static string DESDecrypt(string Text)
		{
			return DESDecrypt(Text, "abCDeGREtgeASEFG");
		}
		/// <summary> 
		/// DES解密数据 
		/// </summary> 
		/// <param name="Text"></param> 
		/// <param name="sKey"></param> 
		/// <returns></returns> 
		public static string DESDecrypt(string Text, string sKey)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			int len;
			len = Text.Length / 2;
			byte[] inputByteArray = new byte[len];
			int x, i;
			for (x = 0; x < len; x++)
			{
				i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
				inputByteArray[x] = (byte)i;
			}
			//des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			//des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			des.Key = Encoding.UTF8.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
			des.IV = Encoding.UTF8.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			return Encoding.Default.GetString(ms.ToArray());
		}

		#endregion

		#region ========MD5 HASH========

		/// <summary>
		/// 32位MD5算法加密
		/// </summary>
		/// <param name="str">需要加密的字符串</param>
		/// <param name="time">需要加密的次数</param>
		/// <returns>加密后的字符串</returns>
		public static string Md5Encryptor32(string str, int time)
		{
			do
			{
				str = Md5Encryptor32(str);
				time--;
			} while (time > 0);
			return str;
		}
		/// <summary>
		/// 32位MD5算法加密
		/// </summary>
		/// <param name="str">需要加密的字符串</param>
		/// <param name="time">需要加密的次数</param>
		/// <param name="length">加密的长度32或16</param>
		/// <returns>加密后的字符串</returns>
		public static string Md5Encryptor32(string str, int time, int length)
		{
			do
			{
				if (length == 32)
				{
					str = Md5Encryptor32(str);
				}
				else
				{
					str = Md5Encryptor16(str);
				}
				time--;
			} while (time > 0);
			return str;
		}

		/// <summary>
		/// 32位MD5算法加密
		/// </summary>
		/// <param name="str">需要加密的字符串</param>
		/// <param name="lower">小写还是大写（默认小写）</param>
		/// <returns>加密后的字符串</returns>
		public static string Md5Encryptor32(string str, bool lower = true)
		{
			string password = "";
			using (MD5 md5 = MD5.Create())
			{
				byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
				foreach (byte b in s)
					password += b.ToString("X2");
				return lower ? password.ToLower() : password.ToUpper();
			}
		}

		/// <summary>
		/// 16位MD5算法加密
		/// </summary>
		/// <param name="str">需要加密的字符串</param>
		/// <param name="lower">小写还是大写（默认小写）</param>
		/// <returns>加密后的字符串</returns>
		public static string Md5Encryptor16(string str, bool lower = true)
		{
			string password = "";
			using (MD5 md5 = MD5.Create())
			{
				byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
				password = BitConverter.ToString(s, 4, 8).Replace("-", "");
				return lower ? password.ToLower() : password.ToUpper();
			}
		}

		/// <summary>
		/// HMACMD5 加密算法
		/// </summary>
		/// <param name="inputStr"></param>
		/// <param name="secret"></param>
		/// <returns></returns>
		public static string HMACMD5(string inputStr, string secret)
		{
			var encoding = new UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(inputStr);
			using (var hmacmd5 = new HMACMD5(keyByte))
			{
				byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);
				return Convert.ToBase64String(hashmessage);
			}
		}

		#endregion

		#region ========AES======== CBC模式 http://www.361way.com/aes/5830.html

		/// <summary>
		/// AES密文 https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?redirectedfrom=MSDN&view=netframework-4.8
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static string AesEncryption(string plainText, string key, string iv = "ABCDEFG")
		{
			if (string.IsNullOrEmpty(plainText))
			{
				return null;
			}

			byte[] strs = Encoding.UTF8.GetBytes(plainText);
			byte[] keys = Encoding.UTF8.GetBytes(key);
			byte[] ivs = Encoding.UTF8.GetBytes(iv);

			using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
			{
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				aes.Key = keys;
				aes.IV = ivs;

				byte[] results = aes.CreateEncryptor().TransformFinalBlock(strs, 0, strs.Length);
				return Convert.ToBase64String(results);
			}

		}

		/// <summary>
		/// AES明文
		/// </summary>
		/// <param name="cipherText">密文</param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static string AesDecryption(string cipherText, string key, string iv = "ABCDEFG")
		{
			if (string.IsNullOrEmpty(cipherText))
			{
				return null;
			}

			byte[] strs = Encoding.UTF8.GetBytes(cipherText);
			byte[] keys = Encoding.UTF8.GetBytes(key);
			byte[] ivs = Encoding.UTF8.GetBytes(iv);

			using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
			{
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				aes.Key = keys;
				aes.IV = ivs;

				byte[] results = aes.CreateDecryptor().TransformFinalBlock(strs, 0, strs.Length);
				return Encoding.UTF8.GetString(results);
			}

			/*
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = keys;
				aesAlg.IV = ivs;
				aesAlg.Mode = CipherMode.CBC;
				aesAlg.Padding = PaddingMode.PKCS7;
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msDecrypt = new MemoryStream(strs))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							return srDecrypt.ReadToEnd();
						}
					}

				}
			}
			*/

		}



		#endregion
	}
}

