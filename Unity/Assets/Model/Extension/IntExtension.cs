using System;
using System.Text;
using UnityEngine;

//namespace MMGame.Framework
//{
    public static class IntExtension
    {
        /// <summary>
        /// 时间转换为短时间格式(06:00)
        /// </summary>
        /// <returns>The to short time.</returns>
        /// <param name="second">Second.</param>
        public static string ConvertToShortTime(this int second)
        {
            string time = string.Format("{0:D2}:{1:D2}", second / 60, second % 60);
            return time;
        }


        #region Time

        /// <summary>
        /// Converts to time span.
        /// </summary>
        /// <returns>The to time span.</returns>
        /// <param name="seconds">Seconds.</param>
        public static TimeSpan ConvertToTimeSpan(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns>返回转换后的时间</returns>
        /// <param name="seconds">总共的秒数</param>
        public static string ConvertToLongTime(this int seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            var builder = new StringBuilder();

            if (timeSpan.Hours > 0)
            {
                builder.AppendFormat("{0:D2}", timeSpan.Days * 24 + timeSpan.Hours);
            }

            if (timeSpan.Minutes >= 0)
            {
                if (builder.Length > 0)
                    builder.Append(":");
                builder.AppendFormat("{0:D2}", timeSpan.Minutes);
            }

            if (timeSpan.Seconds >= 0)
            {
                if (builder.Length > 0)
                    builder.Append(":");
                builder.AppendFormat("{0:D2}", timeSpan.Seconds);
            }
            return builder.ToString();
        }


        /// <summary>
        /// Gets the size of the human read.
        /// </summary>
        /// <returns>The human read size.</returns>
        /// <param name="size">Size.</param>
        public static string ConvertToHumanReadSize(this long size)
        {
            if (size <= 1024)
            {
                return string.Format("{0:N} B", size);
            }
            else if (size <= 1024 * 1024)
            {
                return string.Format("{0:N} KB", size / 1024.0);
            }
            else
            {
                return string.Format("{0:N} MB", size / 1048576.0);
            }
        }

        #endregion
    }
//}

