using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class DataBaseSaveComponetAwakeSystem : AwakeSystem<DataBaseSaveComponet>
    {
        public override void Awake(DataBaseSaveComponet self)
        {
            self.Awake();
        }
    }

    public class DataBaseSaveComponet : Component
    {
        DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

        List<ComponentWithId> userList = new List<ComponentWithId>();

        public void Awake()
        {
            TimingStorage();
        }

        public async void TimingStorage()
        {
            while (true)
            {
                userList.Clear();

                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);
                
                if (Game.Scene.GetComponent<PlayerManagerComponent>().GetAllUser().Count > 0)
                {
                    var playerList = Game.Scene.GetComponent<PlayerManagerComponent>().GetAllUser();

                    if (playerList == null) return;

                    playerList.ForEach(d => userList.Add(d));

                    await DBSaveHelper.SaveBatch(dbProxy, userList);
                }
            }
        }
    }
}
