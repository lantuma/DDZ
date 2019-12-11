using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;
using Google.Protobuf;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZDistributeCptAwakeSystem : AwakeSystem<DDZDistributeCpt>
    {
        public override void Awake(DDZDistributeCpt self)
        {
            self.Awake();
        }
    }
    public class DDZDistributeCpt : Component
    {
        private List<byte> CurrentPokers;
        public List<byte> player1Pokers;
        public List<byte> player2Pokers;
        public List<byte> player3Pokers;
        public List<byte> dpPokers;
        
        public void Awake()
        {
            player1Pokers = new List<byte>(); player2Pokers = new List<byte>();
            player3Pokers = new List<byte>(); dpPokers = new List<byte>();
            CurrentPokers = new List<byte>();
        }
       
        public void RestGame()
        {
            CurrentPokers.Clear(); player1Pokers.Clear();

            player2Pokers.Clear(); player3Pokers.Clear();

            dpPokers.Clear();
        }
        
        public void ResetCurrentCards()
        {
            CurrentPokers = PokerCardsHelper.PokerArray.ToList();
        }
        
        public void DealCards()
        {
            ResetCurrentCards();

            for (int i = 0; i < 3; i++)
            {
                byte card = new byte();
                for (int j = 0; j < 17; j++)
                {
                    card = PokerCardsHelper.TackOneCard(CurrentPokers, true);
                    if (i == 0) player1Pokers.Add(card);
                    else if (i == 1) player2Pokers.Add(card);
                    else if (i == 2) player3Pokers.Add(card);

                    CurrentPokers.Remove(card);
                }
                card = PokerCardsHelper.TackOneCard(CurrentPokers, true);
                dpPokers.Add(card);
                CurrentPokers.Remove(card);
            }

            for (int i = 0; i < 4; i++)
            {
                if (i == 0) dpPokers = QuickSort(dpPokers);
                else if (i == 1) player1Pokers = QuickSort(player1Pokers);
                else if (i == 2) player2Pokers = QuickSort(player2Pokers); 
                else if (i == 3) player3Pokers = QuickSort(player3Pokers);
            }
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

        public List<byte> QuickSort(List<byte> card)
        {
            if (card.Count == 0) return card;
            card = PokerCardsHelper.QuickSort(card.ToArray(), 0, card.Count - 1, true).ToList();
            card = HaveTwoQuickSort(card);
            
            return card;
        }
        
        List<byte> HaveTwoQuickSort(List<byte> card)
        {
            if (card.Count == 0) return card;

            List<int> num = GetPokerNum(card);

            if (!num.Contains(2)) return card;

            int index = num.IndexOf(num.Max());

            if (num.Max() == 16 && num[1] == 15)
            {
                index = 2;
            }
            else if (num.Max() == 16 || num.Max() == 15)
            {
                index = 1;
            }

            for (int i = 0; i < 4; i++)
            {
                if (num.Last() != 2) return card;

                card.Insert(index, card[card.Count - 1]);
                num.Insert(index, num[num.Count - 1]);
                num.RemoveAt(card.Count - 1);
                card.RemoveAt(card.Count - 1);
            }

            return card;
        }
    }
}
