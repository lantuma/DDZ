using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class RoomManagerCptAwakeSystem : AwakeSystem<RoomManagerCpt>
    {
        public override void Awake(RoomManagerCpt self)
        {
            self.Awake();
        }
    }

    public class RoomManagerCpt:Entity
    {
        public List<RoomInfo> roomList;

        public void Awake()
        {
            roomList = new List<RoomInfo>();

            GetRoomInfo();
        }

        private  async void GetRoomInfo()
        {
            if (!DBHelper<RoomInfo>.QureySingleTableAllData("RoomInfo",  roomList))
                InsertAreaData();

            await Task.Delay(1000);

            CreateRoom();
        }

        public async void CreateRoom()
        {
            while (roomList.Count==0)
            {
                await Task.Delay(200);
            }

            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
            Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);

            await realmSession.Call(new G2G_CreateRoom_Req() { });
        }

        private async void InsertAreaData()
        {
            List<ComponentWithId> roomInfoList = new List<ComponentWithId>();
            try
            {
                int DouDiZhuRoomId = RandomHelper.RandInt32();
                RoomInfo DouDiZhuRoom = ComponentFactory.Create<RoomInfo, int>(DouDiZhuRoomId);
                DouDiZhuRoom.GameId = 1; DouDiZhuRoom.AreaId = 101; DouDiZhuRoom.RoomId = DouDiZhuRoomId; DouDiZhuRoom.DiFen = 1;
                DouDiZhuRoom.MaxCount = 3;
                roomInfoList.Add(DouDiZhuRoom);
                
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                await DBSaveHelper.SaveBatch(dbProxy, roomInfoList);

                Log.Debug("向数据库插入 房间数据成功  !!!");
                DBHelper<RoomInfo>.QureySingleTableAllData("RoomInfo",  roomList);
            }
            catch (Exception e)
            {
                Log.Debug($"插入游戏列表失败 信息 {e}!!!");
            }
        }
    }
}
