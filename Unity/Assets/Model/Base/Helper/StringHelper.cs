using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ETModel
{
	public static class StringHelper
	{
		public static IEnumerable<byte> ToBytes(this string str)
		{
			byte[] byteArray = Encoding.Default.GetBytes(str);
			return byteArray;
		}

		public static byte[] ToByteArray(this string str)
		{
			byte[] byteArray = Encoding.Default.GetBytes(str);
			return byteArray;
		}

	    public static byte[] ToUtf8(this string str)
	    {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return byteArray;
        }

		public static byte[] HexToBytes(this string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
			}

			var hexAsBytes = new byte[hexString.Length / 2];
			for (int index = 0; index < hexAsBytes.Length; index++)
			{
				string byteValue = "";
				byteValue += hexString[index * 2];
				byteValue += hexString[index * 2 + 1];
				hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return hexAsBytes;
		}

		public static string Fmt(this string text, params object[] args)
		{
			return string.Format(text, args);
		}

		public static string ListToString<T>(this List<T> list)
		{
			StringBuilder sb = new StringBuilder();
			foreach (T t in list)
			{
				sb.Append(t);
				sb.Append(",");
			}
			return sb.ToString();
		}

        /// <summary>
        /// 获取字符串长度，中文、大写字母占2， 其他占1(zy add)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStrLength(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str).Length;

        }

        /// <summary>
        /// 将字符串截取到指定字符数，多余的用"..."表示(zy add)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charMax"></param>
        /// <returns></returns>
        public static string FormatNickName(string str, double charMax = 10)
        {
            int  len = GetStrLength(str);

            if (len > charMax)
            {
                return str.Substring(0, (int)Math.Round(charMax / 2) - 1) + "...";
            }

            return str;
        }
		
		public static string MessageToStr(object message)
		{
#if SERVER
			return MongoHelper.ToJson(message);
#else
			return Dumper.DumpAsString(message);
#endif
		}
	}
}