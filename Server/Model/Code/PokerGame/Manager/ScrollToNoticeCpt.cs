using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETHotfix;

namespace ETModel
{
    [ObjectSystem]
    public class ScrollToNoticeDataComponentAwakeSystem : AwakeSystem<ScrollToNoticeComponent>
    {
        public override void Awake(ScrollToNoticeComponent self)
        {
            self.Awake();
        }
    }
    
    public class ScrollToNoticeComponent : Entity
    {
        public List <string> mList = new List<string>();
        
        public async void Awake()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(10000);

                    if (mList.Count < 1) continue;

                    foreach (var player in Game.Scene.GetComponent<PlayerManagerComponent>().UserMap.Keys)
                    {
                        var userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(player);

                        if (userInfo == null) continue;
                       
                        ActorMessageSender actorProxy = userInfo.GetComponent<UnitGateComponent>().GetActorMessageSender();

                        Actor_ScrollToNotice_Ntt ntt = new Actor_ScrollToNotice_Ntt
                        {
                            Content = mList[0]
                        };

                        actorProxy.Send(ntt);
                    }

                    mList.RemoveAt(0);

                }
                catch (System.Exception e)
                {
                    Log.Error(e.Message);
                }
            }
        }

        public void UpdataScrollToNotice(int type,string nickName,float winMoney,string writing)
        {
            string str = "";

            if (type==0)
            {
                str = $"恭喜玩家<color=#43A6F2>{nickName}</color>在斗地主赢取<color=#F6B029>{winMoney}</color>金币!";
                mList.Add(str);
            }
           
        }

    }
}
