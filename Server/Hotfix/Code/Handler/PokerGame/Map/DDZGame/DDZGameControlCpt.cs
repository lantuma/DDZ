using ETModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZGameControlCptAwakeSystem : AwakeSystem<DDZGameControlCpt>
    {
        public override void Awake(DDZGameControlCpt self)
        {
            self.Awake();
        }
    }
    public class DDZGameControlCpt : Component
    {
        public DDZPlayerData[] gamePlayrMap;
        public DDZRoomBehaiorCpt roomBehaiorCpt;
        public long operateId { get; set; }
        public int opIndex { get; set; }
        public DDZCompareCard lastPlayerCards;
        public int[] surCardsNum;
        public List<DDZCompareCard> playLastCircleCards;
        public int gameStatus { get; set; }
     
        public void Awake()
        {
            roomBehaiorCpt = GetParent<DDZRoomBehaiorCpt>();
            playLastCircleCards = new List<DDZCompareCard>();
            lastPlayerCards = new DDZCompareCard();
            lastPlayerCards.pokers = new List<byte>();
            surCardsNum = new int[3];
            gamePlayrMap = new DDZPlayerData[3];
        }
        
        public void RestGame()
        {
            lastPlayerCards.pokers.Clear();
            
            gameStatus = 0;    playLastCircleCards.Clear();

            Array.Clear(surCardsNum, 0, surCardsNum.Length);

            if (gamePlayrMap[0] != null) Array.Clear(gamePlayrMap, 0, gamePlayrMap.Length);
        }
        
        public bool AddGamePlayer()
        {
            var seatPlayer = GetParent<DDZRoomBehaiorCpt>().parent.SeatPlayer;

            for (int i = 0; i < seatPlayer.Length; i++)
            {
             
                if (seatPlayer[i] == 0) continue;

                var playerId = seatPlayer[i];
               
                gamePlayrMap[i] = roomBehaiorCpt.GetPlayerData1(playerId);
            }

            if (gamePlayrMap.Length < 3) return true;

            return false;
        }

        public int IsPlayerExist(int playerId)
        {
            for (int i = 0; i < gamePlayrMap.Length; i++)
            {
                if (gamePlayrMap[i].userInfo.PlayerId != playerId) continue;
                return i;
            }
            return -1;
        }
        
        public int GetNextPlayer()
        {
            if (opIndex + 1 > gamePlayrMap.Length - 1)
                opIndex = -1;
            
            opIndex++;

            operateId = gamePlayrMap[opIndex].userId;

            return opIndex;
        }
        
        public int GetNextIndex(int index)
        {
            int tempIndex = -1;

            while (true)
            {
                index++;
                if (index > gamePlayrMap.Length - 1) index = 0;
                tempIndex = index;
                break;
            }

            return tempIndex;
        }

        public DDZPlayerData GetNextInfo(int index)
        {
            DDZPlayerData data = new DDZPlayerData();

            for (int i = 0; i < gamePlayrMap.Length; i++)
            {
                if (gamePlayrMap[i].Chairld==index)
                {
                    data = gamePlayrMap[i];
                }
            }

            return data;
        }
        
        public void NotifyTrunPlayer(int liftTime)
        {
            Actor_DDZTurnPlayer_Ntt trunMessgae = new Actor_DDZTurnPlayer_Ntt()
            {
                LeftTime = liftTime,
                OperateId = opIndex,
                RoomId = roomBehaiorCpt.parent.RoomId,
                State = gameStatus,
            };

            roomBehaiorCpt.BroadcastAll(trunMessgae);
        }
        
        public void DealHandCard()
        {
            gameStatus = 2;

            roomBehaiorCpt.ddzDistributeCpt.DealCards();

            surCardsNum = new int[] { 17, 17, 17 }; int index = 0;

            foreach (var player in gamePlayrMap)
            {
                if (player == null) continue; if (player.IsRobat) return;

                Actor_DDZDealHandCard_Ntt ntt = new Actor_DDZDealHandCard_Ntt();
                ntt.RoomId = roomBehaiorCpt.parent.RoomId;
                ntt.Card = SetReflexPai(index);
                ntt.DpCard = SetReflexPai(3);
                surCardsNum.ToList().ForEach(a => ntt.SurCardsNum.Add(a));
                ntt.UserId = player.userId;
               
                roomBehaiorCpt.BroadPlayer(ntt,player);
                
                index++;
            }
        }

        public DDZCard SetReflexPai(int index)
        {
            DDZCard card = new DDZCard();
            switch (index)
            {
                case 0:
                    card = ChaiFen(roomBehaiorCpt.ddzDistributeCpt.player1Pokers);
                    break;
                case 1:
                    card = ChaiFen(roomBehaiorCpt.ddzDistributeCpt.player2Pokers);
                    break;
                case 2:
                    card = ChaiFen(roomBehaiorCpt.ddzDistributeCpt.player3Pokers);
                    break;
                case 3:
                    card = ChaiFen(roomBehaiorCpt.ddzDistributeCpt.dpPokers);
                    break;
                default:
                    break;
            }

            return card;
        }

        public DDZCard ChaiFen(List<byte> pokers)
        {
            DDZCard card = new DDZCard();

            if (pokers == null) return card;

            for (int i = 0; i < pokers.Count; i++)
            {
                var num = PokerCardsHelper.GetPokerNum(pokers[i]);
                card.Points = num.ToString() + "点";
                card.Value = num;
            }
            card.Card=(ByteString.CopyFrom(pokers.ToArray()));

            return card;
        }
        
        public bool CheckIsCanPass()
        {
            foreach (var item in playLastCircleCards)
                if (item.POKER_TYPE!=DDZCardType.DDZ_POKER_TYPE.DDZ_PASS) return true;

            return false;
        }

        public void SaveLastPlayerCards(long userId)
        {
            lastPlayerCards = new DDZCompareCard
            {
                userId = userId,
                POKER_TYPE =roomBehaiorCpt.ddzPokerCompareCpt.currCard.POKER_TYPE,
            };
            lastPlayerCards.pokers = new List<byte>();
            roomBehaiorCpt.ddzPokerCompareCpt.currCard.pokers.ForEach(u => lastPlayerCards.pokers.Add(u));
          
        }
        
        public void UpdataplayLastCircleCards(DDZCompareCard card)
        {
            if (playLastCircleCards.Count >= 2)
                playLastCircleCards.RemoveAt(0);

            for (int i = 0; i < playLastCircleCards.Count; i++)
                if (playLastCircleCards[i].userId == card.userId)
                    playLastCircleCards.Remove(playLastCircleCards[i]);

            playLastCircleCards.Add(card);
        }
        
    }
}
