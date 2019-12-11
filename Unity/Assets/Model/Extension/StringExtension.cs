using System.Text.RegularExpressions;
//namespace MMGame.Framework
//{
    using System;
    using UnityEngine;

    public static class StringExtension
    {
		const string regex_markup_format_pattern = "<[a-zA-Z]+[^<>]+>.+</[a-zA-Z]+>";

        /// <summary>
        /// Shows the text tips.
        /// </summary>
        /// <param name="thiz">Thiz.</param>
        public static void ShowTextTips( this string thiz )
        {
        }
        

        /// <summary>
        /// Tos the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="thiz">Thiz.</param>
        public static int ToInt(this string thiz)
        {
            return Convert.ToInt32(thiz);
        }

        /// <summary>
        /// Tos the int64.
        /// </summary>
        /// <returns>The int64.</returns>
        /// <param name="thiz">Thiz.</param>
        public static long ToInt64(this string thiz)
        {
            return Convert.ToInt64(thiz);
        }

        public static string TrimEnter(this string thiz)
        {
            return thiz.Replace("\\n", "\n");
        }

		/// <summary>
		/// 匹配是否有标记格式(e.g:http://docs.unity3d.com/Manual/StyledText.html)
		/// </summary>
		/// <returns><c>true</c>, if markup format was hased, <c>false</c> otherwise.</returns>
		/// <param name="thiz">Thiz.</param>
		public static bool HasMarkupFormat(this string thiz)
		{
			if (string.IsNullOrEmpty(thiz))
				return false;
			
			var regex = new Regex(regex_markup_format_pattern, RegexOptions.Singleline);
			return regex.IsMatch(thiz);
		}

		/// <summary>
		/// Trims the markup format. (Trim <> => [])
		/// </summary>
		/// <returns>The markup format.</returns>
		/// <param name="thiz">Thiz.</param>
		public static string TrimMarkupFormat(this string thiz)
		{
			if (string.IsNullOrEmpty(thiz))
				return thiz;

			return thiz.Replace('<', '[').Replace('>', ']');
		}

        /// <summary>
        /// Ises the null or empty.
        /// </summary>
        /// <returns>The null or empty.</returns>
        /// <param name="thiz">Thiz.</param>
        public static bool IsNullOrEmptyExcel(this string thiz)
        {
            bool result = string.IsNullOrEmpty(thiz) || thiz.Equals("-1");
            return result;
        }
    }


//}
