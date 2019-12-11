using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZRoomBehaiorCptUpdateSystem : UpdateSystem<DDZRoomBehaiorCpt>
    {
        public override void Update(DDZRoomBehaiorCpt self)
        {
            self.Update();
        }
    }

    public class DDZRoomBehaiorCpt : RoomBehaviorComponent
    {
        public RoomComponent parent;
        public DDZGameControlCpt gameControlCpt;
        public DDZPokerCompareCpt ddzPokerCompareCpt;
        public DDZDistributeCpt ddzDistributeCpt;
        public Dictionary<long, DDZPlayerData> PlayerMap = new Dictionary<long, DDZPlayerData>();
        DBProxyComponent dbProxyCpt;
        public int qdzCount;
        public long dzUserId;
        public int lastQdzScore;
        public List<byte> currPlayerCard;
        public bool IsWinDiZhu { get; set; }
        public float BeiShu { get; set; }
        public bool IsRespond { get; set; }
        public bool IsStartRun { get; set; }
        public bool IsTurn { get; set; }
        public bool IsSettlement { get; set; }
        public bool IsQdz { get; set; }
        public bool IsChuPai { get; set; }
        public bool IsZhunBei { get; set; }
        public bool IsdzFirstPc { get; set; }
        public int SpringCount { get; set; }
        public bool IsLj { get; set; }
      
        public override void Init(object rule, RoomComponent _parent)
        {
            var _rule = (Rule)rule;
            parent = _parent;
            parent.IsGame = true;
            parent.GameId = _rule.GameId;
            parent.DiFen = _rule.DiFen;
            parent.Threshold = _rule.Threshold;
            parent.maxCount = _rule.MaxCount;
            parent.RoomId = _rule.RoomId;
            parent.SeatPlayer = new int[3];
            currPlayerCard = new List<byte>();
            AddNeedComponent();
        }
        private void AddNeedComponent()
        {
            ddzPokerCompareCpt = this.AddComponent<DDZPokerCompareCpt>();
            gameControlCpt = this.AddComponent<DDZGameControlCpt>();
            ddzDistributeCpt = this.AddComponent<DDZDistributeCpt>();
            dbProxyCpt = Game.Scene.GetComponent<DBProxyComponent>();
        }
        
        public override void ResetGame()
        {
            List<long> keys = new List<long>(PlayerMap.Keys);

            foreach (var player in keys)
            {
                PlayerMap[player].score = 0;
                PlayerMap[player].IsLandlord = false; PlayerMap[player].IshaveOrder = false; PlayerMap[player].qdzSocre = -1;
                PlayerMap[player].Isprepare = false;PlayerMap[player].StartTime = DateTime.Now;
            }

            qdzCount = 0;               lastQdzScore = -1; dzUserId = 0;
            BeiShu = 0;                  IsWinDiZhu = false; SpringCount = 0;
            gameControlCpt.RestGame();   ddzDistributeCpt.RestGame(); ddzPokerCompareCpt.RestGame();
            foreach (var item in PlayerMap.Keys)
            {
                PlayerMap[item].Isprepare = false; PlayerMap[item].IsLandlord = false;
                PlayerMap[item].IshaveOrder = false; PlayerMap[item].qdzSocre = -1;
            }
            
            CheckIstrust();
        }
        
        public override async Task<string> JionRoom(long userId, long sessionId)
        {
            if (PlayerMap.ContainsKey(userId))
            {
                if (sessionId != PlayerMap[userId].GetComponent<UnitGateComponent>().GetActorMessageSender().ActorId)
                {
                    PlayerMap[userId].GetComponent<UnitGateComponent>().GateSessionActorId = sessionId;
                }
                
                PlayerMap[userId].IsDeposit = false;
                return "0";
            }

            if (Game.Scene.GetComponent<PlayerManagerComponent>().Get(userId).Gold < parent.DiFen) return "金额不足,无法加入房间";

            DDZPlayerData ddzPlayerData = ComponentFactory.CreateWithId<DDZPlayerData, long>(IdGenerater.GenerateId(), userId);
            
            ddzPlayerData.userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(userId);

            int playerId = ddzPlayerData.userInfo.PlayerId; ddzPlayerData.Chairld = GetChairId(playerId);
            ddzPlayerData.userId = userId;

            ddzPlayerData.JionGold = ddzPlayerData.userInfo.Gold;
            
            PlayerMap.Add(userId, ddzPlayerData);

            await ddzPlayerData.AddComponent<MailBoxComponent>().AddLocation();
            ddzPlayerData.AddComponent<UnitGateComponent, long>(sessionId);

            Actor_JionDDZRoom_Ntt message = new Actor_JionDDZRoom_Ntt() { RoomId = parent.RoomId };

            message.PlayerData = new DDZPlayerInfo()
            {
                NickeName = ddzPlayerData.userInfo.Nickname,
                UserId = userId,
                PlayerId = playerId,
                ChairId = ddzPlayerData.Chairld,
                Gold = ddzPlayerData.userInfo.Gold,
                HeadId = ddzPlayerData.userInfo.HeadId,
                Gender = ddzPlayerData.userInfo.Gender,
                IsLord=ddzPlayerData.IsLandlord,
                IsPrepare=ddzPlayerData.Isprepare,
                QdzJiaoFen=ddzPlayerData.qdzSocre,
            };

            parent.SetIsFull(PlayerMap.Count);

            BroadcastOtherPlayer(message, userId);
            
            return "0";
        }
        
        public int GetChairId(int playerId)
        {
            for (int i = 0; i < parent.SeatPlayer.Length; i++)
            {
                if (parent.SeatPlayer[i] == 0)
                {
                    parent.SeatPlayer[i] = playerId;
                    return i;
                }
            }
            return -1;
        }
        public override object GetRoomInfo(long userId)
        {
            DDZRoomData ddZRoomData = new DDZRoomData()
            {
                GameState = gameControlCpt.gameStatus, 
                QdzLifeTime = 15,
                CpLifeTime = 25, 
                JsLifeTime = 10,
                DiFen = parent.DiFen,       
                ActiveChairId = gameControlCpt.opIndex, 
                Times =(int)BeiShu,
            };
            
            ddZRoomData.SeatPlayer = new RepeatedField<int>();
            parent.SeatPlayer.ToList().ForEach(d => ddZRoomData.SeatPlayer.Add(d));
           
            ddZRoomData.PlayerData = new RepeatedField<DDZPlayerInfo>();
            PlayerMap.Values.ToList().ForEach(u => ddZRoomData.PlayerData.Add(CreatePlayerData(u)));
           
            ddZRoomData.Card = new DDZCard();
            ddZRoomData.Card = gameControlCpt.ChaiFen(ddzDistributeCpt.QuickSort(GetCurHandCards(PlayerMap[userId].Chairld)));
           
            ddZRoomData.SurCardsNum = new RepeatedField<int>();
            gameControlCpt.surCardsNum.ToList().ForEach(a => ddZRoomData.SurCardsNum.Add(a));
           
            ddZRoomData.DpCards = new DDZCard();
            ddZRoomData.DpCards = gameControlCpt.ChaiFen(GetCurHandCards(3));
           
            ddZRoomData.PlayLastCircleCards = new RepeatedField<DDZCard>();
            DDZCard card = new DDZCard();
            foreach (var item in gameControlCpt.playLastCircleCards)
            {
                card = gameControlCpt.ChaiFen(item.pokers);
                card.UserId = item.userId;
                ddZRoomData.PlayLastCircleCards.Add(card);
            }
            RoomData roomData = new RoomData() {LeftTime=parent.LifeTime, DdzRoomData=ddZRoomData };
            return roomData;
        }
        
        private DDZPlayerInfo CreatePlayerData(DDZPlayerData player)
        {
            DDZPlayerInfo ddZPlayerInfo = new DDZPlayerInfo()
            {
                UserId = player.userId,
                ChairId = player.Chairld,
                Score =  player.score,
                PlayerId = player.userInfo.PlayerId,
                NickeName = player.userInfo.Nickname,
                Gold = player.userInfo.Gold,
                HeadId = player.userInfo.HeadId,
                Gender = player.userInfo.Gender,
                IsLord = player.IsLandlord,
                IsPrepare=player.Isprepare,
                QdzJiaoFen=player.qdzSocre,
                
            };
            return ddZPlayerInfo;
        }
        
        public async override ETTask<string> QuitRoom(long userId)
        {
            if (!PlayerMap.ContainsKey(userId)) return "0";

            int playerId = PlayerMap[userId].userInfo.PlayerId;
            
            if (gameControlCpt.gameStatus != 0 && PlayerMap.ContainsKey(userId) && gameControlCpt.gameStatus != 5)
            {
                PlayerMap[userId].IsDeposit = true;

                return "1";
            }
            parent.SeatPlayer[Array.IndexOf(parent.SeatPlayer, playerId)] = 0;
            
            Actor_QuitDDZHRoom_Ntt message = new Actor_QuitDDZHRoom_Ntt();
            if (PlayerMap[userId].IsTimeOut) message.Message1 = "系统检测到玩家长时间未操作,已将玩家请出房间。";
            message.RoomId = parent.RoomId;
            message.UserId = userId;

            BroadcastAll(message);
            
            parent.ChangeMyLastJionRoom(userId);

            if (PlayerMap.ContainsKey(userId)) PlayerMap.Remove(userId);

            parent.SetIsFull(PlayerMap.Count);
            
            return "0";
        }

        
        private async ETTask StorageRecord(long userId)
        {
            if (PlayerMap[userId].IsRobat) return;
            if (PlayerMap[userId].score == 0) return;
          
            if (Game.Scene.GetComponent<PlayerManagerComponent>().Get(userId) != null)
            {
                parent.AddPlayerRecord(userId, PlayerMap[userId].StartTime, PlayerMap[userId].score);
            }
            else await SaveStorageRecordToDB(userId);
        }
        
        private async ETTask SaveStorageRecordToDB(long userId)
        {
            ETModel.UserInfo userInfo = await dbProxyCpt.Query<ETModel.UserInfo>(userId);

            AddRecord(userInfo, userId);

            await dbProxyCpt.Save(userInfo);
        }

        private void AddRecord(ETModel.UserInfo userInfo, long userId)
        {
            userInfo.GetComponent<MyRecord>().recordsList.Add(new ETModel.Record()
            {
                GameId = parent.GameId,
                JionTime = DateTime.SpecifyKind(PlayerMap[userId].StartTime, DateTimeKind.Utc),
                QuitTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                Income = PlayerMap[userId].score
            });
        }
        
        
        public DDZPlayerData GetPlayerData(int chairId)
        {
            foreach (var player in PlayerMap)
            {
                if (player.Value.Chairld != chairId) continue;

                return player.Value;
            }
            return null;
        }

        public DDZPlayerData GetPlayerData1(int playerId)
        {
            foreach (var player in PlayerMap)
            {
                if (player.Value.userInfo.PlayerId != playerId) continue;

                return player.Value;
            }
            return null;
        }
        
        public override int Prepare(long userId)
        {
            if (!PlayerMap.ContainsKey(userId)) return -1;
            
            PlayerMap[userId].Isprepare = true;
            Actor_OtherPrepare_Ntt ntt = new Actor_OtherPrepare_Ntt
            {
                RoomId=parent.RoomId,
                ChairId = PlayerMap[userId].Chairld,
                UserId = userId,
                IsPrepare=true,
            };
            
            SetPrepareStatus();
            BroadcastOtherPlayer(ntt, userId);
            return 1;
        }
        
        public void FirstQdz()
        {
            int index = RandomHelper.GetRandom().Next(0, 3);

            Actor_FirstQdz_Req_Ntt ntt = new Actor_FirstQdz_Req_Ntt
            {
                ChairId = index,
                RoomId=parent.RoomId
            };

            gameControlCpt.opIndex = index;
            gameControlCpt.operateId = gameControlCpt.gamePlayrMap[index].userId;
            
            BroadcastAll(ntt);
            OperationTime(16);
        }
        
        public override object AskScore(object parameter)
        {
            G2C_DDZAskScore_Res response = new G2C_DDZAskScore_Res();

            var bestInfo = (C2G_DDZAskScore_Req)parameter;

            if (qdzCount ==3 ) return AskScoreResponse(response, "叫地主已经结束");

            if (!PlayerMap.ContainsKey(bestInfo.UserId)) return AskScoreResponse(response, "你不在该局游戏中");

            if (PlayerMap[bestInfo.UserId].IsLandlord) return AskScoreResponse(response, "你已经是地主了");
            
            if (bestInfo.Score != 0 && bestInfo.Score <= lastQdzScore) return AskScoreResponse(response, $"当前分数已被叫过,请叫大于{bestInfo.Score}的分数");
            
            PlayerMap[bestInfo.UserId].qdzSocre = bestInfo.Score;

            response.ChairId = PlayerMap[bestInfo.UserId].Chairld;
            response.LordChairId = -1;
            
            Actor_OtherQdz_NttBroadcast(bestInfo.UserId, PlayerMap[bestInfo.UserId].Chairld, bestInfo.Score);
            
            qdzCount++;

            response.DoType = SetQdzOverStatus(bestInfo.Score);
            if(response.DoType==0)
            {
                response.NextChairId = -1;
            }
            else
                response.NextChairId = gameControlCpt.GetNextIndex(PlayerMap[bestInfo.UserId].Chairld);
            
            IsRespond = true;

            return response;
        }
        
        void Actor_OtherQdz_NttBroadcast(long userId,int chariId,int socre)
        {
            Actor_OtherQdz_Ntt ntt = new Actor_OtherQdz_Ntt
            {
                RoomId = parent.RoomId,
                ChairId = chariId,
                UserId = userId,
                Score = socre,
            };

            BroadcastAll(ntt);
        }

        
        private G2C_DDZAskScore_Res AskScoreResponse(G2C_DDZAskScore_Res response, string message)
        {
            response.Error = ErrorCode.ERR_DDZqdzError;
            response.Message = message;
            return response;
        }
       
        public override object PlayCard(object parameter)
        {
            G2C_DDZPlayCard_Res response = new G2C_DDZPlayCard_Res();

            var bestInfo = (C2G_DDZPlayCard_Req)parameter;

            if (!PlayerMap.ContainsKey(bestInfo.UserId)) return PlayCardResponse(response, "你不在该局游戏中");

            if (!PlayerMap[bestInfo.UserId].IshaveOrder) return PlayCardResponse(response, "你没有权限出牌");
            
            if (IsdzFirstPc && PlayerMap[bestInfo.UserId].IsLandlord && bestInfo.PlayCardStatue == 1) return PlayCardResponse(response, "你是地主,第一轮必须出牌");

            if (!IsdzFirstPc && !gameControlCpt.CheckIsCanPass() && bestInfo.PlayCardStatue == 1) return PlayCardResponse(response, "这轮必须先出牌");

            if (IsdzFirstPc && PlayerMap[bestInfo.UserId].IsLandlord) IsdzFirstPc = false;
            
            bool IsCanPlay = PlayCardLogic(bestInfo.PlayCardStatue, bestInfo.PlayCard, bestInfo.UserId);

            if (!IsCanPlay) return PlayCardResponse(response, "出牌有误");
            
            gameControlCpt.surCardsNum.ToList().ForEach(c => response.SurCardsNum.Add(c));
         
            response.NextChairId = gameControlCpt.GetNextIndex(gameControlCpt.opIndex);
           
            response.Card = new DDZCard();
            response.Card = gameControlCpt.ChaiFen(ddzDistributeCpt.QuickSort(GetCurHandCards(PlayerMap[bestInfo.UserId].Chairld)));
           
            response.Card.PaiXing = (int)gameControlCpt.lastPlayerCards.POKER_TYPE;
          
            response.PlayCard = new DDZCard();
            response.PlayCard = gameControlCpt.ChaiFen(currPlayerCard);
            if (response.Card.PaiXing == 13 || response.Card.PaiXing == 14) BeiShu = BeiShu * 2;
            response.Times =(int)BeiShu;
           
            Actor_OtherPlayCard_NttBroadcast(bestInfo.UserId, response.PlayCard, (int)gameControlCpt.lastPlayerCards.POKER_TYPE, gameControlCpt.surCardsNum.ToList());
          
            CheckIsGameOver(bestInfo.UserId, GetCurHandCards(gameControlCpt.opIndex).Count);
            ddzPokerCompareCpt.RestCurrCards();
            IsRespond = true;
            
            return response;
        }

        void Actor_OtherPlayCard_NttBroadcast(long userId, DDZCard playCard,int paiXing,List<int>surCardsNum)
        {
            Actor_OtherPlayCard_Ntt ntt = new Actor_OtherPlayCard_Ntt
            {  
                RoomId=parent.RoomId,
                UserId = userId,
                ChairId = PlayerMap[userId].Chairld,
                Times =(int)BeiShu
            };
            if (currPlayerCard.Count > 0)
            {
                ntt.PlayCard = new DDZCard();
                ntt.PlayCard = playCard;
                ntt.PlayCard.PaiXing = paiXing;
            }
            surCardsNum.ForEach(u => ntt.SurCardsNum.Add(u));
            BroadcastAll(ntt);
        }
        
        bool PlayCardLogic(int playCardStatus,DDZCard playCard,long userId)
        {
            int cardNum = -1; currPlayerCard.Clear();

            if(playCardStatus == 0)
            {
                playCard.Card.ToList().ForEach(u => currPlayerCard.Add(u));

                if (currPlayerCard.Count >= 4)
                    currPlayerCard = ddzPokerCompareCpt.PaiXingPaiXu(currPlayerCard);
                
                var cards = GetCurHandCards(gameControlCpt.opIndex);
                
                bool IsCanPlay = GetComponent<DDZPokerCompareCpt>().CompareCards(currPlayerCard, userId, gameControlCpt.playLastCircleCards);

                if (!IsCanPlay) return false;

                for (int i = 0; i < currPlayerCard.Count; i++)
                {
                    GetCurHandCards(gameControlCpt.opIndex).Remove(currPlayerCard[i]);
                }
            }

            cardNum = GetCurHandCards(gameControlCpt.opIndex).Count;
            gameControlCpt.surCardsNum[gameControlCpt.opIndex] = GetCurHandCards(gameControlCpt.opIndex).Count;
           
            gameControlCpt.SaveLastPlayerCards(userId);
           
            gameControlCpt.UpdataplayLastCircleCards(gameControlCpt.lastPlayerCards);
         
            gameControlCpt.gamePlayrMap[gameControlCpt.GetNextIndex(gameControlCpt.opIndex)].IshaveOrder = true;

            return true;
        }
        
        List<byte> GetCurHandCards(int index)
        {
            switch (index)
            {
                case 0:
                    return ddzDistributeCpt.player1Pokers;
                case 1:
                    return ddzDistributeCpt.player2Pokers;
                case 2:
                    return ddzDistributeCpt.player3Pokers;
                case 3:
                    return ddzDistributeCpt.dpPokers;
            }

            return null;
        }
        
        G2C_DDZPlayCard_Res PlayCardResponse(G2C_DDZPlayCard_Res response, string message)
        {
            response.Error = ErrorCode.ERR_DDZplayCardError;
            response.Message = message;
            response.PlayCard = new DDZCard();
            response.Card = new DDZCard();
            response.SurCardsNum = new RepeatedField<int>();
           
            return response;
        }
        
        public void SetPrepareStatus()
        {
            if (PlayerMap.Count < 3) return;

            int gameStatus = 1;
            foreach (var item in PlayerMap.Values)
            {
                if (!item.Isprepare)
                {
                    gameStatus = 0;
                    return;
                }
            }

            gameControlCpt.gameStatus = gameStatus;

            IsZhunBei = true;
        }
        
        int SetQdzOverStatus(int socre)
        {
            if (socre == 3 || qdzCount == 3)
            {
                var playerInfo = PlayerMap.Values.ToList().OrderByDescending(u => u.qdzSocre).ToList().First();
                if (playerInfo.qdzSocre == 0)
                {
                    gameControlCpt.gameStatus = 2;
                    
                    IsLj = true;
                   
                    CountDown(RoomState.Prepare, 2);
                    return 0;
                }
                MakeLandlord(playerInfo);
                gameControlCpt.gameStatus = 4;
                CountDown(RoomState.PlayCard, 2);
                return 2;
            }  
            else
            {
                gameControlCpt.gameStatus = 3;
                CountDown(RoomState.Turn, 1);
                return 1;
            }
        }
        
        public void MakeLandlord(DDZPlayerData playerInfo)
        {
            Actor_DDZMakeLord_Ntt ntt = new Actor_DDZMakeLord_Ntt();
            dzUserId = playerInfo.userId;
            gameControlCpt.surCardsNum[playerInfo.Chairld] = 20;
            gameControlCpt.opIndex = playerInfo.Chairld;
            gameControlCpt.operateId = playerInfo.userId;

            playerInfo.IsLandlord = true;
            PlayerMap[playerInfo.userId].IshaveOrder = true;
            IsdzFirstPc = true;

            gameControlCpt.surCardsNum.ToList().ForEach(u => ntt.CardsNum.Add(u));
            ntt.Times = playerInfo.qdzSocre;
            BeiShu = playerInfo.qdzSocre;
            ntt.LordId = playerInfo.Chairld;
            ntt.RoomId = parent.RoomId;
            BroadcastAll(ntt);
            Actor_DDZLord_Ntt ntt1 = new Actor_DDZLord_Ntt();
            List<byte> card = GetCurHandCards(playerInfo.Chairld);
            for (int i = 0; i < 3; i++)
            {
                card.Add(ddzDistributeCpt.dpPokers[i]);
            }
            ntt1.RoomId = parent.RoomId;
            card = ddzDistributeCpt.QuickSort(card);
            ntt1.HandCards = gameControlCpt.ChaiFen(card);
            DDZPlayerData playerData = GetPlayerData(playerInfo.Chairld);
            ntt1.UserId = playerData.userId;
            BroadPlayer(ntt1, playerData);
        }
        
       
        void CheckIsGameOver(long userId, int num)
        {
            if (num == 0)
            {
                bool IsSpring = true;
                if (PlayerMap[userId].IsLandlord)
                {
                    foreach (var player in PlayerMap.Values)
                    {
                        if (player.IsLandlord) continue;
                        if (GetCurHandCards(player.Chairld).Count < 17) IsSpring = false;
                    }
                    IsWinDiZhu = true;
                }
                else
                {
                    if (GetCurHandCards(PlayerMap[dzUserId].Chairld).Count < 19) IsSpring = false;
                }
                if (IsSpring)
                {
                    SpringCount = 1; BeiShu *= 2;
                }
                IsSettlement = true;
                gameControlCpt.gameStatus = 5;
                CountDown(RoomState.Settling, 2);
            }
            else if (num < 0 || num > 0)
            {
                gameControlCpt.gameStatus = 4;
               
                CountDown(RoomState.Turn, 1);
            }
        }
        
        private bool Settlement()
        {
            float Score = parent.DiFen * BeiShu;
            
            if (PlayerMap[dzUserId].userInfo.Gold < (Score * 2)) Score = PlayerMap[dzUserId].userInfo.Gold / 2;

            if (IsWinDiZhu)
            {
                float dzScore = 0;

                foreach (var player in PlayerMap.Values)
                {
                    if(!player.IsLandlord)
                    {
                        if (player.userInfo.Gold < Score) Score = player.userInfo.Gold;
                      
                        DeductionMoney(player, Score);

                        AddMoney(PlayerMap[dzUserId], Score);

                        dzScore += Score;
                    }
                }
                
                if (dzScore >= 100) Game.Scene.GetComponent<ScrollToNoticeComponent>().UpdataScrollToNotice(0, PlayerMap[dzUserId].userInfo.Nickname, dzScore, "");
            }
            else
            {
                foreach (var player in PlayerMap.Values)
                {
                    if (!player.IsLandlord)
                    {
                        if (player.userInfo.Gold < Score) Score = player.userInfo.Gold;
                      
                        AddMoney(player, Score);

                        DeductionMoney(PlayerMap[dzUserId], Score);
                        
                        if (Score >= 100) Game.Scene.GetComponent<ScrollToNoticeComponent>().UpdataScrollToNotice(0, player.userInfo.Nickname, Score,"");
                    }
                }
            }

            return false;
        }

        void AddMoney(DDZPlayerData player,float money)
        {
            player.userInfo.Gold += money;
            player.userInfo.Scroe += money;
            player.score += money;

        }

        void  DeductionMoney(DDZPlayerData player,float money)
        {
            player.userInfo.Gold -= money;
            player.userInfo.Scroe -= money;
            player.score -= money;
        }

        public void PublishResult()
        {
            Actor_DDZhSettlement_Req_Ntt ntt = new Actor_DDZhSettlement_Req_Ntt();
           
            ntt.RoomId = parent.RoomId;
           
            foreach (var player in PlayerMap.Values)
            {
                ntt.Beishu.Add((int)BeiShu);
                
                ntt.PlayerData.Add(CreatePlayerData(player));

                StorageRecord(player.userId);
               
            }
            
            ntt.OtherData.Add(PlayerMap[dzUserId].qdzSocre);
         
            ntt.OtherData.Add(ddzPokerCompareCpt.bombNum);
         
            ntt.OtherData.Add(SpringCount);
            
            ntt.ShowHand = new RepeatedField<DDZCard>();
            DDZCard card = new DDZCard();
            for (int i = 0; i < 3; i++)
            {
                card = gameControlCpt.ChaiFen(GetCurHandCards(i));
                card.UserId = gameControlCpt.gamePlayrMap[i].userId;
                ntt.ShowHand.Add(card);
            }
            
            BroadcastAll(ntt);
        }
        
        public override void BroadcastAll(IActorMessage message)
        {
            foreach (var player in PlayerMap)
            {
                if (player.Value.IsRobat) continue;
                ActorMessageSender actorProxy = player.Value.GetComponent<UnitGateComponent>().GetActorMessageSender();
                actorProxy.Send(message);
            }
        }

        public override void BroadcastOtherPlayer(IActorMessage message, long userId)
        {
            foreach (var player in PlayerMap)
            {
                if (player.Value.IsRobat) continue;
                if (player.Value.userId == userId) continue;
                ActorMessageSender actorProxy = player.Value.GetComponent<UnitGateComponent>().GetActorMessageSender();
                actorProxy.Send(message);
            }
        }

        public void BroadPlayer(IActorMessage message, DDZPlayerData PlayerData)
        {
            if (PlayerData.IsRobat) return;
            ActorMessageSender actorProxy = PlayerData.GetComponent<UnitGateComponent>().GetActorMessageSender();
            actorProxy.Send(message);
        }
        
        public async void CountDown(RoomState roomState, int timer)
        {
            parent.LifeTime = timer;
            
            while (true)
            {
                if (parent.LifeTime <= 0)
                {
                    if (roomState == RoomState.Prepare)
                    {
                        parent.GameState = 5;
                        IsStartRun = true;
                        break;
                    }
                    else if (roomState == RoomState.Qdizhu)
                    {
                        parent.GameState = 6;
                        IsQdz = true;
                        break;
                    }
                    else if (roomState == RoomState.Turn)
                    {
                        parent.GameState = 4;
                        IsTurn = true;
                        break;
                    }
                    else if (roomState == RoomState.PlayCard)
                    {
                        parent.GameState = 7;
                        IsChuPai = true;
                        break;
                    }
                    else if (roomState==RoomState.Settling)
                    {
                        IsSettlement = true;
                        parent.GameState = 2;
                        break;
                    }

                }
                parent.LifeTime--;
                
                await Task.Delay(1000);
            }
        }

        public async void OperationTime(int timer)
        {
            parent.LifeTime = timer;

            while (true)
            {
                if (IsRespond)
                {
                    IsRespond = false;
                    break;
                }

                if (parent.LifeTime <= 0)
                {
                    TimeoutOperation();
                    break;
                }
                parent.LifeTime--;

                await Task.Delay(1000);
            }
        }

     
        private void TimeoutOperation()
        {
            long userId = gameControlCpt.operateId;
            if (gameControlCpt.gameStatus==1)
            {
                QuitRoom(gameControlCpt.operateId);
            }
            else if (gameControlCpt.gameStatus == 3)
            {
                PlayerMap[userId].qdzSocre = 0;
                Actor_OtherQdz_NttBroadcast(userId, gameControlCpt.opIndex, 0);
                qdzCount++;
                SetQdzOverStatus(0);
            }
            else if (gameControlCpt.gameStatus==4)
            {
                List<byte> card = new List<byte>();
                int playCardStatus = 1;
                if (IsdzFirstPc && PlayerMap[userId].IsLandlord)
                {
                    card.Add((ddzDistributeCpt.QuickSort(GetCurHandCards(gameControlCpt.opIndex)).Last()));
                    playCardStatus = 0;
                    IsdzFirstPc = false;
                }
                else if (!IsdzFirstPc && !gameControlCpt.CheckIsCanPass())
                {
                    card.Add((ddzDistributeCpt.QuickSort(GetCurHandCards(gameControlCpt.opIndex)).Last()));
                    playCardStatus = 0;
                }
                DDZCard dZCard = gameControlCpt.ChaiFen(card);

                PlayCardLogic(playCardStatus, dZCard, userId);
                Actor_OtherPlayCard_NttBroadcast(gameControlCpt.operateId, dZCard, (int)gameControlCpt.lastPlayerCards.POKER_TYPE, gameControlCpt.surCardsNum.ToList());
                CheckIsGameOver(userId, GetCurHandCards(gameControlCpt.opIndex).Count);
                ddzPokerCompareCpt.RestCurrCards();
            }
             
        }
        
        public void Update()
        {
            Thread.Sleep(1);
            if (PlayerMap.Count ==3 && parent.IsGame && IsZhunBei)
            {
                parent.IsGame = false;
                IsZhunBei = false;
                
                parent.GameState = (int)RoomState.Prepare;IsStartRun = true;
            }
            else if (parent.GameState == (int)RoomState.Prepare && IsStartRun)
            {
                IsStartRun = false; ResetGame();
                
                if (gameControlCpt.AddGamePlayer()) return;
                
                gameControlCpt.DealHandCard();

                IsLj = false;

                CountDown(RoomState.Qdizhu, 4);
            }
            else if (parent.GameState == (int)RoomState.Qdizhu && IsQdz)
            {
                IsQdz = false;
                gameControlCpt.gameStatus = 3;
                FirstQdz();
            }
            else if (parent.GameState == (int)RoomState.PlayCard && IsChuPai)
            {
                IsChuPai = false;
                
                gameControlCpt.NotifyTrunPlayer(25);

                OperationTime(25);
            }
            else if (parent.GameState == (int)RoomState.Turn && IsTurn)
            {
                IsTurn = false;
                
                gameControlCpt.GetNextPlayer();
                
                if(gameControlCpt.gameStatus==3)
                {
                    gameControlCpt.NotifyTrunPlayer(15);
                    OperationTime(15);
                }
                else if(gameControlCpt.gameStatus==4)
                {
                    gameControlCpt.NotifyTrunPlayer(25);
                    OperationTime(25);
                }
            }
            else if (parent.GameState == (int)RoomState.Settling && IsSettlement)
            {
                IsSettlement = false;
               
                Settlement();
               
                PublishResult();

                gameControlCpt.gameStatus = 0;

                ResetGame();

                SetNextGame();
                
            }
        }
        
        void CheckIstrust()
        {
            List<long> keys = new List<long>(PlayerMap.Keys);

            foreach (var userId in keys)
            {
                if (!IsLj && PlayerMap[userId].IsDeposit || PlayerMap[userId].IsDeposit|| PlayerMap[userId].userInfo.Gold < parent.DiFen) QuitRoom(userId);
            }
        }
        
        async void SetNextGame()
        {
            int lefetime = 30;
            while (true)
            {
                if (IsZhunBei)
                {
                    IsZhunBei = false;
                    CountDown(RoomState.Prepare, 3);
                    break;
                }

                if (lefetime < 0)
                {
                    List<long> keys = new List<long>();
                    foreach (var item in PlayerMap.Values)
                    {
                        if (!item.Isprepare)
                        {
                            keys.Add(item.userId);
                        }
                    }

                    foreach (var userId in keys)
                    {
                        PlayerMap[userId].IsTimeOut = true;
                        QuitRoom(userId);
                    }

                    parent.IsGame = true;

                    break;
                }

                lefetime--;
                await Task.Delay(1000);
            }
        }
    }
}
