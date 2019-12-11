/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 出牌规则 }                                                                                                                   
*         【修改日期】{ 2019年6月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System.Collections;
using System.Collections.Generic;

namespace ETHotfix
{
    /// <summary>
    /// 出牌规则
    /// </summary>
    public class DDZCardRule
    {
        /// <summary>
        /// 是否是单
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsSingle(List<int> cards)
        {
            if (cards.Count == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否是对子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDouble(List<int> cards)
        {
            if (cards.Count == 2)
            {
                if (cards[0] == cards[1])
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 是否是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(List<int> cards)
        {
            if (cards.Count < 5 || cards.Count > 12)
                return false;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                int w = cards[i];

                if (cards[i + 1] - w != 1)
                    return false;

                //不能超过A
                if (w > 13 || cards[i + 1] > 13)//zy:w> 12 
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否是双顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(List<int> cards)
        {
            if (cards.Count < 6 || cards.Count % 2 != 0)
                return false;

            for (int i = 0; i < cards.Count; i += 2)
            {
                if (cards[i + 1] != cards[i])
                    return false;

                if (i < cards.Count - 2)
                {
                    if (cards[i + 2] - cards[i] != 1)
                        return false;

                    //不能超过A
                    if (cards[i] > 13 || cards[i + 2] > 13)//zy:>12
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 三不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsOnlyThree(List<int> cards)
        {
            if (cards.Count % 3 != 0)
                return false;
            if (cards[0] != cards[1])
                return false;
            if (cards[1] != cards[2])
                return false;
            if (cards[0] != cards[2])
                return false;

            return true;
        }

        /// <summary>
        /// 三带一
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndOne(List<int> cards)
        {
            if (cards.Count != 4)
                return false;

            if (cards[0] == cards[1] && cards[1] == cards[2])
                return true;
            else if (cards[1] == cards[2] && cards[2] == cards[3])
                return true;

            return false;
        }

        /// <summary>
        /// 三带二
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(List<int> cards)
        {
            if (cards.Count != 5)
                return false;

            if (cards[0] == cards[1] && cards[1] == cards[2])
            {
                if (cards[3] == cards[4])
                    return true;
            }
            else if (cards[2] == cards[3] && cards[3] == cards[4])
            {
                if (cards[0] == cards[1])
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(List<int> cards)
        {
            if (cards.Count != 4)
                return false;

            if (cards[0] != cards[1])
                return false;

            if (cards[1] != cards[2])
                return false;

            if (cards[2] != cards[3])
                return false;

            return true;
        }

        /// <summary>
        /// 王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(List<int> cards)
        {
            if (cards.Count != 2)
                return false;

            if (cards[0] == 16)//zy:53
            {
                if (cards[1] == 17)//zy:54
                    return true;

                return false;
            }
            else if (cards[0] == 17)//zy:54
            {
                if (cards[1] == 16)//zy:53
                    return true;

                return false;
            }

            return false;
        }

        /// <summary>
        /// 飞机不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraight(List<int> cards)
        {
            if (cards.Count < 6 || cards.Count % 3 != 0)
                return false;

            for (int i = 0; i < cards.Count; i += 3)
            {
                if (cards[i + 1] != cards[i])
                    return false;

                if (cards[i + 2] != cards[i])
                    return false;

                if (cards[i + 1] != cards[i + 2])
                    return false;

                if (i < cards.Count - 3)
                {
                    if (cards[i + 3] - cards[i] != 1)
                        return false;

                    //不能超过A
                    if (cards[i] > 13 || cards[i + 3] > 13)//zy:12
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 飞机带单
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool isPlaneWithSingle(List<int> cards)
        {
            if (!HaveFour(cards))
            {
                List<int> tempThreeList = new List<int>();

                for (int i = 0; i < cards.Count; i++)
                {
                    int tempInt = 0;

                    for (int j = 0; j < cards.Count; j++)
                    {
                        if (cards[i] == cards[j])
                        {
                            tempInt++;
                        }
                    }

                    if (tempInt == 3)
                    {
                        tempThreeList.Add(cards[i]);
                    }
                }

                if (tempThreeList.Count % 3 != cards.Count % 4)
                {
                    return false;
                }
                else
                {
                    if (IsTripleStraight(tempThreeList))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 飞机带双
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool isPlaneWithTwin(List<int> cards)
        {
            if (!HaveFour(cards))
            {
                List<int> tempThreeList = new List<int>();

                List<int> tempTwoList = new List<int>();

                for (int i = 0; i < cards.Count; i++)
                {
                    int tempInt = 0;

                    for (int j = 0; j < cards.Count; j++)
                    {
                        if (cards[i] == cards[j])
                        {
                            tempInt++;
                        }
                    }
                    if (tempInt == 3)
                    {
                        tempThreeList.Add(cards[i]);
                    }
                    else if (tempInt == 2)
                    {
                        tempTwoList.Add(cards[i]);
                    }
                }

                if (tempThreeList.Count % 3 != cards.Count % 5 && tempTwoList.Count % 2 != cards.Count % 5)
                {
                    return false;
                }
                else
                {
                    if (IsTripleStraight(tempThreeList))
                    {
                        if (IsAllDouble(tempTwoList))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// 判断牌里面是否拥有4张牌
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool HaveFour(List<int> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int tempInt = 0;

                for (int j = 0; j < cards.Count; j++)
                {
                    if (cards[i] == cards[j])
                    {
                        tempInt++;
                    }
                }

                if (tempInt == 4)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断牌里面全是对子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsAllDouble(List<int> cards)
        {
            for (int i = 0; i < cards.Count % 2; i += 2)
            {
                if (cards[i] != cards[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断是否是四带二
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool isSiDaiEr(List<int> cards)
        {
            bool flag = false;

            if (cards != null && cards.Count == 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    int grade1 = cards[i];

                    int grade2 = cards[i + 1];

                    int grade3 = cards[i + 2];

                    int grade4 = cards[i + 3];

                    if (grade2 == grade1 && grade3 == grade1 && grade4 == grade1)
                    {
                        flag = true;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 判断是否符合出牌规则
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool PopEnable(List<int> cards, out DDZ_POKER_TYPE type)
        {
            type = DDZ_POKER_TYPE.DDZ_PASS;

            bool isRule = false;

            switch (cards.Count)
            {
                case 1:
                    isRule = true;

                    type = DDZ_POKER_TYPE.Single;

                    break;

                case 2:
                    if (IsDouble(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.TWIN;
                    }
                    else if (IsJokerBoom(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.KING_BOMB;
                    }
                    break;
                case 3:
                    if (IsOnlyThree(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.TRIPLE;
                    }
                    break;
                case 4:
                    if (IsBoom(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.FOUR_BOMB;
                    }
                    else if (IsThreeAndOne(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE;
                    }
                    break;
                case 5:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsThreeAndTwo(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.TRIPLE_WITH_TWIN;
                    }
                    break;
                case 6:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isSiDaiEr(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.FOUR_WITH_SINGLE;
                    }
                    break;
                case 7:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    break;
                case 8:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;
                    }
                    break;
                case 9:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    break;
                case 10:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithTwin(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                    }
                    break;
                case 11:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    break;
                case 12:
                    if (IsStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    break;
                case 13:
                    break;
                case 14:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    break;
                case 15:
                    if (IsTripleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    else if (isPlaneWithTwin(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                    }
                    break;
                case 16:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;
                    }
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    break;
                case 19:
                    break;
                case 20:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;
                    }
                    else if (isPlaneWithTwin(cards))
                    {
                        isRule = true;

                        type = DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                    }
                    break;
                default:
                    break;
            }
            return isRule;
        }
        
    }
}
