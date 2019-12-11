using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class AreaManagerCptAwakeSystem : AwakeSystem<AreaManagerCpt>
    {
        public override void Awake(AreaManagerCpt self)
        {
            self.Awake();
        }
    }

    public  class AreaManagerCpt:Component
    {
        public List<AreaInfo> areaList ;

        public void Awake()
        {
            areaList = new List<AreaInfo>();
         
            GetAreaInfo();
        }

        private void GetAreaInfo()
        {
            if (!DBHelper<AreaInfo>.QureySingleTableAllData("AreaInfo",  areaList))
                InsertAreaData();
        }
        
        private async void InsertAreaData()
        {
            List<ComponentWithId> areaInfoList = new List<ComponentWithId>();
            try
            {
                AreaInfo DDZArea = ComponentFactory.Create<AreaInfo, int>(101);
                DDZArea.GameId = 1; DDZArea.AreaId = 101; DDZArea.AreaType = "初级场";DDZArea.Score = 1;
                areaInfoList.Add(DDZArea);
               
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                await DBSaveHelper.SaveBatch(dbProxy, areaInfoList);

                Log.Debug("向数据库插入场数据成功  !!!");

                DBHelper<AreaInfo>.QureySingleTableAllData("AreaInfo", areaList);
            }
            catch (System.Exception e)
            {
                Log.Debug($"插入游戏列表失败 信息 {e}!!!");
            }
        }
    }
}
