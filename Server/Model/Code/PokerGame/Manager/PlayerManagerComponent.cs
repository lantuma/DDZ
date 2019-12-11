using ETHotfix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ETModel
{
    [ObjectSystem]
    public class PlayerManagerComponentAwakeSystem : AwakeSystem<PlayerManagerComponent>
    {
        public override void Awake(PlayerManagerComponent self)
        {
            self.Awake();
        }
    }

    public class PlayerManagerComponent: Component 
    {
        public Dictionary<long, SessionUserComponent> UserMap;

        private Dictionary<long, long> playerMap;
        
        private List<UserInfo> userInfos = new List<UserInfo>();

        private List<UserInfo> allUserInfoList = new List<UserInfo>();
        
        public void Awake()
        {
            UserMap = new Dictionary<long, SessionUserComponent>();

            playerMap = new Dictionary<long, long>();

        }
        
        public void Add(long userId, SessionUserComponent user, long sessionId)
        {
            if (!UserMap.ContainsKey(userId))
                UserMap.Add(userId, user);

            if (!playerMap.ContainsKey(sessionId))
            {
                if (playerMap.ContainsValue(userId))
                    playerMap.Remove(playerMap.FirstOrDefault(q => q.Value == userId).Key);

                playerMap.Add(sessionId, userId);
            }
        }

        public UserInfo Get(long userId)
        {
            if (UserMap.ContainsKey(userId))
                return UserMap[userId].userInfo;

            return null;
        } 

        public SessionUserComponent GetSessionUser(long userId)
        {
            if (UserMap.ContainsKey(userId))
                return UserMap[userId];

            return null;
        }
        
        public void Remove(long userId)
        {
            this.UserMap.Remove(userId);
        }
        
        public List<UserInfo> GetAllUser()
        {
            userInfos.Clear();

            UserMap.Values?.ToList().ForEach(u => userInfos.Add(u.userInfo));
            
            return userInfos;
        }
        
        public async void RemoveSession(long sessionId)
        {
            try
            {
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);

                if (!playerMap.ContainsKey(sessionId)) return;
                long userId = playerMap[sessionId];

                var user = UserMap[userId];

                await realmSession.Call(new G2G_QuitSave_Req() { UserId = userId });

                playerMap.Remove(sessionId);

                Remove(userId);
            }
            catch (System.Exception e)
            {
                Log.Debug($"删除session时发生错误:{e.ToString()}");
            }
        }
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (var userInfo in this.UserMap.Values)
            {
                userInfo.Dispose();
            }
            
            playerMap.Clear();
        }
    }
}
