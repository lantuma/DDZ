using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ETHotfix
{
    [ObjectSystem]
    public class SessionDestroySystenComponent : DestroySystem<SessionUserComponent>
    {
        public async override void Destroy(SessionUserComponent self)
        {
            DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            try
            {
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;

                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);

                await realmSession.Call(new G2R_PlayerOffline_Req() { UserID = self.userInfo.Id });

                var myLastRoomInfo = self.userInfo.GetComponent<MyLastJionRoom>().MyLastJionRoomList;

                if (myLastRoomInfo.Count > 0)
                {
                    RoomHelper.QuitRoom(myLastRoomInfo.First().GameId, myLastRoomInfo.First().AreaId, self.userInfo.Id, myLastRoomInfo.First().RoomId);
                }
                
                self.userInfo.IsOnline = false;
               
                DateTime endTime = DateTime.Now;

                await dbProxy.Save(Game.Scene.GetComponent<PlayerManagerComponent>().Get(self.userInfo.Id));
              
                Game.Scene.GetComponent<PlayerManagerComponent>()?.Remove(self.userInfo.Id);

                self.userInfo.Dispose();

                self.userInfo = null;
                
                Game.Scene.GetComponent<PingComponent>().RemoveSession(self.sessionId);
            }
            catch (System.Exception e)
            {
                Log.Trace(e.ToString());
            }
        }
    }
}
