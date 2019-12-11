using System;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class R2G_PlayerKickOut_ReqHandler : AMRpcHandler<R2G_PlayerKickOut_Req, G2R_PlayerKickOut_Res>
    {
        protected  override async void Run(Session session, R2G_PlayerKickOut_Req message, Action<G2R_PlayerKickOut_Res> reply)
        {
            G2R_PlayerKickOut_Res response = new G2R_PlayerKickOut_Res();
            try
            {
                ETModel.UserInfo userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserID);
                
                RealmHelper.KickNotification(message.UserID);
                
                await Task.Delay(1000);
                
                long userSessionId = userInfo.GetComponent<UnitGateComponent>().GateSessionActorId;

                Game.Scene.GetComponent<NetOuterComponent>().Remove(userSessionId);

                Log.Info($"将玩家{message.UserID}连接断开");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }

        }
    }
}
