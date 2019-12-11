using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZPokerCompareCptAwakeSystem : AwakeSystem<DDZPokerCompareCpt>
    {
        public override void Awake(DDZPokerCompareCpt self)
        {
            self.Awake();
        }
    }
    public class DDZCardType
    {
        public enum DDZ_POKER_TYPE
        {
            DDZ_PASS = 0,                 
            Single = 1,                      
            TWIN = 2,                    
            TRIPLE = 3,                   
            TRIPLE_WITH_SINGLE = 4,       
            TRIPLE_WITH_TWIN = 5,          
            STRAIGHT_SINGLE = 6,          
            STRAIGHT_TWIN = 7,          
            PLANE_PURE = 8,              
            PLANE_WITH_SINGLE = 9,       
            PLANE_WITH_TWIN = 10,      
            FOUR_WITH_SINGLE = 11,       
            FOUR_WITH_TWIN = 12,          
            FOUR_BOMB = 13,            
            KING_BOMB = 14,              
        }
    }
    
    public class DDZCompareCard
    {
        public List<byte> pokers;
        public long userId;
        public DDZCardType.DDZ_POKER_TYPE POKER_TYPE;
    }
    
    public class DDZPokerCompareCpt : Component
    {
        public DDZCompareCard currCard;
        public int bombNum;
        
        public void Awake()
        {
            currCard = new DDZCompareCard();
            currCard.pokers = new List<byte>();
            bombNum = 0;
        }
        
        public void RestCurrCards()
        {
            currCard.pokers.Clear();
            currCard.POKER_TYPE = DDZCardType.DDZ_POKER_TYPE.DDZ_PASS;
            currCard.userId = 0;
        }

        public void RestGame()
        {
            bombNum = 0;
        }

        
        public bool CompareCards(List<byte> myCards,long userId, List< DDZCompareCard> lastCardsList)
        {
            DDZCompareCard lastCards = new DDZCompareCard(); lastCards.pokers = new List<byte>();
            
            if(lastCardsList.Count>0)
            {
                for (int i = lastCardsList.Count-1; i > -1; i--)
                {
                    if (lastCardsList[i].POKER_TYPE == DDZCardType.DDZ_POKER_TYPE.DDZ_PASS && lastCardsList[i].pokers.Count == 0) continue;
                   
                    lastCards.POKER_TYPE = lastCardsList[i].POKER_TYPE; lastCardsList[i].pokers.ForEach(u => lastCards.pokers.Add(u)); lastCards.userId = lastCardsList[i].userId;
                    
                    break;
                }
            }
            DDZCardType.DDZ_POKER_TYPE myCardType = DDZCardType.DDZ_POKER_TYPE.DDZ_PASS;
            
            bool IsPopEnable = PopEnable(myCards, out myCardType);

            if (!IsPopEnable) return false;
            
            if (lastCards.pokers.Count == 0 && lastCards.POKER_TYPE == DDZCardType.DDZ_POKER_TYPE.DDZ_PASS)
            {
                if (myCardType == DDZCardType.DDZ_POKER_TYPE.KING_BOMB||myCardType==DDZCardType.DDZ_POKER_TYPE.FOUR_BOMB)
                {
                    bombNum++;
                }

                ChangeCard(myCards, userId, myCardType);
                return true;
            }
            else  
            {
                if (lastCards.POKER_TYPE == DDZCardType.DDZ_POKER_TYPE.KING_BOMB)
                {
                    return false;
                }
                else if (myCardType == DDZCardType.DDZ_POKER_TYPE.KING_BOMB)
                {
                    bombNum++;
                    ChangeCard(myCards, userId, myCardType);
                    return true;
                }
                if (lastCards.POKER_TYPE != DDZCardType.DDZ_POKER_TYPE.FOUR_BOMB && myCardType == DDZCardType.DDZ_POKER_TYPE.FOUR_BOMB)
                {
                    bombNum++;
                    ChangeCard(myCards, userId, myCardType);
                    return true;
                }
                
                if (myCards.Count != lastCards.pokers.Count) return false;

                if (myCardType != lastCards.POKER_TYPE) return false;
                
                var pokerNum0 = PokerCardsHelper.GetPokerNum(lastCards.pokers[0]);
                var pokerNum1 = PokerCardsHelper.GetPokerNum(myCards[0]);

                if (pokerNum1 == 2 && pokerNum0 <= 14 &&pokerNum0!=2)
                {
                    if (myCardType == DDZCardType.DDZ_POKER_TYPE.FOUR_BOMB)
                    {
                        bombNum++;
                    }
                    ChangeCard(myCards, userId, myCardType);
                    return true;
                }
                else if (pokerNum0 == 2 && pokerNum1 <= 14) return false;
                if (pokerNum1 > pokerNum0)
                {
                    if (myCardType == DDZCardType.DDZ_POKER_TYPE.FOUR_BOMB)
                    {
                        bombNum++;
                    }
                    ChangeCard(myCards, userId, myCardType);
                    return true;
                }
            }
            
            return false;
        }
        
        List<int> GetPokerNum(List<byte> cards)
        {
            List<int> num = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                num.Add(PokerCardsHelper.GetPokerNum(cards[i]));
            }
            return num;
        }
        
        public void ChangeCard(List<byte> myCards, long userId, DDZCardType.DDZ_POKER_TYPE type)
        {
            currCard.userId = userId; myCards.ForEach(u => currCard.pokers.Add(u)); currCard.POKER_TYPE = type;
        }
        
        public List<byte> PaiXingPaiXu(List<byte> cards)
        {
            List<int> num = new List<int>();
            cards = PokerCardsHelper.QuickSort(cards.ToArray(), 0, cards.Count - 1).ToList();
            num = GetPokerNum(cards);
            switch (cards.Count)
            {
                case 4:
                    if (num[1] == num[2] && num[2] == num[3])
                    {
                        cards.Add(cards[0]); cards.RemoveAt(0);
                    }
                    break;
                case 5:
                    if (num[0] == num[1] && num[1] != num[2] && num[2] == num[3] && num[3] == num[4])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]);
                        cards = RemoveAtFirst(2, cards);
                    }
                    break;
                case 6:
                    if (num[2] == num[3] && num[3] == num[4] && num[4] == num[5])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]);
                        
                        cards = RemoveAtFirst(2, cards);
                    }
                    else if (num[1] == num[2] && num[2] == num[3] && num[3] == num[4])
                    {
                        cards.Add(cards[0]); cards.RemoveAt(0);
                    }
                    break;
                case 8:
                    if (num[1] != num[2] && num[2] == num[3] && num[3] == num[4] && num[4] + 1 == num[5] && num[5] == num[6] && num[6] == num[7])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]);
                        
                        cards = RemoveAtFirst(2, cards);
                    }
                   
                    else if (num[0] != num[1] && num[1] == num[2] && num[2] == num[3] && num[3] + 1 == num[4] && num[4] == num[5] && num[5] == num[6])
                    {
                        cards.Add(cards[0]); cards.RemoveAt(0);
                    }
                    else if (num[0]==num[1]&&num[1]!=num[2]&&num[2]==num[3]&&num[3]==num[4]&&num[4]==num[5]&&num[5]!=num[6]&&num[6]==num[7])
                    {
                        cards.Add(cards[0]);cards.Add(cards[1]);
                        cards = RemoveAtFirst(2, cards);
                    }
                    else if (num[0]==num[1]&&num[1]!=num[2]&&num[2]==num[3]&&num[3]!=num[4]&&num[4]==num[5]&&num[5]==num[6]&&num[6]==num[7])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]); cards.Add(cards[3]);
                        
                        cards = RemoveAtFirst(4, cards);
                    }
                    break;
                case 10:
                    if (num[4] == num[5] && num[5] == num[6] && num[6] + 1 == num[7] && num[7] == num[8] && num[8] == num[9])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]); cards.Add(3);
                        cards = RemoveAtFirst(4, cards);
                    }
                    break;
                case 12:
                    if (num[3] == num[4] && num[4] == num[5] && num[5] + 1 == num[6] && num[6] == num[7] && num[7] == num[8] && num[8] + 1 == num[9] && num[9] == num[10] && num[10] == num[11])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]);
                        cards = RemoveAtFirst(3, cards);
                    }
                    break;
                case 15:
                    if (num[6] == num[7] && num[7] == num[8] && num[8] + 1 == num[9] && num[9] == num[10] && num[10] == num[11] && num[11] + 1 == num[12] && num[12] == num[13] && num[13] == num[14])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]); cards.Add(cards[3]); cards.Add(cards[4]); cards.Add(cards[5]);
                        cards = RemoveAtFirst(6, cards);
                    }
                    else if (num[4] == num[5] && num[5] == num[6] && num[6] + 1 == num[7] && num[7] == num[8] && num[8] == num[9] && num[9] + 1 == num[10] && num[10] == num[11] && num[11] == num[12])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]); cards.Add(cards[3]);
                        cards = RemoveAtFirst(4, cards);
                    }
                    else if (num[2] == num[3] && num[3] == num[4] && num[4] + 1 == num[5] && num[5] == num[6] && num[6] == num[7] && num[7] + 1 == num[8] && num[8] == num[9] && num[9] == num[10])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]);
                        cards = RemoveAtFirst(2, cards);
                    }
                    break;
                case 16:
                    if (num[4] == num[5] && num[5] == num[6] && num[6] + 1 == num[7] && num[7] == num[8] && num[8] == num[9] && num[9] + 1 == num[10] && num[10] == num[11] && num[11] == num[12] && num[12] + 1 == num[13] && num[13] == num[14] && num[14] == num[15])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]); cards.Add(cards[3]);
                        cards = RemoveAtFirst(4, cards);
                    }
                    else if (num[3] == num[4] && num[4] == num[5] && num[5] + 1 == num[6] && num[6] == num[7] && num[7] == num[8] && num[8] + 1 == num[9] && num[9] == num[10] && num[10] == num[11] && num[11] + 1 == num[12] && num[12] == num[13] && num[13] == num[14])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]); cards.Add(cards[2]);
                        cards = RemoveAtFirst(3, cards);
                    }
                    else if (num[2] == num[3] && num[3] == num[4] && num[4] + 1 == num[5] && num[5] == num[6] && num[6] == num[7] && num[7] + 1 == num[8] && num[8] == num[9] && num[9] == num[10] && num[10] + 1 == num[11] && num[11] == num[12] && num[12] == num[13])
                    {
                        cards.Add(cards[0]); cards.Add(cards[1]);
                        cards = RemoveAtFirst(2, cards);
                    }
                    else if (num[1] == num[2] && num[2] == num[3] && num[3] + 1 == num[4] && num[4] == num[5] && num[5] == num[6] && num[6] + 1 == num[7] && num[7] == num[8] && num[8] == num[9] && num[9] + 1 == num[10] && num[10] == num[11] && num[11] == num[12])
                    {
                        cards.Add(cards[0]);
                        cards.RemoveAt(0);
                    }
                    break;


                default:
                    break;
            }


            return cards;
        }

        List<byte> ChangeCards(List<int> num, List<byte> cards)
        {
            int three = 0, index = 0;
            if (!HaveFour(cards))
            {
                List<byte> tempThreeList = new List<byte>();
                for (int i = 0; i < cards.Count; i++)
                {
                    int tempInt = 0;
                    for (int j = 0; j < cards.Count; j++)
                    {

                        if (num[i] == num[j])
                        {
                            tempInt++;
                        }

                    }
                    if (tempInt == 3)
                    {
                        tempThreeList.Add(cards[i]);
                    }
                }
            }


            return cards;
        }


        List<byte> RemoveAtFirst(int count,List<byte> cards)
        {
            List<int> num = new List<int>();
            for (int i = 0; i < count; i++)
            {
                cards.RemoveAt(0);
                num = GetPokerNum(cards);
            }

            return cards;
        }

        public bool PopEnable(List<byte> cards, out DDZCardType.DDZ_POKER_TYPE type)
        {
            type = DDZCardType.DDZ_POKER_TYPE.DDZ_PASS;
            bool isRule = false;
            switch (cards.Count)
            {
                case 1:
                    isRule = true;
                    type = DDZCardType.DDZ_POKER_TYPE.Single;
                    break;
                case 2:
                    if (IsDouble(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.TWIN;
                    }
                    else if (IsJokerBoom(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.KING_BOMB;
                    }
                    break;
                case 3:
                    if (IsOnlyThree(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.TRIPLE;
                    }
                    break;
                case 4:
                    if (IsBoom(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.FOUR_BOMB;
                    }
                    else if (IsThreeAndOne(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE;
                    }

                    break;
                case 5:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsThreeAndTwo(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.TRIPLE_WITH_TWIN;
                    }
                    break;
                case 6:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (IsSiDaiEr(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.FOUR_WITH_SINGLE;  
                    }
                    break;
                case 7:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    break;
                case 8:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_SINGLE;   
                    }
                    else if (IsSiDaiDoubleDui(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.FOUR_WITH_TWIN;  
                    }
                    break;
                case 9:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    break;
                case 10:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithTwin(cards))       
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                    }
                    break;

                case 11:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    break;
                case 12:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_SINGLE;  
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    break;
                case 13:
                    break;
                case 14:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    break;
                case 15:
                    if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    else if (isPlaneWithTwin(cards))     
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                    }
                    break;
                case 16:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_SINGLE;  
                    }
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_PURE;
                    }
                    break;
                case 19:
                    break;

                case 20:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.STRAIGHT_TWIN;
                    }
                    else if (isPlaneWithSingle(cards))
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_SINGLE;  
                    }
                    else if (isPlaneWithTwin(cards))         
                    {
                        isRule = true;
                        type = DDZCardType.DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                    }
                    break;
                default:
                    break;
            }

            return isRule;
        }
        
        public bool IsSingle(List<byte> cards)
        {
            if (cards.Count == 1)
                return true;
            else
                return false;
        }
        
        public bool IsDouble(List<byte> cards)
        {
            if (cards.Count == 2)
            {
                List<int> num = GetPokerNum(cards);
                if (num[0] == num[1])
                    return true;
            }

            return false;
        }
        
        public bool IsStraight(List<byte> cards)
        {
            if (cards.Count < 5 || cards.Count > 12)
                return false;

            List<int> num = GetPokerNum(cards);
            for (int i = 0; i < num.Count - 1; i++)
            {
                int w = num[i];
                if (num[i + 1] - w != 1)
                    return false;
                
                if (w > 14 || num[i + 1] > 14)
                    return false;
            }

            return true;
        }
        
        public bool IsDoubleStraight(List<byte> cards)
        {
            if (cards.Count < 6 || cards.Count % 2 != 0)
                return false;

            List<int> num = GetPokerNum(cards);
            for (int i = 0; i < num.Count; i += 2)
            {
                if (num[i + 1] != num[i])
                    return false;

                if (i < cards.Count - 2)
                {
                    if (num[i + 2] - num[i] != 1)
                        return false;
                    
                    if (num[i] > 14 || num[i + 2] > 14)
                        return false;
                }
            }

            return true;
        }
        
        public bool IsOnlyThree(List<byte> cards)
        {
            if (cards.Count % 3 != 0)
                return false;
            List<int> num = GetPokerNum(cards);
            if (num[0] != num[1])
                return false;
            if (num[1] != num[2])
                return false;
            if (num[0] != num[2])
                return false;

            return true;
        }
        
        public bool IsThreeAndOne(List<byte> cards)
        {
            if (cards.Count != 4)
                return false;
            List<int> num = GetPokerNum(cards);
            if (num[0] == num[1] &&
                num[1] == num[2])
                return true;
            else if (num[1] == num[2] &&
                num[2] == num[3])
                return true;
            return false;
        }
        
        public bool IsThreeAndTwo(List<byte> cards)
        {
            if (cards.Count != 5)
                return false;

            List<int> num = GetPokerNum(cards);
            if (num[0] == num[1] &&
                num[1] == num[2])
            {
                if (num[3] == num[4])
                    return true;
            }

            else if (num[2] == num[3] &&
                num[3] == num[4])
            {
                if (num[0] == num[1])
                    return true;
            }

            return false;
        }
        
        public bool IsBoom(List<byte> cards)
        {
            if (cards.Count != 4)
                return false;

            List<int> num = GetPokerNum(cards);
            if (num[0] != num[1])
                return false;
            if (num[1] != num[2])
                return false;
            if (num[2] != num[3])
                return false;

            return true;
        }
        
        public bool IsJokerBoom(List<byte> cards)
        {
            if (cards.Count != 2)
                return false;
            List<int> num = GetPokerNum(cards);
            if (num[0] == 15)
            {
                if (num[1] == 16)
                    return true;
                return false;
            }
            else if (num[0] == 16)
            {
                if (num[1] == 15)
                    return true;
                return false;
            }

            return false;
        }
        
        public bool IsTripleStraight(List<byte> cards)
        {
            if (cards.Count < 6 || cards.Count % 3 != 0)
                return false;

            List<int> num = GetPokerNum(cards);
            for (int i = 0; i < cards.Count; i += 3)
            {
                if (num[i + 1] != num[i])
                    return false;
                if (num[i + 2] != num[i])
                    return false;
                if (num[i + 1] != num[i + 2])
                    return false;

                if (i < cards.Count - 3)
                {
                    if (num[i + 3] - num[i] != 1)
                        return false;

                    if (num[i] == 2) return false;
                    
                    if (num[i] > 12 && num[i + 3] > 14)
                        return false;
                }
            }

            return true;
        }
        
        public bool isPlaneWithSingle(List<byte> cards)
        {
            if (!HaveFour(cards))
            {
                List<byte> tempThreeList = new List<byte>();
                List<int> num = GetPokerNum(cards);

                if (num.Contains(15) && num.Contains(16)) return false;

                for (int i = 0; i < cards.Count; i++)
                {
                    int tempInt = 0;
                    for (int j = 0; j < cards.Count; j++)
                    {

                        if (num[i] == num[j])
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
        
        public bool isPlaneWithTwin(List<byte> cards)
        {
            if (!HaveFour(cards))
            {
                List<byte> tempThreeList = new List<byte>();
                List<byte> tempTwoList = new List<byte>();
                List<int> num = GetPokerNum(cards);
                for (int i = 0; i < cards.Count; i++)
                {
                    int tempInt = 0;
                    for (int j = 0; j < cards.Count; j++)
                    {

                        if (num[i] == num[j])
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
        
        public bool HaveFour(List<byte> cards)
        {
            List<int> num = GetPokerNum(cards);
            for (int i = 0; i < cards.Count; i++)
            {
                int tempInt = 0;
                for (int j = 0; j < cards.Count; j++)
                {

                    if (num[i] == num[j])
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
        
        public bool IsAllDouble(List<byte> cards)
        {
            List<int> num = GetPokerNum(cards);
            for (int i = 0; i < cards.Count % 2; i += 2)
            {
                if (num[i] != num[i + 1])
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool IsSiDaiEr(List<byte> cards)
        {
            bool flag = false;

            List<int> num = GetPokerNum(cards);

            if (num.Contains(15) && num.Contains(16)) return false;

            List<int> temp = new List<int>();

            if (cards != null && cards.Count == 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    int grade1 = num[i];
                    int grade2 = num[i + 1];
                    int grade3 = num[i + 2];
                    int grade4 = num[i + 3];
                    
                    if (grade2 == grade1 && grade3 == grade1 && grade4 == grade1)
                    {
                        flag = true;
                    }

                   
                }
            }
            
            return flag;
        }
        
        public bool IsSiDaiDoubleDui(List<byte> cards)
        {
            bool flag = false;
            List<int> num = GetPokerNum(cards);
            
            List<int> temp = new List<int>();
            
            if (cards != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    int grade1 = num[i];
                    int grade2 = num[i + 1];
                    int grade3 = num[i + 2];
                    int grade4 = num[i + 3];

                    if (grade2 == grade1 && grade3 == grade1 && grade4 == grade1)
                    {
                        temp.Add(num[i]); temp.Add(num[i+1]);temp.Add(num[i + 2]);temp.Add(num[i + 3]);
                        break;
                    }
                }

                if (temp.Count == 4)
                {
                    temp.ForEach(u => num.Remove(u));
                    
                    if (num.Count == 4)
                    {
                        if (num[0] == num[1] && num[2] == num[3]&& num[1]!= num[2])
                        {
                            flag = true;
                        }
                    }
                }
            }
            return flag;
        }
        
    }
}