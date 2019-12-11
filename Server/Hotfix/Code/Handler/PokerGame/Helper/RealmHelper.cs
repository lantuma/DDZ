using ETModel;
using System.Net;
using System.Threading.Tasks;

namespace ETHotfix
{
    public static class RealmHelper
    {
        public static async Task KickOutPlayer(long userId)
        {
            int gateAppId = Game.Scene.GetComponent<OnlineComponent>().Get(userId);
            if (gateAppId != 0)
            {
                StartConfig userGateConfig = Game.Scene.GetComponent<StartConfigComponent>().Get(gateAppId);
                IPEndPoint userGateIPEndPoint = userGateConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session userGateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(userGateIPEndPoint);
                
                await userGateSession.Call(new R2G_PlayerKickOut_Req() { UserID = userId });

                await Task.Delay(1200);

                Log.Info($"玩家{userId}已被踢下线");
            }
        }

        public static void KickNotification(long userId)
        {
            ActorMessageSender actorProxy = Game.Scene.GetComponent<PlayerManagerComponent>().Get(userId).
                GetComponent<UnitGateComponent>().GetActorMessageSender();

            Actor_PlayerOffline_Ntt message = new Actor_PlayerOffline_Ntt() { PlayerOfflineTypes = 1 };

            actorProxy.Send(message);
        }

    }
}
