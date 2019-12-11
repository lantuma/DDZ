using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class LobbyManagerCptAwakeSystem : AwakeSystem<LobbyManagerCpt>
    {
        public override void Awake(LobbyManagerCpt self)
        {
            self.Awake();
        }
    }

    public class LobbyManagerCpt:Entity
    {
        #region 

        public List<GameInfo> gameList = new List<GameInfo>();

        public DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
        
        public  void Awake()
        {
            if (!DBHelper<GameInfo>.QureySingleTableAllData("GameInfo",  gameList))
                InsertGameData();

            this.AddComponent<AreaManagerCpt>();
            this.AddComponent<RoomManagerCpt>();
        }
        
        #endregion

        #region  

        private async void InsertGameData()
        {
            try
            {
                List<ComponentWithId> gameInfoList = new List<ComponentWithId>();
              
                ETModel.GameInfo DouDiZhukWar = ComponentFactory.CreateWithId<ETModel.GameInfo>(IdGenerater.GenerateId());
                DouDiZhukWar.GameId = 1;
                DouDiZhukWar.GameName = "斗地主";
                gameInfoList.Add(DouDiZhukWar);
                
                await DBSaveHelper.SaveBatch(dbProxy, gameInfoList);
                
                DBHelper<GameInfo>.QureySingleTableAllData("GameInfo",  gameList);
            }
            catch (System.Exception e)
            {
                Log.Debug($"插入游戏列表失败 信息 {e}!!!");
            }
        }

        #endregion
    }
}
