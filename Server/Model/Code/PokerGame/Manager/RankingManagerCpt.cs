using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ETModel
{
    [ObjectSystem]
    public class RankingManagerCptAwakeSystem : AwakeSystem<RankingManagerCpt>
    {
        public override void Awake(RankingManagerCpt self)
        {
            self.Awake();
        }
    }

    public  class RankingManagerCpt:Component
    {
        public List<UserInfo> IncomeRankingList = new List<UserInfo>();
       
        public List<UserInfo> GoldRankingList= new List<UserInfo>();

        public List<UserInfo> userInfoList = new List<UserInfo>();

        public DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

        public int day;
       
        private bool IsClear { get; set; }

        public void Awake()
        {
            GetRankList();

            TimerRankinfo();
        }
        
        public void GetRankList()
        {
            userInfoList.Clear();

            if (DBHelper<UserInfo>.QureySingleTableAllData("UserInfo",  userInfoList))
            {
                IncomeRankingList = (from a in userInfoList orderby a.Scroe descending select a).Take(10).ToList();

                GoldRankingList = (from a in userInfoList orderby a.Gold descending select a).Take(10).ToList();
            }
        }

        public async void TimerRankinfo()
        {
            while (true)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);

                if (DateTime.Now.Hour == 0 && day != DateTime.Now.Day)
                    IsClear = true;

                if (IsClear)
                    ClearIncome();

                GetRankList();
            }
        }

        public async void ClearIncome()
        {
            IsClear = false; day = DateTime.Now.Day;

            userInfoList.Clear();

            if (DBHelper<UserInfo>.QureySingleTableAllData("UserInfo",  userInfoList))
            {
                List<ComponentWithId> userList = new List<ComponentWithId>();

                userInfoList.ForEach(u => { u.Scroe = 0; userList.Add(u); });

                await DBSaveHelper.SaveBatch(dbProxy, userList);
            }
        }
    }
}
