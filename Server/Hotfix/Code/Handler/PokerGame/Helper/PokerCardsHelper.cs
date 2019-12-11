using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETHotfix
{
    public class PokerCardsHelper
    {
        
        public enum OPERATE_POKER_MASK
        {
            UG_HUA_MASK = 0xF0,  
            UG_VAL_MASK = 0x0F, 
        };
        
        public static readonly byte[] PokerArray = {
                0x00,                 
                0x01,0x11,0x21,0x31,  
                0x02,0x12,0x22,0x32, 
                0x03,0x13,0x23,0x33,  
                0x04,0x14,0x24,0x34,    
                0x05,0x15,0x25,0x35,    
                0x06,0x16,0x26,0x36,    
                0x07,0x17,0x27,0x37,    
                0x08,0x18,0x28,0x38,    
                0x09,0x19,0x29,0x39,    
                0x0A,0x1A,0x2A,0x3A,    
                0x0B,0x1B,0x2B,0x3B,    
                0x0C,0x1C,0x2C,0x3C,    
                0x0D,0x1D,0x2D,0x3D,    
                0x4E,0x4F               
        };

        public static byte GetPokerHua(byte iPoker)
        {
            return (byte)((iPoker & (int)OPERATE_POKER_MASK.UG_HUA_MASK));
        }

       
        public static byte GetPokerNum(byte iPoker)
        {
            return (byte)((iPoker & (int)OPERATE_POKER_MASK.UG_VAL_MASK) + 1);
        }

        public static int GetPokerOfInt(byte iPoker)
        {
            int target = 0;
            byte hua = GetPokerHua(iPoker);
            if (hua == 0x20) target += 13;  
            else if (hua == 0x10) target += 26;     
            else if (hua == 0x00 || hua == 0x40) target += 39;  

            int num = GetPokerNum(iPoker);
            num = num == 14 ? 0 : num - 1;
            target += num;

            return target;
        }

       
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

        public static byte TackOneCard(List<byte> iSourcePokers, bool hasJoker = false)
        {
            if (hasJoker && iSourcePokers.Count <= 1) return 0;
            if (!hasJoker && iSourcePokers.Count <= 3) return 0;
            var index = RandomHelper.GetRandom().Next(1, hasJoker ? iSourcePokers.Count : iSourcePokers.Count - 2);
            return iSourcePokers[index];
        }
        
        public static byte TackOneCardOfNumExceptHua(List<byte> iSourcePokers, byte num, byte exceptHua)
        {
            var cards = iSourcePokers.Where(a => GetPokerNum(a) == num && GetPokerHua(a) != exceptHua || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }
        
        public static byte TackOneCardOfExceptHuaAndNum(List<byte> iSourcePokers, byte exceptHua, byte exceptNum)
        {
            var cards = iSourcePokers.Where(a => GetPokerHua(a) != exceptHua && GetPokerNum(a) != exceptNum || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        public static byte TackOneCardOfExceptHua(List<byte> iSourcePokers, byte exceptHua)
        {
            var cards = iSourcePokers.Where(a => GetPokerHua(a) != exceptHua || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }
        
        public static byte TackOneCardOfExceptNum(List<byte> iSourcePokers, byte exceptNum)
        {
            var cards = iSourcePokers.Where(a => GetPokerNum(a) != exceptNum || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }
        
        public static byte TackOneCardOfHua(List<byte> iSourcePokers, byte hua)
        {
            var cards = iSourcePokers.Where(a => GetPokerHua(a) == hua || IsOtherPoker(a)).ToArray();
            if (cards.Length <= 3) return 0;
            int index = RandomHelper.GetRandom().Next(1, cards.Length - 2);

            return cards[index];
        }

        public static byte TackOneCardOfNumber(List<byte> iSourcePokers, byte iNum)
        {
            var cards = iSourcePokers.Where(poker => GetPokerNum(poker) == iNum || IsOtherPoker(poker)).ToArray();
            if (cards.Length <= 3) return 0;
            var index = RandomHelper.GetRandom().Next(1, cards.Length - 2);
            return cards[index];
        }

     
        public static byte TackOneCardOfPoker(List<byte> iSourcePokers, byte iPoker)
        {
            return TackOneCardOfNumber(iSourcePokers, GetPokerNum(iPoker));
        }

       
        private static bool IsOtherPoker(byte poker)
        {
            return poker == 0x00 || poker == 0x4E || poker == 0x4F;
        }
        
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
        
    }
}