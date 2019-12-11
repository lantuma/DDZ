using CSharpx;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class RoomComponent : Entity
    {
        #region 数据
        
        public int GameId { get; set; }
        
        public int RoomId { get; set; }
        
        public bool IsGame { get; set; }
        
        public int GameState { get; set; }

        public int[] SeatPlayer { get; set; }
        
        public int LifeTime { get; set; }
        
        public bool IsFull { get; set; }
        
        public int maxCount { get; set; }
        
        public int DiFen { get; set; }

        public int Threshold { get; set; }
        
        public RoomBehaviorComponent roomBehaviorCpt;
        
        public DateTime StartTime { get; set; }

        #endregion

        #region 

        public void Init<T>(object rule, RoomComponent roomCpt) where T : RoomBehaviorComponent, new()
        {
            roomBehaviorCpt = AddComponent<T>();

            roomBehaviorCpt.Init(rule, roomCpt);
        }

        #endregion

        #region 

        public void SetIsFull(int Count)
        {
            if (Count >= maxCount)
            {
                if (IsFull) return;
                IsFull = true;
            }
            else
            {
                if (!IsFull) return;
                IsFull = false;
            }
        }

        #endregion

        #region

        public void AddPlayerRecord(long userId, DateTime jionTime, float Income)
        {
            Game.Scene.GetComponent<PlayerManagerComponent>().Get(userId).GetComponent<MyRecord>().recordsList.Add(new ETModel.Record()
            {
                GameId = GameId,
                JionTime = DateTime.SpecifyKind(jionTime, DateTimeKind.Utc),
                QuitTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                Income = Income
            });
        }

        #endregion
        
        #region

        public async void ChangeMyLastJionRoom(long userId)
        {
            try
            {
                UserInfo Info = Game.Scene.GetComponent<PlayerManagerComponent>().Get(userId)
                  ?? await Game.Scene.GetComponent<DBProxyComponent>().Query<UserInfo>(userId);

                Info.GetComponent<MyLastJionRoom>().MyLastJionRoomList.ForEach(u => Console.WriteLine($"我即将清除我的最后房间信息的游戏id为{u.GameId}场Id为{u.AreaId}房间id{u.RoomId}"));

                if (Info.GetComponent<MyLastJionRoom>().MyLastJionRoomList.Count == 0) return;

                Info.GetComponent<MyLastJionRoom>().MyLastJionRoomList.Clear();

                await Game.Scene.GetComponent<DBProxyComponent>().Save(Info);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                throw;
            }
        }

        #endregion
    }
}

