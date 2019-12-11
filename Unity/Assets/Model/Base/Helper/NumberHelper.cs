/******************************************************************************************
*         【模块】{ 基础模块 }                                                                                                                      
*         【功能】{ 数字帮助类 }                                                                                                                   
*         【修改日期】{ 2019年4月25日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ETModel
{
    public static class NumberHelper
    {
        /// <summary>
        /// 当数字超过10万时，将数字进行转换。例如10万的金币，"117315"转换成"11.73万
        /// </summary>
        /// <param name="num">待转换数字</param>
        /// <returns>转换结果</returns>
        public static string _FormatMoney(double num)
        {
            if (num >= 10000 || num <= -10000)
            {
                return Math.Round(num / 100) / 100 + "万";
            }
            else
            {
                return num + "";
            }
        }

        /// <summary>
        /// 转换金币为格式字符串
        /// </summary>
        /// <param name="num">待转换数字</param>
        /// <returns>转换结果</returns>
        public static string FormatMoney(double coin)
        {
            if (coin < 0) return "0.00";

            if (coin < 1e4) return $"{coin:###0.00}";

            if (coin < 1e8) return $"{Math.Floor(coin / 1e3) / 1e1:F1}万";

            return $"∞";
        }

        /// <summary>
        /// 将数字格式化为时间数字,例如 5->"05"
        /// </summary>
        /// <param name="num">待格式化数字</param>
        /// <returns>格式化后的数字字符串</returns>
        public static string FormatTime(int num)
        {
            if (num >= 0 && num < 10)
            {
                return "0" + num;
            }
            else
            {
                return num + "";
            }
        }
        
    }
}