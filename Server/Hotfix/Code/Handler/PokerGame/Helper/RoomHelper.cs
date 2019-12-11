using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETHotfix
{
    public static class RoomHelper 
    {
        #region 
        
        private static Dictionary<int, Dictionary<int, Dictionary<int, RoomComponent>>> RoomMap =
            new Dictionary<int, Dictionary<int, Dictionary<int, RoomComponent>>>();

        #endregion

        #region 

        public static void CreateRoom()
        {
            var roomList = Game.Scene.GetComponent<LobbyManagerCpt>().GetComponent<RoomManagerCpt>().roomList;

            foreach (var room in roomList)
            {
                Dictionary<int, Dictionary<int, RoomComponent>> AreaDic;

                if (RoomMap.ContainsKey(room.GameId))
                    AreaDic = RoomMap[room.GameId];
                else
                {
                    AreaDic = new Dictionary<int, Dictionary<int, RoomComponent>>();

                    RoomMap.Add(room.GameId, AreaDic);
                }

                Dictionary<int, RoomComponent> roomDic;
               
                if (AreaDic.ContainsKey(room.AreaId))
                    roomDic = AreaDic[room.AreaId];
                else
                {
                    roomDic = new Dictionary<int, RoomComponent>();

                    AreaDic.Add(room.AreaId, roomDic);
                }

                CreateRoomComponent(room.GameId, room, roomDic);
            }
        }
        
        private static RoomComponent CreateRoomComponent(int GameId,RoomInfo roomInfo,Dictionary<int,RoomComponent> roomDict)
        {
            switch (GameId)
            {
                case 1:
                    RoomComponent DDZRoomCpt = new RoomComponent();
                    var roomId1 = RandomHelper.GenerateRandomCode(8);
                    DDZRoomCpt.Init<DDZRoomBehaiorCpt>(new Rule()
                    {
                        GameId = GameId,
                        DiFen = roomInfo.DiFen,
                        RoomId = roomId1,
                        MaxCount = roomInfo.MaxCount
                    }, DDZRoomCpt);
                    roomDict.Add(roomId1, DDZRoomCpt);
                    return DDZRoomCpt;
            }
            return null;
        }

        #endregion

        #region 
    
        public static int GetRoomState(int gameId, int areaId, int roomId)
        {
            if (RoomMap.ContainsKey(gameId))
            {
                if (RoomMap[gameId].ContainsKey(areaId))
                {
                    if (RoomMap[gameId][areaId].ContainsKey(roomId))
                    {
                        return RoomMap[gameId][areaId][roomId].GameState;
                    }
                }
            }
            return -1;
        }

        #endregion
        
        #region 

        public static int JionRoom(int gameId, int areaId, long userId, long sessionId,int RoomId , out string message)
        {
            if (!RoomMap.ContainsKey(gameId))
            {
                message = "没有找到该游戏";
                return -1;
            }
            if (!RoomMap[gameId].ContainsKey(areaId))
            {
                message = "没有找到该场次";
                return -1;
            }

            var roomDict = RoomMap[gameId][areaId]; RoomComponent roomComponen = null;

            if (RoomId != 0 )
            {
                if (!roomDict.ContainsKey(RoomId))
                {
                    message = "加入房间失败,房间已经退出";
                    return -1;
                } 
                
                roomComponen = roomDict[RoomId];
            }
            else
            {
                foreach (var room in roomDict)
                {
                    if (!room.Value.IsFull)
                    {
                        roomComponen = room.Value;
                        break;
                    }
                    else continue;
                      
                }

                if (roomComponen==null)
                {
                    roomComponen = CreateNewRoom(gameId, areaId, roomDict);
                }
            }
        
            var result = roomComponen.roomBehaviorCpt.JionRoom(userId, sessionId).Result;

            if (result != "0")
            {
                message = result;
                return -1;
            }
            else
            {
                message = "0";
                return roomComponen.RoomId;
            }
             
        }
        
        #endregion

        #region 

        public async static ETTask<string> QuitRoom(int gameId, int areaId, long userId, int roomId)
        {
            if (!RoomMap.ContainsKey(gameId))
            {
                return "没有找到该游戏";
            }
            if (!RoomMap[gameId].ContainsKey(areaId))
            {
                return "没有找到该场次";
            }
            if (!RoomMap[gameId][areaId].ContainsKey(roomId))
            {
                return "没有找到该房间";
            }

            return await RoomMap[gameId][areaId][roomId].roomBehaviorCpt.QuitRoom(userId);
        }

        #endregion

        #region

        public static object GetRoomInfo(int gameId, int areaId, long userId, int roomId)
        {
            if (!RoomMap.ContainsKey(gameId)) return null;

            if (!RoomMap[gameId].ContainsKey(areaId)) return null;

            if (!RoomMap[gameId][areaId].ContainsKey(roomId)) return null;

            return RoomMap[gameId][areaId][roomId].roomBehaviorCpt.GetRoomInfo(userId);
        }

        #endregion
        
        #region 

        public static int[] ChangerRoom(int gameId, int areaId, int roomId, long sessionId)
        {
            int[] restul = new int[2];

            if (!RoomMap.ContainsKey(gameId))
            {
                restul[0] = -1;
                return restul;
            }

            if (!RoomMap[gameId].ContainsKey(areaId))
            {
                restul[0] = -1;
                return restul;
            }

            var roomDict = RoomMap[gameId][areaId];

            if (roomDict == null)
            {
                restul[0] = -1;
                return restul;
            }

            RoomComponent roomComponen = null;

            if (roomDict.Count > 1)
            {
                foreach (var room in roomDict)
                {
                    if (room.Key == roomId || room.Value.IsFull)
                    {
                        restul[1]++;
                        continue;
                    }

                    roomComponen = room.Value;
                    break;
                }
                if (roomComponen == null)
                    roomComponen = CreateNewRoom(gameId, areaId, roomDict);
            }
            else
            {
                roomComponen = CreateNewRoom(gameId, areaId, roomDict);
                restul[1] = roomDict.Count - 1;
            }

            restul[0] = roomComponen.RoomId;

            return restul;
        }

       private  static RoomComponent CreateNewRoom(int gameId, int areaId, Dictionary<int, RoomComponent> roomDict)
        {
            RoomInfo _roomInfo = Game.Scene.GetComponent<LobbyManagerCpt>()
               .GetComponent<RoomManagerCpt>().roomList.First(u => u.AreaId == areaId);

            RoomComponent roomComponen = CreateRoomComponent(gameId, _roomInfo, roomDict);

            return roomComponen;
        }

        #endregion

        #region

        public static  List<RoomListInfo>  GetRoomList(int gameId, int areaId)
        {
            List<RoomListInfo> roomList = new List<RoomListInfo>();

            if (!RoomMap.ContainsKey(gameId)) return roomList;

            if (!RoomMap[gameId].ContainsKey(areaId)) return roomList;

            var roomDic = RoomMap[gameId][areaId];

            foreach (var room in roomDic)
            {
                RoomListInfo roomIfo = new RoomListInfo() { GameId = gameId, AreaId = areaId, RoomId = room.Value.RoomId, DiFen = room.Value.DiFen };

                roomList.Add(roomIfo);
            }
            return roomList;
        }

        #endregion

        #region 

        public static int Prepare(int gameId, int areaId, int roomId, long userId)
        {
            if (!RoomMap.ContainsKey(gameId)) return -1;

            if (!RoomMap[gameId].ContainsKey(areaId)) return -1;

            if (!RoomMap[gameId][areaId].ContainsKey(roomId)) return -1;

            return RoomMap[gameId][areaId][roomId].roomBehaviorCpt.Prepare(userId);
        }

        #endregion

        #region

        public static object AskSocre(int gameId, int areaId, int roomId, object ask)
        {
            G2C_DDZAskScore_Res res = new G2C_DDZAskScore_Res();
            if (!RoomMap.ContainsKey(gameId))
            {
                res.Error = ErrorCode.ERR_AddBetsError;
                res.Message = "不存在该游戏";
                return res;
            }
            if (!RoomMap[gameId].ContainsKey(areaId))
            {
                res.Error = ErrorCode.ERR_AddBetsError;
                res.Message = "不存在该场次";
                return res;
            }
            if (!RoomMap[gameId][areaId].ContainsKey(roomId))
            {
                res.Error = ErrorCode.ERR_AddBetsError;
                res.Message = "不存在该房间";
                return res;
            }

            return RoomMap[gameId][areaId][roomId].roomBehaviorCpt.AskScore(ask);
        }

        #endregion

        #region

        public static object PlayCard(int gameId, int areaId, int roomId, object ask)
        {
            G2C_DDZPlayCard_Res res = new G2C_DDZPlayCard_Res();
            if (!RoomMap.ContainsKey(gameId))
            {
                res.Error = ErrorCode.ERR_AddBetsError;
                res.Message = "不存在该游戏";
                return res;
            }
            if (!RoomMap[gameId].ContainsKey(areaId))
            {
                res.Error = ErrorCode.ERR_AddBetsError;
                res.Message = "不存在该场次";
                return res;
            }
            if (!RoomMap[gameId][areaId].ContainsKey(roomId))
            {
                res.Error = ErrorCode.ERR_AddBetsError;
                res.Message = "不存在该房间";
                return res;
            }

            return RoomMap[gameId][areaId][roomId].roomBehaviorCpt.PlayCard(ask);
        }

        #endregion
      
    }
}
