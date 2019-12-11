/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ AI组件 }                                                                                                                   
*         【修改日期】{ 2019年6月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZAIComponentAwakeSystem : AwakeSystem<DDZAIComponent>
    {
        public override void Awake(DDZAIComponent self)
        {
            self.Awake();
        }
    }

    public  class DDZAIComponent : Component
    {
        

        public void Awake()
        {
            
        }

        /// <summary>
        /// 当前选择的牌是否可以出
        /// </summary>
        /// <returns></returns>
        public bool CheckCanOut()
        {
            List<int> myCards = this.GetMySelectCards();

            DDZ_POKER_TYPE myCardType = this.GetPokerTYPE(myCards);

            List<int> lastCards = this.GetLastCards();

            if (lastCards.Count == 0) { return true; }

            DDZ_POKER_TYPE lastCardType = this.GetPokerTYPE(lastCards);

            return isSelectCardCanPut(myCards, myCardType, lastCards, lastCardType);
        }

        /// <summary>
        /// 检测当前能否要得起不
        /// </summary>
        /// <returns></returns>
        public bool CheckShowOutCard()
        {
            List<int> myCards = this.GetMyCards();

            List<int> lastCards = this.GetLastCards();

            if (lastCards.Count == 0) { return true; }

            if (myCards.Contains(16) && myCards.Contains(17)) { return true; }

            DDZ_POKER_TYPE lastCardType = this.GetPokerTYPE(lastCards);

            return isShowOutCardBtn(myCards, lastCards, lastCardType);
        }

        /// <summary>
        /// 获得提示牌
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetTipCards()
        {
            List<int> myCards = this.GetMyCards();

            List<int> lastCards = this.GetLastCards();
            
            if (lastCards.Count == 0) { return null; }

            DDZ_POKER_TYPE lastCardType = this.GetPokerTYPE(lastCards);

            return FindPromptCards(myCards, lastCards, lastCardType);
        }

        #region 辅助方法

        /// <summary>
        /// 获得自己选择的牌
        /// </summary>
        /// <returns></returns>
        private List<int> GetMySelectCards()
        {
            List<byte> mySelectCards = DDZConfig.GameScene.DDZHandCardPlugin.outCardList;

            var myCards = PokerCardsHelper.QuickSort(mySelectCards.ToArray(), 0, mySelectCards.Count - 1).ToList();

            return DDZGameHelper.CardFromByteToValue(myCards);
        }
        
        /// <summary>
        /// 获取自己的手牌
        /// </summary>
        /// <returns></returns>
        private List<int> GetMyCards()
        {
            DDZCard ddzCard = DDZGameConfigComponent.Instance.myHandCard;

            List<int> intList = new List<int>();

            for (int i = 0; i < ddzCard.Card.Length; i++)
            {
               int intValue = DDZGameHelper.GetPokerNum(ddzCard.Card[i]);

                intList.Add(intValue);
            }

            return intList;
        }

        /// <summary>
        /// 获得最后一家出牌
        /// </summary>
        /// <returns></returns>
        private List<int> GetLastCards()
        {
            DDZCard ddzCard = DDZGameConfigComponent.Instance.preOutCard;

            List<int> intList = new List<int>();

            if (ddzCard == null || ddzCard.Card == null) { return intList; }

            for (int i = 0; i < ddzCard.Card.Length; i++)
            {
                int intValue = DDZGameHelper.GetPokerNum(ddzCard.Card[i]);

                intList.Add(intValue);
            }

            return intList;
        }

        /// <summary>
        /// 获得牌型
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public DDZ_POKER_TYPE GetPokerTYPE(List<int> cards)
        {
            DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

            bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

            return myCardType;
        }

        #endregion

        #region 比较两个手牌的大小
        /// <summary>
        /// 比较两个手牌的大小
        /// </summary>
        /// <param name="myCards"></param>
        /// <param name="myCardType"></param>
        /// <param name="lastCards"></param>
        /// <param name="lastCardType"></param>
        /// <returns></returns>
        public bool isSelectCardCanPut(List<int> myCards, DDZ_POKER_TYPE myCardType, List<int> lastCards, DDZ_POKER_TYPE lastCardType)
        {
            //我的牌和上家的牌都不能为null
            if (myCards == null || lastCards == null)
            {
                return false;
            }

            //上家出的牌不合法
            if (myCardType == null || lastCardType == null)
            {
                return false;
            }

            //上一手 手牌的个数
            int prevSize = lastCards.Count;

            int mySize = myCards.Count;

            //我先出牌，上家没有牌
            if (prevSize == 0 && mySize != 0)
            {
                return true;
            }

            //优先判断是否王炸，免得多次判断王炸
            if (lastCardType == DDZ_POKER_TYPE.KING_BOMB)
            {
                //上家王炸，肯定不能出
                return false;
            }
            else if (myCardType == DDZ_POKER_TYPE.KING_BOMB)
            {
                //我王炸，肯定能出
                return true;
            }

            //优先判断对方不是炸弹，我出炸弹的情况
            if (lastCardType != DDZ_POKER_TYPE.FOUR_BOMB && myCardType == DDZ_POKER_TYPE.FOUR_BOMB)
            {
                return true;
            }

            //所有牌提前排序过了

            int myGrade = myCards[0];

            int prevGrade = lastCards[0];

            //1.我出和上家一种类型的牌，即对子管对子

            //单
            if (lastCardType == DDZ_POKER_TYPE.Single && myCardType == DDZ_POKER_TYPE.Single)
            {
                //一张牌可以大过上家的牌
                return compareGrade(myGrade, prevGrade);
            }
            //对子
            else if (lastCardType == DDZ_POKER_TYPE.TWIN && myCardType == DDZ_POKER_TYPE.TWIN)
            {
                //二张牌可以大过上家的牌
                return compareGrade(myGrade, prevGrade);
            }
            //3不带
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE && myCardType == DDZ_POKER_TYPE.TRIPLE)
            {
                //三张牌可以大过上家的牌
                return compareGrade(myGrade, prevGrade);
            }
            //炸弹
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_BOMB && myCardType == DDZ_POKER_TYPE.FOUR_BOMB)
            {
                //4张牌可以大过上家的牌
                return compareGrade(myGrade, prevGrade);
            }
            //3带1
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE)
            {
                // 3带1只需要比较第2张牌的大小
                myGrade = myCards[1];
                prevGrade = lastCards[1];

                return compareGrade(myGrade, prevGrade);
            }
            //3带2
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_TWIN)
            {
                //3带2只需比较第3张牌的大小
                myGrade = myCards[2];
                prevGrade = lastCards[2];

                return compareGrade(myGrade, prevGrade);
            }
            //4带2
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE && myCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
            {
                //4带2只需比较第3张牌的大小
                myGrade = myCards[2];

                prevGrade = lastCards[2];

                return compareGrade(myGrade, prevGrade);
            }
            //4带2对子
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_TWIN && myCardType == DDZ_POKER_TYPE.FOUR_WITH_TWIN)
            {
                myGrade = myCards[2];

                prevGrade = lastCards[2];

                return compareGrade(myGrade, prevGrade);
            }
            //顺子
            else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE && myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
            {
                if (mySize != prevSize)
                {
                    return false;
                }
                else
                {
                    //顺子只需比较最大的1张牌的大小
                    myGrade = myCards[mySize - 1];
                    prevGrade = lastCards[prevSize - 1];

                    return compareGrade(myGrade, prevGrade);
                }
            }
            //连对
            else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_TWIN && myCardType == DDZ_POKER_TYPE.STRAIGHT_TWIN)
            {
                if (mySize != prevSize)
                {
                    return false;
                }
                else
                {
                    //连对只需比较最大的1张牌的大小
                    myGrade = myCards[mySize - 1];
                    prevGrade = lastCards[prevSize - 1];

                    return compareGrade(myGrade, prevGrade);
                }
            }
            //飞机不带
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_PURE && myCardType == DDZ_POKER_TYPE.PLANE_PURE)
            {
                if (mySize != prevSize)
                {
                    return false;
                }
                else
                {
                    myGrade = myCards[4];
                    prevGrade = lastCards[4];

                    return compareGrade(myGrade, prevGrade);
                }
            }
            //飞机带单
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_SINGLE && myCardType == DDZ_POKER_TYPE.PLANE_WITH_SINGLE)
            {
                if (mySize != prevSize)
                {
                    return false;
                }
                else
                {
                    List<int> tempThreeList = new List<int>();

                    for (int i = 0; i < myCards.Count; i++)
                    {
                        int tempInt = 0;

                        for (int j = 0; j < myCards.Count; j++)
                        {
                            if (myCards[i] == myCards[j])
                            {
                                tempInt++;
                            }
                        }
                        if (tempInt == 3)
                        {
                            tempThreeList.Add(myCards[i]);
                        }
                    }
                    myGrade = tempThreeList[4];
                    prevGrade = lastCards[4];

                    return compareGrade(myGrade, prevGrade);

                }
            }
            //飞机带双
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_TWIN && myCardType == DDZ_POKER_TYPE.PLANE_WITH_TWIN)
            {
                if (mySize != prevSize)
                {
                    return false;
                }
                else
                {
                    List<int> tempThreeList = new List<int>();

                    List<int> tempTwoList = new List<int>();

                    for (int i = 0; i < myCards.Count; i++)
                    {
                        int tempInt = 0;

                        for (int j = 0; j < myCards.Count; j++)
                        {
                            if (myCards[i] == myCards[j])
                            {
                                tempInt++;
                            }
                        }

                        if (tempInt == 3)
                        {
                            tempThreeList.Add(myCards[i]);
                        }
                        else if (tempInt == 2)
                        {
                            tempTwoList.Add(myCards[i]);
                        }
                    }
                    myGrade = tempThreeList[4];

                    prevGrade = lastCards[4];

                    if (compareGrade(myGrade, prevGrade))
                    {
                        return DDZCardRule.IsAllDouble(tempTwoList);
                    }
                }
            }
            //默认不能出牌
            return false;
        }


        #endregion

        #region 判断手牌是否大于上家的牌

        /// <summary>
        /// 判断手牌是否大于上家的牌
        /// </summary>
        /// <param name="myCards"></param>
        /// <param name="lastCards"></param>
        /// <param name="lastCardType"></param>
        /// <returns></returns>
        public bool isShowOutCardBtn(List<int> myCards, List<int> lastCards, DDZ_POKER_TYPE lastCardType)
        {

            // 上一首牌的个数
            int prevSize = lastCards.Count;

            int mySize = myCards.Count;

            // 我先出牌，上家没有牌
            if (prevSize == 0 && mySize != 0)
            {
                return true;
            }

            // 集中判断是否王炸，免得多次判断王炸
            if (lastCardType == DDZ_POKER_TYPE.KING_BOMB)
            {
                //上家王炸，肯定不能出。
                return false;
            }

            if (mySize >= 2)
            {
                List<int> cards = new List<int>();

                cards.Add(myCards[mySize - 1]);

                cards.Add(myCards[mySize - 2]);

                if (DDZCardRule.IsJokerBoom(cards))
                {
                    return true;
                }
            }

            // 集中判断对方不是炸弹，我出炸弹的情况
            if (lastCardType != DDZ_POKER_TYPE.FOUR_BOMB)
            {
                if (mySize > 4)
                {
                    for (int i = 0; i < mySize - 3; i++)
                    {
                        int grade0 = myCards[i];

                        int grade1 = myCards[i + 1];

                        int grade2 = myCards[i + 2];

                        int grade3 = myCards[i + 3];

                        if (grade1 == grade0 && grade2 == grade0
                                && grade3 == grade0)
                        {
                            return true;
                        }
                    }
                }

            }

            int prevGrade = lastCards[0];

            // 上家出单
            if (lastCardType == DDZ_POKER_TYPE.Single)
            {
                // 一张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 0; i--)
                {
                    int grade = myCards[i];
                    
                    if (grade > prevGrade)
                    {
                        // 只要有1张牌可以大过上家，则返回true
                        return true;
                    }
                }

            }
            // 上家出对子
            else if (lastCardType == DDZ_POKER_TYPE.TWIN)
            {
                // 2张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 1; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    if (grade0 == grade1)
                    {
                        if (grade0 > prevGrade)
                        {
                            // 只要有1对牌可以大过上家，则返回true
                            return true;
                        }
                    }
                }

            }
            // 上家出3不带
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE)
            {
                // 3张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 2; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    if (grade0 == grade1 && grade0 == grade2)
                    {
                        if (grade0 > prevGrade)
                        {
                            // 只要3张牌可以大过上家，则返回true
                            return true;
                        }
                    }
                }

            }
            // 上家出3带1
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE)
            {
                // 3带1 3不带 比较只多了一个判断条件
                if (mySize < 4)
                {
                    return false;
                }

                // 3张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 2; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    if (grade0 == grade1 && grade0 == grade2)
                    {
                        if (grade0 > lastCards[1])
                        {
                            // 只要3张牌可以大过上家，则返回true
                            return true;
                        }
                    }
                }

            }
            // 上家出3带2
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_TWIN)
            {
                // 3带1 3不带 比较只多了一个判断条件
                if (mySize < 5)
                {
                    return false;
                }

                // 3张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 2; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    if (grade0 == grade1 && grade0 == grade2)
                    {
                        if (grade0 > lastCards[2])
                        {
                            // 只要3张牌可以大过上家，则返回true 
                            myCards.RemoveAt(i);

                            myCards.RemoveAt(i - 1);

                            myCards.RemoveAt(i - 2);

                            for (int j = myCards.Count - 1; j >= 1; j--)
                            {
                                int temp0 = myCards[j];

                                int temp1 = myCards[j - 1];

                                if (temp0 == temp1)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

            }
            // 上家出炸弹
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_BOMB)
            {
                // 4张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 3; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    int grade3 = myCards[i - 3];

                    if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                    {
                        if (grade0 > prevGrade)
                        {
                            // 只要有4张牌可以大过上家，则返回true
                            return true;
                        }
                    }
                }

            }
            // 上家出4带2 
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
            {
                // 4张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 3; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    int grade3 = myCards[i - 3];

                    if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                    {
                        // 只要有炸弹，则返回true
                        return true;
                    }
                }
            }
            // 上家出顺子
            else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
            {
                if (mySize < prevSize)
                {
                    return false;
                }
                else
                {

                    Hashtable myCardsHash = SortCardUseHash(myCards);

                    if (myCardsHash.Count < prevSize)
                    {
                        Log.Debug("hash的总数小于顺子的count 肯定fales");
                        //return false;
                    }
                    List<int> myCardsHashKey = new List<int>();

                    foreach (int key in myCardsHash.Keys)
                    {
                        myCardsHashKey.Add(key);
                    }

                    myCardsHashKey.Sort();

                    for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                    {
                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize; j++)
                        {
                            cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        //是不是顺子
                        if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                        {
                            int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后

                            int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后

                            if (myGrade2 > prevGrade2)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            // 上家出连对
            else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_TWIN)
            {
                if (mySize < prevSize)
                {
                    return false;
                }
                else
                {
                    Hashtable myCardsHash = SortCardUseHash(myCards);

                    if (myCardsHash.Count < prevSize)
                    {
                        Log.Debug("hash的总数小于顺子的count 肯定fales");
                        //return false;
                    }
                    List<int> myCardsHashKey = new List<int>();

                    foreach (int key in myCardsHash.Keys)
                    {
                        myCardsHashKey.Add(key);
                    }

                    myCardsHashKey.Sort();

                    for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                    {
                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize; j++)
                        {
                            cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                        }
                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                        {
                            int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后

                            int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后

                            if (myGrade2 > prevGrade2)
                            {
                                for (int ii = 0; ii < cards.Count; ii++)
                                {
                                    if ((int)myCardsHash[cards[ii]] < 2)
                                    {
                                        //是顺子但不是双顺
                                        return false;
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            //上家出飞机
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_PURE)
            {
                if (mySize < prevSize)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i <= mySize - prevSize; i++)
                    {
                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize; j++)
                        {
                            cards.Add(myCards[i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                        {
                            int myGrade4 = cards[4];

                            int prevGrade4 = lastCards[4];

                            if (myGrade4 > prevGrade4)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            //上家出飞机带单
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_SINGLE)
            {
                if (mySize < prevSize)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i <= mySize - prevSize; i++)
                    {
                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize - (prevSize / 4); j++)
                        {
                            cards.Add(myCards[i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                        {
                            int myGrade4 = cards[4];

                            int prevGrade4 = lastCards[4];

                            if (myGrade4 > prevGrade4)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //上家出飞机带双
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_TWIN)
            {
                if (mySize < prevSize)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i <= mySize - prevSize; i++)
                    {
                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize - (prevSize / 5); j++)
                        {
                            cards.Add(myCards[i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                        {
                            int myGrade4 = cards[4];

                            int prevGrade4 = lastCards[4];

                            if (myGrade4 > prevGrade4)
                            {
                                List<int> tempTwoList = new List<int>();

                                for (int ii = 0; ii < cards.Count; ii++)
                                {
                                    int tempInt = 0;

                                    for (int j = 0; j < cards.Count; j++)
                                    {

                                        if (cards[ii] == cards[j])
                                        {
                                            tempInt++;
                                        }

                                    }
                                    if (tempInt == 2)
                                    {
                                        tempTwoList.Add(cards[ii]);
                                    }

                                }
                                if (tempTwoList.Count / 2 < prevSize / 5)
                                {

                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            // 默认不能出牌
            return false;
        }


        #endregion

        #region 提示出牌
        public Dictionary<int, List<int>> FindPromptCards(List<int> myCards, List<int> lastCards, DDZ_POKER_TYPE lastCardType)
        {
            Dictionary<int, List<int>> PromptCards = new Dictionary<int, List<int>>();

            Hashtable tempMyCardHash = SortCardUseHash1(myCards);

            // 上一首牌的个数
            int prevSize = lastCards.Count;

            int mySize = myCards.Count;

            // 我先出牌，上家没有牌
            if (prevSize == 0 && mySize != 0)
            {
                //把所有牌权重存入返回
                //上家没有牌"
                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    List<int> tempIntList = new List<int>();

                    tempIntList.Add(myCardsHashKey[i]);

                    PromptCards.Add(i, tempIntList);

                }
            }

            // 集中判断是否王炸，免得多次判断王炸
            if (lastCardType == DDZ_POKER_TYPE.KING_BOMB)
            {
                //上家王炸，肯定不能出。

            }
            int prevGrade = 0;

            if (prevSize > 0)
            {
                prevGrade = lastCards[0];
            }

            // 上家出单
            if (lastCardType == DDZ_POKER_TYPE.Single)
            {
                int tempCount = 0;

                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if (myCardsHashKey[i] > prevGrade)
                    {
                        List<int> tempIntList = new List<int>();

                        tempIntList.Add(myCardsHashKey[i]);

                        PromptCards.Add(tempCount, tempIntList);

                        tempCount++;
                    }


                }


            }
            // 上家出对子
            else if (lastCardType == DDZ_POKER_TYPE.TWIN)
            {
                int tempCount = 0;

                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 2)
                    {
                        List<int> tempIntList = new List<int>();

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        PromptCards.Add(tempCount, tempIntList);

                        tempCount++;
                    }


                }

            }
            // 上家出3不带
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE)
            {
                int tempCount = 0;

                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 3)
                    {
                        List<int> tempIntList = new List<int>();

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        PromptCards.Add(tempCount, tempIntList);

                        tempCount++;
                    }


                }

            }
            // 上家出3带1
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE)
            {
                // 3带1 3不带 比较只多了一个判断条件
                if (mySize < 4)
                {

                }
                int grade3 = 0;

                foreach (int key in tempMyCardHash.Keys)
                {
                    if (int.Parse(tempMyCardHash[key].ToString()) == 1)
                    {
                        grade3 = key;

                        break;
                    }
                }
                int tempCount = 0;

                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 3)
                    {
                        List<int> tempIntList = new List<int>();

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(grade3);

                        PromptCards.Add(tempCount, tempIntList);

                        tempCount++;
                    }
                }

            }
            // 上家出3带2
            else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_TWIN)
            {
                // 3带1 3不带 比较只多了一个判断条件
                if (mySize < 5)
                {

                }
                int grade3 = 0;

                int grade4 = 0;

                foreach (int key in tempMyCardHash.Keys)
                {
                    if (int.Parse(tempMyCardHash[key].ToString()) == 2)
                    {
                        grade3 = key;

                        grade4 = key;

                        break;
                    }
                }

                int tempCount = 0;

                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 3)
                    {
                        List<int> tempIntList = new List<int>();

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(grade3);

                        tempIntList.Add(grade4);

                        PromptCards.Add(tempCount, tempIntList);

                        tempCount++;
                    }


                }

            }
            // 上家出炸弹
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_BOMB)
            {
                int tempCount = 0;
                // 4张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 3; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    int grade3 = myCards[i - 3];

                    if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                    {
                        if (grade0 > prevGrade)
                        {
                            // 把四张牌存进去
                            List<int> tempIntList = new List<int>();

                            tempIntList.Add(grade0);

                            tempIntList.Add(grade1);

                            tempIntList.Add(grade2);

                            tempIntList.Add(grade3);

                            PromptCards.Add(tempCount, tempIntList);

                            tempCount++;
                        }
                    }
                }

            }
            // 上家出4带2 
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
            {
                // 4张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 3; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    int grade3 = myCards[i - 3];

                    if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                    {
                        // 只要有炸弹，则返回true

                    }
                }
            }
            // 上家出4带2 对子
            else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
            {
                // 4张牌可以大过上家的牌
                for (int i = mySize - 1; i >= 3; i--)
                {
                    int grade0 = myCards[i];

                    int grade1 = myCards[i - 1];

                    int grade2 = myCards[i - 2];

                    int grade3 = myCards[i - 3];

                    if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                    {
                        // 只要有炸弹，则返回true

                    }
                }
            }
            // 上家出顺子
            else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
            {
                if (mySize < prevSize)
                {

                }
                else
                {
                    List<int> tempMyCards = new List<int>();

                    tempMyCards = myCards;

                    Hashtable myCardsHash = SortCardUseHash(tempMyCards);

                    if (myCardsHash.Count < prevSize)
                    {
                        Log.Debug("hash的总数小于顺子的count 肯定fales 1287");

                    }
                    List<int> myCardsHashKey = new List<int>();

                    foreach (int key in myCardsHash.Keys)
                    {
                        myCardsHashKey.Add(key);
                    }

                    myCardsHashKey.Sort();

                    int tempCount = 0;

                    for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                    {
                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize; j++)
                        {
                            cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                        {
                            int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后

                            int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后

                            if (myGrade2 > prevGrade2)
                            {
                                //存进去PromptCards
                                PromptCards.Add(tempCount, cards);

                                tempCount++;
                            }
                        }
                    }

                }

            }
            // 上家出连对
            else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_TWIN)
            {
                if (mySize < prevSize)
                {

                }
                else
                {
                    List<int> tempMyCards = new List<int>();

                    tempMyCards = myCards;

                    Hashtable myCardsHash = SortCardUseHash(tempMyCards);

                    if (myCardsHash.Count < prevSize)
                    {
                        Log.Debug("hash的总数小于顺子的count 肯定fales 1350");

                    }
                    List<int> myCardsHashKey = new List<int>();

                    foreach (int key in myCardsHash.Keys)
                    {
                        myCardsHashKey.Add(key);
                    }

                    myCardsHashKey.Sort();

                    int tempCount = 0;

                    for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                    {

                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize; j++)
                        {
                            cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                        {
                            int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后

                            int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后

                            if (myGrade2 > prevGrade2)
                            {
                                for (int ii = 0; ii < cards.Count; ii++)
                                {
                                    if ((int)myCardsHash[cards[ii]] < 2)
                                    {
                                        //是顺子但不是双顺
                                        return PromptCards;
                                    }
                                    else
                                    {
                                        for (int iii = 0; iii < cards.Count; iii++)
                                        {
                                            cards.Add(cards[iii]);
                                        }
                                        //存进去PromptCards
                                        PromptCards.Add(tempCount, cards);

                                        tempCount++;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            //上家出飞机
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_PURE)
            {
                if (mySize < prevSize)
                {

                }
                else
                {
                    int tempCount = 0;

                    for (int i = 0; i <= mySize - prevSize; i++)
                    {

                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize; j++)
                        {
                            cards.Add(myCards[i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                        {
                            int myGrade4 = cards[4];

                            int prevGrade4 = lastCards[4];

                            if (myGrade4 > prevGrade4)
                            {
                                //存进去PromptCards
                                PromptCards.Add(tempCount, cards);

                                tempCount++;
                            }
                        }
                    }
                }
            }
            //上家出飞机带单
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_SINGLE)
            {
                if (mySize < prevSize)
                {

                }
                else
                {
                    int tempCount = 0;

                    for (int i = 0; i <= mySize - prevSize; i++)
                    {

                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize - prevSize / 4; j++)
                        {
                            cards.Add(myCards[i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                        {
                            int myGrade4 = cards[4];

                            int prevGrade4 = lastCards[4];

                            if (myGrade4 > prevGrade4)
                            {
                                int ii = 0;
                                //存进去PromptCards 然后再找一个最小的两个单
                                foreach (int key in tempMyCardHash.Keys)
                                {
                                    if (int.Parse(tempMyCardHash[key].ToString()) == 1)
                                    {
                                        cards.Add(key);

                                        ii++;

                                        if (ii == prevSize / 4)
                                        {
                                            break;
                                        }
                                    }
                                }

                                PromptCards.Add(tempCount, cards);

                                tempCount++;
                            }
                        }
                    }
                }
            }

            //上家出飞机带双
            else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_TWIN)
            {
                if (mySize < prevSize)
                {

                }
                else
                {
                    int tempCount = 0;

                    for (int i = 0; i <= mySize - prevSize; i++)
                    {

                        List<int> cards = new List<int>();

                        for (int j = 0; j < prevSize - prevSize / 5; j++)
                        {
                            cards.Add(myCards[i + j]);
                        }

                        DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;

                        bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                        if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                        {
                            int myGrade4 = cards[4];
                            int prevGrade4 = lastCards[4];

                            if (myGrade4 > prevGrade4)
                            {
                                List<int> tempTwoList = new List<int>();

                                for (int ii = 0; ii < cards.Count; ii++)
                                {
                                    int tempInt = 0;

                                    for (int j = 0; j < cards.Count; j++)
                                    {

                                        if (cards[ii] == cards[j])
                                        {
                                            tempInt++;
                                        }

                                    }
                                    if (tempInt == 2)
                                    {
                                        tempTwoList.Add(cards[ii]);
                                    }

                                }
                                if (tempTwoList.Count / 2 < prevSize / 5)
                                {


                                }
                                else
                                {
                                    //存进去
                                    int iii = 0;
                                    //存进去PromptCards 然后再找一个最小的两个单
                                    foreach (int key in tempMyCardHash.Keys)
                                    {
                                        if (int.Parse(tempMyCardHash[key].ToString()) == 2)
                                        {
                                            cards.Add(key);

                                            cards.Add(key);

                                            iii++;

                                            if (iii == prevSize / 5)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    PromptCards.Add(tempCount, cards);

                                    tempCount++;
                                }
                            }
                        }
                    }
                }
            }




            // 集中判断对方不是炸弹，我出炸弹的情况
            if (lastCardType != DDZ_POKER_TYPE.FOUR_BOMB)
            {

                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }

                myCardsHashKey.Sort();

                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if ((int)tempMyCardHash[myCardsHashKey[i]] == 4)
                    {
                        List<int> tempIntList = new List<int>();

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        tempIntList.Add(myCardsHashKey[i]);

                        PromptCards.Add(PromptCards.Count, tempIntList);

                    }


                }

            }
            if (mySize >= 2)
            {
                List<int> myCardsHashKey = new List<int>();

                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                if (myCardsHashKey.Contains(16) && myCardsHashKey.Contains(17))//修改:原来是53,54
                {
                    List<int> tempIntList = new List<int>();

                    tempIntList.Add(16);

                    tempIntList.Add(17);

                    PromptCards.Add(PromptCards.Count, tempIntList);
                }
            }

            return PromptCards;
        }

        #endregion

        #region 帮助方法
        private bool compareGrade(int grade1, int grade2)
        {
            return grade1 > grade2;
        }

        /// <summary>
        /// 使用哈希去存所有的牌
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public Hashtable SortCardUseHash(List<int> cards)
        {
            Hashtable temp = new Hashtable();

            List<int> tempJoker = new List<int>();

            //不存小王，大王，2
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] == 16)//zy:53
                {
                    cards.RemoveAt(i);
                }
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] == 17)//zy:54
                {
                    cards.RemoveAt(i);
                }
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] == 15)//zy:13
                {
                    cards.Remove(i);
                }
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (temp.Contains(cards[i]))
                {
                    temp[cards[i]] = (int)temp[cards[i]] + 1;
                }
                else
                {
                    temp.Add(cards[i], 1);
                }
            }

            return temp;
        }

        public Hashtable SortCardUseHash1(List<int> cards)
        {
            Hashtable temp = new Hashtable();

            for (int i = 0; i < cards.Count; i++)
            {
                if (temp.Contains(cards[i]))
                {
                    temp[cards[i]] = (int)temp[cards[i]] + 1;
                }
                else
                {
                    temp.Add(cards[i], 1);
                }
            }

            return temp;
        }
        #endregion
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }

}
