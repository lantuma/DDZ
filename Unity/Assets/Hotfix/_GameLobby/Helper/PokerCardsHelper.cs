using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ETHotfix
{
    public class PokerCardsHelper
    {
        //操作掩码
        public enum OPERATE_POKER_MASK
        {
            UG_HUA_MASK = 0xF0,  // 1111 0000
            UG_VAL_MASK = 0x0F,  // 0000 1111
        };

        //扑克牌牌值采用16进制
        public static readonly byte[] PokerArray = {
                0x00,                 //Non
                0x01,0x11,0x21,0x31,  //方块2~黑桃2
                0x02,0x12,0x22,0x32,  //方块3~黑桃3
                0x03,0x13,0x23,0x33,  //方块4~黑桃4
                0x04,0x14,0x24,0x34,  //方块5~黑桃5
                0x05,0x15,0x25,0x35,  //方块6~黑桃6
                0x06,0x16,0x26,0x36,  //方块7~黑桃7
                0x07,0x17,0x27,0x37,  //方块8~黑桃8
                0x08,0x18,0x28,0x38,  //方块9~黑桃9
                0x09,0x19,0x29,0x39,  //方块10~黑桃10
                0x0A,0x1A,0x2A,0x3A,  //方块J~黑桃J
                0x0B,0x1B,0x2B,0x3B,  //方块Q~黑桃Q
                0x0C,0x1C,0x2C,0x3C,  //方块K~黑桃K
                0x0D,0x1D,0x2D,0x3D,  //方块A~黑桃A
                0x4E,0x4F             //鬼（王）
            };

        /// <summary>
        /// 获取花色
        /// </summary>
        /// <param name="iPoker">牌</param>
        /// <returns></returns>
        public static byte GetPokerHua(byte iPoker)
        {
            return (byte)((iPoker & (int)OPERATE_POKER_MASK.UG_HUA_MASK));
        }

        /// <summary>
        /// 获取牌值(满足牌值定义做一个偏移)
        /// </summary>
        /// <param name="iPoker">牌</param>
        /// <returns></returns>
        public static byte GetPokerNum(byte iPoker)
        {
            return (byte)((iPoker & (int)OPERATE_POKER_MASK.UG_VAL_MASK) + 1);
        }

        /// <summary>
        /// 获取int类型的牌
        /// </summary>
        /// <param name="iPoker"></param>
        /// <returns></returns>
        public static int GetPokerOfInt(byte iPoker)
        {
            int target = 0;
            byte hua = GetPokerHua(iPoker);
            if (hua == 0x20) target += 13; // 红桃
            else if (hua == 0x10) target += 26; // 梅花
            else if (hua == 0x00 || hua == 0x40) target += 39; // 方块

            int num = GetPokerNum(iPoker);
            num = num == 14 ? 0 : num - 1;
            target += num;

            return target;
        }

        /// <summary>
        /// 获取牌的字符串
        /// </summary>
        /// <param name="iPoker"></param>
        /// <returns></returns>
        public static string GetPokerString(byte iPoker)
        {
            var str = "";
            var hua = GetPokerHua(iPoker);
            var val = GetPokerNum(iPoker);

            if (hua == 0x00) str += "方块";
            else if (hua == 0x10) str += "梅花";
            else if (hua == 0x20) str += "红桃";
            else if (hua == 0x30) str += "黑桃";

            if (val < 0x0B) str += val;
            else if (val == 0x0B) str += "J";
            else if (val == 0x0C) str += "Q";
            else if (val == 0x0D) str += "K";
            else if (val == 0x0E) str += "A";

            if (iPoker == 0x4E) str = "BlackJoker";
            else if (iPoker == 0x4F) str = "RedJoker";

            return str;
        }

        /// <summary>
        /// 获取炸金花牌型字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetZjhTypeString(ZjhCardType type)
        {
            switch (type)
            {
                case ZjhCardType.Normal:
                    return "单张";
                case ZjhCardType.Double:
                    return "对子";
                case ZjhCardType.Sequence:
                    return "顺子";
                case ZjhCardType.EntireHua:
                    return "金花";
                case ZjhCardType.EntireHuaSequence:
                    return "顺金";
                case ZjhCardType.Leopard:
                    return "豹子";
                default:
                    return "单张";
            }
        }

        /// <summary>
        /// 获取一张随机的牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="hasJoker">是否要王</param>
        /// <returns></returns>
        public static byte TackOneCard(List<byte> iSourcePokers, bool hasJoker = false)
        {
            if (hasJoker && iSourcePokers.Count <= 1) return 0;
            if (!hasJoker && iSourcePokers.Count <= 3) return 0;
            var index = RandomHelper.GetRandom().Next(1, hasJoker ? iSourcePokers.Count : iSourcePokers.Count - 2);
            return iSourcePokers[index];
        }

        #region 无大小王通用算法

        /// <summary>
        /// 取指定数字牌,排除某个花色
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="num"></param>
        /// <param name="exceptHua"></param>
        /// <returns></returns>
        public static byte TackOneCardOfNumExceptHua(List<byte> iSourcePokers, byte num, byte exceptHua)
        {
            var cards = iSourcePokers.Where(a => GetPokerNum(a) == num && GetPokerHua(a) != exceptHua || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        /// <summary>
        /// 获取一张非某花色,且非某数字的牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="exceptHua"></param>
        /// <param name="exceptNum"></param>
        /// <returns></returns>
        public static byte TackOneCardOfExceptHuaAndNum(List<byte> iSourcePokers, byte exceptHua, byte exceptNum)
        {
            var cards = iSourcePokers.Where(a => GetPokerHua(a) != exceptHua && GetPokerNum(a) != exceptNum || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        /// <summary>
        /// 获取一张非某花色的牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="exceptHua"></param>
        /// <returns></returns>
        public static byte TackOneCardOfExceptHua(List<byte> iSourcePokers, byte exceptHua)
        {
            var cards = iSourcePokers.Where(a => GetPokerHua(a) != exceptHua || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        /// <summary>
        /// 获取一张非某数字的牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="exceptNum"></param>
        /// <returns></returns>
        public static byte TackOneCardOfExceptNum(List<byte> iSourcePokers, byte exceptNum)
        {
            var cards = iSourcePokers.Where(a => GetPokerNum(a) != exceptNum || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        /// <summary>
        /// 取一张某个花色的牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="hua"></param>
        /// <returns></returns>
        public static byte TackOneCardOfHua(List<byte> iSourcePokers, byte hua)
        {
            var cards = iSourcePokers.Where(a => GetPokerHua(a) == hua || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        /// <summary>
        /// 获取指定数字牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="iNum"></param>
        /// <returns></returns>
        public static byte TackOneCardOfNumber(List<byte> iSourcePokers, byte iNum)
        {
            var cards = iSourcePokers.Where(poker => GetPokerNum(poker) == iNum || IsOtherPoker(poker)).ToArray();
            if (cards.Length <= 3) return 0;
            var index = RandomHelper.GetRandom().Next(1, cards.Length - 2);
            return cards[index];
        }

        /// <summary>
        /// 获取指定数字牌
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="iPoker"></param>
        /// <returns></returns>
        public static byte TackOneCardOfPoker(List<byte> iSourcePokers, byte iPoker)
        {
            return TackOneCardOfNumber(iSourcePokers, GetPokerNum(iPoker));
        }

        /// <summary>
        /// 是否是其他牌
        /// 0x00 None标记牌
        /// 0x4E 小王
        /// 0x4F 大王
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        private static bool IsOtherPoker(byte poker)
        {
            return poker == 0x00 || poker == 0x4E || poker == 0x4F;
        }

        #endregion

        #region 快速排序

        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="invert"></param>
        public static byte[] QuickSort(byte[] iSourcePokers, int start, int end, bool invert = false)
        {
            var pokers = new byte[iSourcePokers.Length];
            Array.Copy(iSourcePokers, pokers, iSourcePokers.Length);
            while (true)
            {
                if (start >= end) return pokers;
                var index = invert ?
                    INTERNAL_QUICKSORT_INVERT(pokers, start, end) :
                    INTERNAL_QUICKSORT(pokers, start, end);

                pokers = QuickSort(pokers, start, index - 1, invert);
                start = index + 1;
            }
        }
        /// <summary>
        /// 寻找基准点
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static int INTERNAL_QUICKSORT(byte[] iSourcePokers, int start, int end)
        {
            var poker = iSourcePokers[start];
            while (start < end)
            {
                while (GetPokerNum(iSourcePokers[end]) >= GetPokerNum(poker) && start < end) end--;
                iSourcePokers[start] = iSourcePokers[end];
                while (GetPokerNum(iSourcePokers[start]) <= GetPokerNum(poker) && start < end) start++;
                iSourcePokers[end] = iSourcePokers[start];
            }
            iSourcePokers[start] = poker;
            return start;
        }
        /// <summary>
        /// 寻找基准点_逆序
        /// </summary>
        /// <param name="iSourcePokers"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static int INTERNAL_QUICKSORT_INVERT(byte[] iSourcePokers, int start, int end)
        {
            var poker = iSourcePokers[start];
            while (start < end)
            {
                while (GetPokerNum(iSourcePokers[end]) <= GetPokerNum(poker) && start < end) end--;
                iSourcePokers[start] = iSourcePokers[end];
                while (GetPokerNum(iSourcePokers[start]) >= GetPokerNum(poker) && start < end) start++;
                iSourcePokers[end] = iSourcePokers[start];
            }
            iSourcePokers[start] = poker;
            return start;
        }

        #endregion
    }
}